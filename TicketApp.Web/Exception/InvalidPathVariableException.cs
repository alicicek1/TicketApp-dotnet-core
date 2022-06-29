namespace TicketApp.Web.Exception
{
    public class InvalidPathVariableException : System.Exception

    {
        public InvalidPathVariableException(string message) : base(message)
        {
        }
    }
}