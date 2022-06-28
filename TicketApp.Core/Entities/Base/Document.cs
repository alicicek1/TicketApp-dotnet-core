using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketApp.Core.Entities.Base
{
    public class Document : IDocument
    {
        [BsonElement("_id")] public Guid? Id { get; set; }
        [BsonElement("createdAt")] public DateTime CreatedDate { get; set; }
        [BsonElement("updatedAt")] public DateTime UpdatedDate { get; set; }
    }
}