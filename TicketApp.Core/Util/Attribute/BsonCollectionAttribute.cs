using System;

namespace TicketApp.Core.Util.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : System.Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            this.CollectionName = collectionName;
        }
    }
}