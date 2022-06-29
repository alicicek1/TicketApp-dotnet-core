using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TicketApp.Application.Interfaces;
using TicketApp.Application.Models.Request;
using TicketApp.Core.Util.Attribute;
using TicketApp.Core.Util.Filter;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Exceptions;
using TicketApp.Web.Exception;

namespace TicketApp.Web.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DataFilter dataFilter)
        {
            return ApiResponse(_userService.GetAll(dataFilter));
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {
            ValidatePathVariable(id);

            return ApiResponse(_userService.GetById(id));
        }


        //[TokenRequired]
        [HttpPost]
        public IActionResult Post([FromBody] UserRequestModel model)
        {
            //TODO model validations
            return Ok(_userService.Upsert(model));
        }

        [TokenRequired]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            ValidatePathVariable(id);
            return Ok(_userService.Delete(id));
        }
    }
}