using System.Collections.Generic;
using System.Net;

namespace TicketApp.Core.Util.Result
{
    public class ErrorDataResult : DataResult
    {
        public ErrorDataResult(List<string> errorMessages, string errorCode, HttpStatusCode httpStatusCode)
        {
            this.IsSuccessful = false;
            this.ErrorMessageList = errorMessages;
            this.ErrorCode = errorCode;
            this.HttpStatusCode = httpStatusCode;
        }
    }
}