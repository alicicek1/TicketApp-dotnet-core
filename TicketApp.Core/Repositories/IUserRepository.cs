using TicketApp.Core.Entities;
using TicketApp.Core.Repositories.Base;

namespace TicketApp.Core.Repositories
{
    public interface IUserRepository : IRepository<UserDocument>
    {
    }
}