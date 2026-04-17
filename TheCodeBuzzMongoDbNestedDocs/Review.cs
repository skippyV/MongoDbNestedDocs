using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TheCodeBuzzMongoDbNestedDocs
{
    public class Review
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Reviewer { get; set; }

        public int Grade { get; set; }

    }
}
