using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketApp.Core.Entities.Base
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string? Id { get; set; }

        [BsonElement("createdAt")] DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonIgnoreIfNull]
        DateTime UpdatedAt { get; set; }
    }
}