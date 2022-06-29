using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketApp.Core.Entities.Base
{
    public class Document : IDocument
    {
        [BsonElement("_id")] public string? Id { get; set; }
        [BsonElement("createdAt")] public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonIgnoreIfNull]
        public DateTime UpdatedAt { get; set; }
    }
}