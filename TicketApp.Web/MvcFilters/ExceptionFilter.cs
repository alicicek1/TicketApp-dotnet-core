using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Exceptions;
using TicketApp.Web.Exception;

namespace TicketApp.Web.MvcFilters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case AppSettingsNotFoundException:
                    ManipulateReponse(context);
                    break;
                case EmptyTokenException:
                    ManipulateReponse(context);
                    break;
                case EmptyPathVariableException:
                    ManipulateReponse(context);
                    break;
                case InvalidPathVariableException:
                    ManipulateReponse(context);
                    break;
                default:
                    var errMessage = context.Exception.Message;
                    if (context.Exception.InnerException != null)
                    {
                        errMessage = string.Join(" - ", errMessage, context.Exception.InnerException.Message);
                    }

                    using (var dataResult = new DataResult())
                    {
                        using (var err = new Error("Exception handler", "Exception handling",
                                   new List<string> {$"An unexpected error occured. - {errMessage}"},
                                   4000))
                            dataResult.Error = err;
                        dataResult.HttpStatusCode = (int) HttpStatusCode.InternalServerError;

                        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        context.Result = new JsonResult(dataResult);
                    }

                    break;
            }
        }

        private void ManipulateReponse(ExceptionContext context)
        {
            var appSettingsNotFoundExceptionObj =
                JsonConvert.DeserializeObject<DataResult>(context.Exception.Message);
            if (appSettingsNotFoundExceptionObj != null)
            {
                context.HttpContext.Response.StatusCode = appSettingsNotFoundExceptionObj.HttpStatusCode;
                context.Result = new JsonResult(appSettingsNotFoundExceptionObj);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = new JsonResult($"Invalid error format! - {context.Exception.Message}");
            }
        }
    }
}