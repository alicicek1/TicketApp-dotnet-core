using AutoMapper;
using TicketApp.Application.Interfaces;
using TicketApp.Application.Models.Request;
using TicketApp.Application.Models.Response;
using TicketApp.Application.Services.Base;
using TicketApp.Core.Entities;
using TicketApp.Core.Repositories.Base;

namespace TicketApp.Application.Services
{
    public class UserService : Service<UserDocument, BaseRequestModel, BaseResponseModel>,
        IUserService
    {
        public UserService(IRepository<UserDocument> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}