using TicketApp.Application.Interfaces.Base;
using TicketApp.Application.Models.Request;
using TicketApp.Core.Entities;

namespace TicketApp.Application.Interfaces
{
    public interface IUserService : IService<UserDocument, BaseRequestModel>
    {
    }
}