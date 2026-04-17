using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TheCodeBuzzMongoDbNestedDocs
{
    public class LibraryUser
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public required int UserId { get; set; }

        public List<Book> Books { get; set; } = [];

        public void AddBook(Book book)
        {
            Books.Add(book);
        }
    }
}
