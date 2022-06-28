using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using TicketApp.Application.Interfaces.Base;
using TicketApp.Application.Models.Request;
using TicketApp.Application.Models.Response;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Repositories.Base;
using TicketApp.Core.Util.Filter;

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
                foreach (var filter in dataFilter.Filter.Split(','))
                {
                    throw new NotImplementedException();
                }
            }

            //var expParameter = Expression.Parameter(typeof(TDocument), item.Name);
            // var expParameter = Expression.
            // return Expression.Lambda<Func<TDocument, bool>>();
            return Expression.Lambda<Func<TDocument, bool>>(Expression.Constant(true), Expression.Parameter(type, "_"));
        }

        public virtual TDocument GetById(Guid id)
        {
            return _repository.FindById(id);
        }

        public virtual UpsertResponseModel Upsert(TRequest reqModel)
        {
            TDocument model = this._mapper.Map<TDocument>(reqModel);

            var result = _repository.ReplaceOne(model);
            if (result == null) return null;

            return new UpsertResponseModel
            {
                Id = result.Id.Value
            };
        }

        public virtual DeleteResponseModel Delete(TRequest reqModel)
        {
            return new DeleteResponseModel() {IsSuccessful = _repository.DeleteOneById(reqModel.Id.Value)};
        }
    }
}