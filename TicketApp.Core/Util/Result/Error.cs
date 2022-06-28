using System;
using MongoDB.Bson;
using TicketApp.Infrastructure.Extension;

namespace TicketApp.Core.Util.Result
{
    public class Error : IDisposable
    {
        public Error()
        {
        }

        public Error(string application, string operation, string description, int httpStatusCode, int errorCode)
        {
            Application = application;
            Operation = operation;
            Description = description;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

        public string Application { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public int HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }

        public override string ToString()
        {
            return this.ToJSON();
        }

        public void Dispose()
        {
        }
    }
}