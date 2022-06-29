using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Util.Result;
using TicketApp.Web.Exception;

namespace TicketApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected void ValidatePathVariable(string id)
        {
            Guid pathVariable;
            if (string.IsNullOrWhiteSpace(id))
            {
                using var dataResult = new DataResult();
                using (var err = new Error("WEB", "PATH VARIABLE VALIDATION",
                           new List<string> {"Path variable was not found."},
                           50005))
                    dataResult.Error = err;
                dataResult.HttpStatusCode = (int) HttpStatusCode.NotFound;
                throw new EmptyPathVariableException(dataResult.ToString());
            }


            bool isValid = Guid.TryParse(id, out pathVariable);
            if (!isValid)
            {
                using var dataResult = new DataResult();
                using (var err = new Error("WEB", "PATH VARIABLE VALIDATION",
                           new List<string> {"Invalid path variable format."},
                           50004))
                    dataResult.Error = err;
                dataResult.HttpStatusCode = (int) HttpStatusCode.BadRequest;
                throw new InvalidPathVariableException(dataResult.ToString());
            }
        }

        protected IActionResult ApiResponse<Document>(List<Document> data)
        {
            if (data.Any())
                return Ok(data);

            using var dataResult = new DataResult();
            using (var err = new Error("WEB", "GET", new List<string> {"Not found."},
                       50002))
                dataResult.Error = err;
            dataResult.HttpStatusCode = (int) HttpStatusCode.NotFound;
            return NotFound(dataResult);
        }

        protected IActionResult ApiResponse<Document>(Document data)
        {
            if (data != null)
                return Ok(data);

            using var dataResult = new DataResult();
            using (var err = new Error("WEB", "GET", new List<string> {"Not found."},
                       50001))
                dataResult.Error = err;
            dataResult.HttpStatusCode = (int) HttpStatusCode.NotFound;
            return NotFound(dataResult);
        }
    }
}