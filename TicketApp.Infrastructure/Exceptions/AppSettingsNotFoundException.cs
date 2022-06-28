using System;

namespace TicketApp.Infrastructure.Exceptions
{
    public class AppSettingsNotFoundException : Exception
    {
        public AppSettingsNotFoundException(string message) : base(message)
        {
        }
    }
}