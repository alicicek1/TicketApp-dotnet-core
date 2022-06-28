using MongoDB.Bson.Serialization.Attributes;
using TicketApp.Core.Util.Attribute;

namespace TicketApp.Core.Entities
{
    [BsonCollection("User")]
    public class UserDocument : TicketApp.Core.Entities.Base.Document
    {
        [BsonElement("username")] public string Username { get; set; }
        [BsonElement("password")] public string Password { get; set; }
        [BsonElement("email")] public string Email { get; set; }
        [BsonElement("age")] public int Age { get; set; }
        [BsonElement("type")] public byte Type { get; set; }
    }
}