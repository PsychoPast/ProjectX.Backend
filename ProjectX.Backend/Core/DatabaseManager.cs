using MongoDB.Driver;

namespace ProjectX.Backend.Core
{
    using MongoDB.Bson;

    public class DatabaseManager
    {
        /// <summary>
        /// The dynamic database.
        /// </summary>
        public IMongoDatabase DynamicDatabase { get; }

        /// <summary>
        /// The static database.
        /// </summary>
        public IMongoDatabase StaticDatabase { get; }

        public DatabaseManager(string username, string password)
        {
            MongoClient mongoClient = new($"mongodb+srv://{username}:{password}@projectx-db.vt3ak.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            // we store an instance of both databases
            DynamicDatabase = mongoClient.GetDatabase("projectx-dynamic");
            StaticDatabase = mongoClient.GetDatabase("projectx-static");
        }
    }
}