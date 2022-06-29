using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using TicketApp.Application.Interfaces.Base;
using TicketApp.Application.Models.Request;
using TicketApp.Application.Models.Response;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Repositories.Base;
using TicketApp.Core.Util.Filter;
using TicketApp.Core.Util.Result;

namespace TicketApp.Application.Services.Base
{
    public class Service<TDocument, TRequest, TResponse> : IService<TDocument, TRequest>
        where TDocument : Document
        where TRequest : BaseRequestModel
        where TResponse : BaseResponseModel
    {
        public List<string> Ops = new List<string>
        {
            "==", "@=", "!=", ">", "<", ">=", "<="
        };

        private readonly IMapper _mapper;
        private readonly IRepository<TDocument> _repository;

        public Service(IRepository<TDocument> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual List<TDocument> GetAll(DataFilter dataFilter)
        {
            var expressionFromFilter = CreateExpressionFromFilter(typeof(TDocument), dataFilter);
            return _repository.FilterByPaginationAndSorting(expressionFromFilter, dataFilter).ToList();
        }

        private Expression<Func<TDocument, bool>> CreateExpressionFromFilter(Type type, DataFilter dataFilter)
        {
            if (dataFilter != null && !string.IsNullOrWhiteSpace(dataFilter.Filter))
            {
                foreach (var andFilter in dataFilter.Filter.Split(","))
                {
                    // switch (andFilter[0])
                    // {
                    //     case "":
                    //         break;
                    // }

                    foreach (var orFilter in andFilter.Split('|'))
                    {
                    }
                }
            }

            //var expParameter = Expression.Parameter(typeof(TDocument), item.Name);
            // var expParameter = Expression.
            // return Expression.Lambda<Func<TDocument, bool>>();
            return Expression.Lambda<Func<TDocument, bool>>(Expression.Constant(true), Expression.Parameter(type, "_"));
        }

        public virtual TDocument GetById(string id)
        {
            return _repository.FindById(id);
        }

        public virtual DataResult Upsert(TRequest reqModel)
        {
            TDocument model = this._mapper.Map<TDocument>(reqModel);

            using var dataResult = new DataResult();
            var result = _repository.ReplaceOne(model);
            if (result == null)
            {
                using (var err = new Error("SERVICE", "DELETE", new List<string> {"Deleting action was failed."}, 4000))
                {
                    dataResult.HttpStatusCode = (int) HttpStatusCode.BadRequest;
                    dataResult.Error = err;
                }
            }

            using (var postResponse = new UpsertResponseModel())
            {
                postResponse.Id = result.Id;
                dataResult.Data = postResponse;
            }

            return dataResult;
        }

        public virtual DataResult Delete(string id)
        {
            using var dataResult = new DataResult();
            var deleteRes = _repository.DeleteOneById(id);
            if (deleteRes)
            {
                using var deletedResult = new DeleteResponseModel();
                deletedResult.IsSuccessful = true;
                dataResult.Data = deletedResult;
                return dataResult;
            }

            using (var err = new Error("SERVICE", "DELETE", new List<string> {"Deleting action was failed."},
                       4000))
            {
                dataResult.HttpStatusCode = (int) HttpStatusCode.BadRequest;
                dataResult.Error = err;
            }

            return dataResult;
        }
    }
}