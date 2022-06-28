using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Exceptions;

namespace TicketApp.Web.MvcFilters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case AppSettingsNotFoundException:
                    var resObj = JsonConvert.DeserializeObject<Error>(context.Exception.Message);
                    if (resObj != null)
                    {
                        context.HttpContext.Response.StatusCode = resObj.HttpStatusCode;
                        context.Result = new JsonResult(resObj);
                    }
                    else
                    {
                        context.Result = new JsonResult("Invalid error format!");
                    }

                    break;
                default:
                    var errMessage = context.Exception.Message;
                    if (context.Exception.InnerException != null)
                    {
                        errMessage = string.Join(" - ", errMessage, context.Exception.InnerException.Message);
                    }

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Result = new JsonResult(new Error("Exception handler", "Exception handling",
                        $"An unexpected error occured. - {errMessage}", (int) HttpStatusCode.InternalServerError,
                        4000));

                    break;
            }
        }
    }
}