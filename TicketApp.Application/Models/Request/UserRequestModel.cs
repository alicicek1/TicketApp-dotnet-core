using System;

namespace TicketApp.Application.Models.Request
{
    public class UserRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public LookupObjectModel Type { get; set; }
    }
}