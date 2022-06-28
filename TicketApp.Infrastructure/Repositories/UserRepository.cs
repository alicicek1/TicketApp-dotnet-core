using Microsoft.Extensions.Options;
using TicketApp.Core.Configuration;
using TicketApp.Core.Entities;
using TicketApp.Core.Repositories.Base;

namespace TicketApp.Infrastructure.Repositories
{
    public class UserRepository : Repository<UserDocument>
    {
        public UserRepository(IOptions<AppSettings> settings) : base(settings)
        {
        }
    }
}