using MongoDB.Driver;
using ProjectX.Backend.DatabaseCollections;
using System.Threading.Tasks;
using MongoDB.Bson;
using ProjectX.Backend.Exceptions;

namespace ProjectX.Backend.Core
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Returns a collection manager instance of matching collection.
        /// </summary>
        /// <param name="database">The database to get the collection from.</param>
        /// <param name="collectionName">The collection name.</param>
        /// <returns></returns>
        public static async Task<ICollectionManager> GetCollectionAsync(this IMongoDatabase database, string collectionName)
        {
            FilterDefinition<BsonDocument> collFilter = new BsonDocument("name", collectionName);
            IAsyncCursor<string> collections = await database.ListCollectionNamesAsync(new ListCollectionNamesOptions() { Filter = collFilter });
            // we check if the collection exists
            if (await collections.AnyAsync())
            {
                return new CollectionManager(database.GetCollection<BsonDocument>(collectionName));
            }

            throw new CollectionNotFoundException(collectionName, database.DatabaseNamespace.DatabaseName);
        }

        /// <summary>
        /// Try to return a collection manager instance of matching collection.
        /// </summary>
        /// <param name="database">The database to get the collection from.</param>
        /// <param name="collectionName">The collection name.</param>
        /// <returns></returns>
        public static async Task<(bool exists, ICollectionManager collection)> TryGetCollectionAsync(this IMongoDatabase database, string collectionName)

        {
            ICollectionManager collection = null;
            bool exists;
            try
            {
                collection = await database.GetCollectionAsync(collectionName);
                exists = true;
            }

            catch(CollectionNotFoundException)
            {
                exists = false;
            }

            return (exists, collection);
        }
    }
}