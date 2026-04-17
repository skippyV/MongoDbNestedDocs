using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TheCodeBuzzMongoDbNestedDocs
{
    public class Book
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string BookTitle { get; set; }

        public int Price { get; set; }

        public List<Review> Reviewers { get; set; } = [];

        public void AddReview(Review review)
        {
            Reviewers.Add(review);
        }
    }
}
