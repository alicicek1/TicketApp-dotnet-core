using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using TicketApp.Core.Util.Attribute;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Exceptions;
using TicketApp.Web.Exception;

namespace TicketApp.Web.Middleware
{
    public class TokenValidatorHandler
    {
        private readonly RequestDelegate _next;

        public TokenValidatorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

            if (endpoint != null)
            {
                var hasTokenRequiredAttribute = endpoint.Metadata.GetMetadata<TokenRequiredAttribute>();
                if (hasTokenRequiredAttribute != null)
                {
                    var token = context.Request.Headers["Authorization"];
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        using var dataResult = new DataResult();
                        using (var err = new Error("WEB", "TOKEN CHECK IN MIDDLEWARE",
                                   new List<string> {"Token is required for this endpoint."},
                                   5003))
                            dataResult.Error = err;
                        dataResult.HttpStatusCode = (int) HttpStatusCode.Forbidden;
                        ThrowException(context, dataResult);
                        return;
                    }
                }
            }

            await _next(context);
        }

        private void ThrowException(HttpContext context, DataResult dataResult)
        {
            context.Response.StatusCode = dataResult.HttpStatusCode;
            context.Response.WriteAsync(dataResult.ToString(), Encoding.UTF8).Wait();
        }
    }
}