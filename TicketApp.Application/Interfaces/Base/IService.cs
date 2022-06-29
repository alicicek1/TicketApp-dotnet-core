using System;
using System.Collections.Generic;
using TicketApp.Application.Models.Request;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Util.Filter;
using TicketApp.Core.Util.Result;

namespace TicketApp.Application.Interfaces.Base
{
    public interface IService<TDocument, TRequest>
        where TDocument : Document
        where TRequest : BaseRequestModel
    {
        List<TDocument> GetAll(DataFilter dataFilter);
        TDocument GetById(string id);
        DataResult Upsert(TRequest reqModel);
        DataResult Delete(string id);
    }
}