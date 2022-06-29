namespace TicketApp.Web.Exception
{
    public class EmptyTokenException : System.Exception
    {
        public EmptyTokenException(string message) : base(message)
        {
        }
    }
}