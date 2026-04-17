using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Serilog;
using Serilog.Core;

namespace TheCodeBuzzMongoDbNestedDocs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing MongoDB code from TheCodeBuzz.com");

            // Setup Serilog logging
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //MongoClient? mongoClient = new MongoClient("mongodb://127.0.0.1:27017/");
            MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017/");
            settings.LoggingSettings = new LoggingSettings(serviceProvider.GetService<ILoggerFactory>());
            ILogger<Program>? iLoggerForProgram = serviceProvider.GetService<ILogger<Program>>();
            if (iLoggerForProgram is not null)
            {
                iLoggerForProgram!.LogInformation("Log in Progam.cs");
            }

            // https://www.mongodb.com/docs/drivers/csharp/current/serialization/class-mapping/
            // This did NOT make a difference. 3rd level doc (the Review list)
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Review>(classmap =>
            {
                classmap.AutoMap();
                classmap.MapMember(p => p.Reviewer);
                classmap.MapMember(p => p.Grade);
            });

            var mongoClient = new MongoClient(settings);
            IMongoDatabase? iMongoDatabase = mongoClient.GetDatabase("TheCodeBuzz");

            IMongoCollection<LibraryUser> collection = CreateTheDocs(iMongoDatabase, iLoggerForProgram); // Create the Library of Users

            // Inject the Seriloger into the class
            ILogger<FirstTests>? iLoggerForFirstTests = serviceProvider.GetService<ILogger<FirstTests>>();
            FirstTests firstTests = new FirstTests(iLoggerForFirstTests);
            firstTests.RunCode(collection);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            Logger skippy = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("TheCodeBuzz-LOG.txt").CreateLogger();

            //services.AddLogging(configure => configure.AddSerilog(skippy)).AddTransient<FirstTests>(); // works
            //services.AddLogging(configure => configure.AddSerilog(skippy)).AddTransient<Program>();    // works
            services.AddLogging(configure => configure.AddSerilog(skippy));                              // works
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
        }

        public static IMongoCollection<LibraryUser> CreateTheDocs(IMongoDatabase? iMongoDatabase, ILogger<Program>? iLoggerForProgram)
        {
            IMongoCollection<LibraryUser> LibraryUsersCollection;

            bool collectionExists = iMongoDatabase.ListCollectionNames().ToList().Contains("LibraryUsers");

            if (collectionExists)
            {
                LibraryUsersCollection = iMongoDatabase!.GetCollection<LibraryUser>("LibraryUsers");
                iMongoDatabase.DropCollection("LibraryUsers");
            }

            iLoggerForProgram.LogInformation("Creating the LibraryUsers collection");

            iMongoDatabase.CreateCollection("LibraryUsers");
            LibraryUsersCollection = iMongoDatabase!.GetCollection<LibraryUser>("LibraryUsers");

            LibraryUser userDoc = new()  // First Level 
            {
                UserId = 999999
            };

            Book book = new()  // Second Level 
            {
                BookTitle = "Old Yeller",
                Price = 11
            };

            Review review = new() // Third Level 
            {
                Reviewer = "Corin",
                Grade = 55
            };
            book.AddReview(review); // Add Review doc to Book 

            userDoc.AddBook(book);  // Add Book doc to LibraryUser doc

            // Repeat for another book with 2 Reviews
            book = new()
            {
                BookTitle = "Moby Dick",
                Price = 15
            };

            review = new()
            {
                Reviewer = "Julie",
                Grade = 66
            };
            book.AddReview(review);

            review = new()
            {
                Reviewer = "Sammy",
                Grade = 78
            };
            book.AddReview(review);

            userDoc.AddBook(book);

            //var result = LibraryUsersCollection.InsertOneAsync(userDoc);
            //result.Wait();
            LibraryUsersCollection.InsertOne(userDoc);

            return LibraryUsersCollection;
        }
    }
}
