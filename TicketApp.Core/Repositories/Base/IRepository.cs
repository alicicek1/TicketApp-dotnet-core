using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Util.Filter;

namespace TicketApp.Core.Repositories.Base
{
    public interface IRepository<TDocument>
        where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
        IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression);

        IEnumerable<TDocument> FilterByPaginationAndSorting(Expression<Func<TDocument, bool>> filterExpression,
            DataFilter dataFilter);

        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        long GetCount();
        long GetCountBy(FilterDefinition<TDocument> filterExpression);

        TDocument FindById(string id);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindByIdAsync(string id);

        TDocument InsertOne(TDocument document);
        ICollection<TDocument> InsertMany(ICollection<TDocument> documents);
        Task InsertOneAsync(TDocument document);
        Task InsertManyAsync(ICollection<TDocument> documents);

        TDocument ReplaceOne(TDocument document);
        Task ReplaceOneAsync(TDocument document);

        bool DeleteOneById(string id);
        bool DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        bool DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteByIdAsync(string id);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}