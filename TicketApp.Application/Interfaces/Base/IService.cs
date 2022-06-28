using System;
using System.Collections.Generic;
using TicketApp.Application.Models.Request;
using TicketApp.Application.Models.Response;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Util.Filter;

namespace TicketApp.Application.Interfaces.Base
{
    public interface IService<TDocument, TRequest>
        where TDocument : Document
        where TRequest : BaseRequestModel
    {
        List<TDocument> GetAll(DataFilter dataFilter);
        TDocument GetById(Guid id);
        UpsertResponseModel Upsert(TRequest reqModel);
        DeleteResponseModel Delete(TRequest reqModel);
    }
}