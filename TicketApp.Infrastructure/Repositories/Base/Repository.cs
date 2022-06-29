using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using TicketApp.Core.Configuration;
using TicketApp.Core.Util.Attribute;
using TicketApp.Core.Util.Filter;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Exceptions;
using TicketApp.Infrastructure.Extension;
using Document = System.Reflection.Metadata.Document;

namespace TicketApp.Core.Repositories.Base
{
    public class Repository<TDocument> : IRepository<TDocument>
        where TDocument : TicketApp.Core.Entities.Base.Document
    {
        private readonly IMongoCollection<TDocument> _collection;

        public Repository(IOptions<AppSettings> settings)
        {
            if (settings != null && settings.Value != null)
            {
                if (string.IsNullOrWhiteSpace(settings.Value.MongoConnectionString) ||
                    string.IsNullOrWhiteSpace(settings.Value.MongoDatabaseName))
                {
                    using var dataResult = new DataResult();
                    using (var err = new Error("Core", "Get configuration values",
                               new List<string> {"Empty configuration information."},
                               10000))
                        dataResult.Error = err;
                    dataResult.HttpStatusCode = (int) HttpStatusCode.NotFound;
                    throw new AppSettingsNotFoundException(dataResult.ToString());
                }
                else
                {
                    var database =
                        new MongoClient(settings.Value.MongoConnectionString).GetDatabase(settings.Value
                            .MongoDatabaseName);
                    _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
                }
            }
            else
            {
                using var dataResult = new DataResult();
                using (var err = new Error("Core", "Get configuration",
                           new List<string> {"There is no configuration in app settings model."},
                           10000))
                    dataResult.Error = err;
                dataResult.HttpStatusCode = (int) HttpStatusCode.NotFound;
                throw new AppSettingsNotFoundException(dataResult.ToString());
            }
        }

        private string GetCollectionName(Type documentType)
        {
            return (documentType.GetCustomAttributes()
                    .FirstOrDefault(a => a.GetType() == typeof(BsonCollectionAttribute)) as BsonCollectionAttribute)
                .CollectionName;
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression, new FindOptions() {AllowDiskUse = true}).ToEnumerable();
        }

        public IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.Find(filterExpression, new FindOptions() {AllowDiskUse = true}).ToEnumerable();
        }

        public IEnumerable<TDocument> FilterByPaginationAndSorting(Expression<Func<TDocument, bool>> filterExpression,
            DataFilter dataFilter)
        {
            var sortingField = FixField(typeof(TDocument), dataFilter.SortingProperty);
            var sortingDirection = FixDirectionFilter(dataFilter.SortingDirection);

            switch (sortingDirection)
            {
                case "1":
                    return _collection.Find(filterExpression, new FindOptions() {AllowDiskUse = true})
                        .SortBy(sortingField)
                        .Skip(dataFilter.Page)
                        .Limit(dataFilter.PageSize).ToEnumerable();
                case "-1":
                    return _collection.Find(filterExpression, new FindOptions() {AllowDiskUse = true})
                        .SortByDescending(sortingField)
                        .Skip(dataFilter.Page)
                        .Limit(dataFilter.PageSize).ToEnumerable();
                default:
                    return _collection.Find(filterExpression, new FindOptions() {AllowDiskUse = true})
                        .Skip(dataFilter.Page)
                        .Limit(dataFilter.PageSize).ToEnumerable();
            }
        }

        private Expression<Func<TDocument, object>> FixField(Type type, string dataFilterSortingProperty)
        {
            if (!string.IsNullOrWhiteSpace(dataFilterSortingProperty))
            {
                foreach (PropertyInfo item in type.GetProperties())
                {
                    if (item.Name.Trim().ToLower().Contains(dataFilterSortingProperty.ToLower().Trim()))
                    {
                        var expParameter = Expression.Parameter(typeof(TDocument), item.Name);
                        var parts = dataFilterSortingProperty.Split(".");
                        Expression parent = expParameter;
                        foreach (var part in parts)
                        {
                            parent = Expression.Property(parent, part);
                        }

                        if (parent.Type.IsValueType)
                        {
                            var converted = Expression.Convert(parent, typeof(object));
                            return Expression.Lambda<Func<TDocument, object>>(converted, expParameter);
                        }

                        return Expression.Lambda<Func<TDocument, object>>(parent, expParameter);
                    }
                }
            }

            return null;
        }


        private string FixDirectionFilter(string dataFilterSortingDirection)
        {
            if (string.IsNullOrWhiteSpace(dataFilterSortingDirection))
                return "";

            dataFilterSortingDirection = dataFilterSortingDirection.ToLower();
            if (dataFilterSortingDirection.Contains("desc") || dataFilterSortingDirection == "-1")
            {
                return "-1";
            }

            return "1";
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public long GetCount()
        {
            return _collection.CountDocuments(a => true);
        }

        public long GetCountBy(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.CountDocuments(filterExpression);
        }

        public TDocument FindById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(a => a.Id, id);
            var res = _collection.Find(filter).FirstOrDefault();
            return res;
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefault());
        }

        public Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(a => a.Id, id);
                return _collection.Find(filter).FirstOrDefault();
            });
        }

        public TDocument InsertOne(TDocument document)
        {
            document.Id = Guid.NewGuid().ToString();
            document.CreatedAt = DateTime.Now;
            _collection.InsertOne(document);
            return document;
        }

        public ICollection<TDocument> InsertMany(ICollection<TDocument> documents)
        {
            documents.ToList().ForEach(a =>
            {
                a.Id = Guid.NewGuid().ToString();
                a.CreatedAt = DateTime.Now;
            });
            _collection.InsertMany(documents);
            return documents;
        }

        public Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() =>
            {
                document.Id = Guid.NewGuid().ToString();
                document.CreatedAt = DateTime.Now;
                _collection.InsertOne(document);
                return document;
            });
        }

        public Task InsertManyAsync(ICollection<TDocument> documents)
        {
            return Task.Run(() =>
            {
                documents.ToList().ForEach(a =>
                {
                    a.Id = Guid.NewGuid().ToString();
                    a.CreatedAt = DateTime.Now;
                });
                _collection.InsertMany(documents);
                return documents;
            });
        }

        public TDocument ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(a => a.Id, document.Id);
            if (string.IsNullOrWhiteSpace(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
                document.CreatedAt = DateTime.Now;
            }

            var result = _collection.ReplaceOne(filter, document);
            if (result.ModifiedCount > 0)
            {
                return document;
            }

            return null;
        }

        public Task ReplaceOneAsync(TDocument document)
        {
            return Task.Run(() =>
            {
                _collection.ReplaceOne(Builders<TDocument>.Filter.Eq(a => a.Id, document.Id), document);
            });
        }

        public bool DeleteOneById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(a => a.Id, id);
            var result = _collection.DeleteOne(filter);
            if (result.DeletedCount > 0)
            {
                return true;
            }

            return false;
        }

        public bool DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = _collection.DeleteOne(filterExpression);
            if (result.DeletedCount > 0)
            {
                return true;
            }

            return false;
        }

        public bool DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = _collection.DeleteMany(filterExpression);
            if (result.DeletedCount > 0)
            {
                return true;
            }

            return false;
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() =>
            {
                var result = _collection.DeleteOne(filterExpression);
                if (result.DeletedCount > 0)
                {
                    return true;
                }

                return false;
            });
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(a => a.Id, id);
                var result = _collection.DeleteOne(filter);
                if (result.DeletedCount > 0)
                {
                    return true;
                }

                return false;
            });
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() =>
            {
                var result = _collection.DeleteMany(filterExpression);
                if (result.DeletedCount > 0)
                {
                    return true;
                }

                return false;
            });
        }
    }
}