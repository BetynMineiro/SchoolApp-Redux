using SchoolApp.Domain.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolApp.MongoDb.Adapter.Entities.Base;

public class MongoBaseEntity : EntityBase
{
    [BsonId] public ObjectId Id { get; set; }

}