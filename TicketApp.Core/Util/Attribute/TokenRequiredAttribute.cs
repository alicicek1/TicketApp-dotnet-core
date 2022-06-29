using System;

namespace TicketApp.Core.Util.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class TokenRequiredAttribute : System.Attribute
    {
    }
}