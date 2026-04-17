using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace TheCodeBuzzMongoDbNestedDocs
{
    internal class FirstTests
    {
        private Microsoft.Extensions.Logging.ILogger iLogger;

        public FirstTests(ILogger<FirstTests> iLogger)
        {
            this.iLogger = iLogger;
        }
        public void RunCode(IMongoCollection<LibraryUser> collection)
        {
            iLogger.LogInformation("In RunCode of FirstTests");
        }
    }
}
