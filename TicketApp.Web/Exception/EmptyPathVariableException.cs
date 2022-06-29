namespace TicketApp.Web.Exception
{
    public class EmptyPathVariableException : System.Exception

    {
        public EmptyPathVariableException(string message) : base(message)
        {
        }
    }
}