using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectX.Backend.DatabaseCollections.Serialization;
namespace ProjectX.Backend.DatabaseCollections
{
    public class CollectionManager : ICollectionManager
    {
        private static FilterDefinition<BsonDocument> _noFilter = Builders<BsonDocument>.Filter.Empty;

        private readonly IMongoCollection<BsonDocument> _collection;

        public CollectionManager(IMongoCollection<BsonDocument> collection)
        {
            CollectionName = collection.CollectionNamespace.CollectionName;
            _collection = collection;
        }

        /// <summary>
        /// The name of the collection.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// Returns all the documents of the collection.
        /// </summary>
        /// <returns>The documents.</returns>
        public async Task<List<BsonDocument>> FetchAllDocumentsAsync() =>
            await (await _collection.FindAsync(_noFilter)).ToListAsync();

        /// <summary>
        /// Returns the document specified by the id.
        /// </summary>
        /// <param name="id">The ID of the document.</param>
        /// <returns>The document.</returns>
        public Task<BsonDocument> FetchDocumentByIdAsync(string id)
        {
            // check if the ID is null or no.
            _ = id ?? throw new ArgumentNullException(nameof(id));
            return FetchDocumentByValueAsync("_id", id);
        }

        /// <summary>
        /// Returns the document specified by the id as an object.
        /// </summary>
        /// <typeparam name="TObject">The type we wanna return.</typeparam>
        /// <param name="id">The ID of the document.</param>
        /// <returns>The object.</returns>
        public async Task<TObject> FetchDocumentByIdAsync<TObject>(string id) where TObject : IDocumentSerialization<TObject>, new()
        {
            // check if the ID is null or no.
            _ = id ?? throw new ArgumentNullException(nameof(id));
            BsonDocument doc = await FetchDocumentByIdAsync(id);
            return new TObject().Deserialize(doc);
        }

        /// <summary>
        /// Returns the first document document based on some value of a field.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldName">he field we wanna change.</param>
        /// <param name="fieldValue">The new value of field.</param>
        /// <returns></returns>
        public async Task<BsonDocument> FetchDocumentByValueAsync<TValue>(string fieldName, TValue fieldValue)
        {
            // check if the field name is null or no.
            _ = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            return await(await _collection.FindAsync(Builders<BsonDocument>.Filter.Eq(fieldName, fieldValue)))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns the first document document based on some value of a field.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldName">he field we wanna change.</param>
        /// <param name="fieldValue">The new value of field.</param>
        /// <returns></returns>
        public async Task<TObject> FetchDocumentByValueAsync<TValue, TObject>(string fieldName, TValue fieldValue) where TObject : IDocumentSerialization<TObject>, new()
        {
            // check if the field name is null or no.
            _ = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            BsonDocument doc = await FetchDocumentByValueAsync(fieldName, fieldValue);
            return new TObject().Deserialize(doc);
        }

        /// <summary>
        /// Finds and updates a document.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TSearch"></typeparam>
        /// <param name="searchField">The field name we want to find.</param>
        /// <param name="fieldName">The field we wanna change.</param>
        /// <param name="fieldValue">The new value of field.</param>
        /// <returns></returns>
        public Task FindAndUpdateDocumentAsync<TValue, TSearch>(string searchField, TSearch searchValue, string fieldName, TValue fieldValue)
        {
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set(fieldName, fieldValue);
            return _collection.UpdateOneAsync(Builders<BsonDocument>.Filter.Eq(searchField, searchValue), update);
        }

        /// <summary>
        /// Adds a document to the collection.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <returns></returns>
        public async Task InsertDocumentAsync(BsonDocument document)
        {
            // check if the document is null or no
            _ = document ?? throw new ArgumentNullException(nameof(document));
            string docId = document["_id"].AsObjectId.ToString();
            var exists = await FetchDocumentByIdAsync(docId) != null;
            if (!exists)
            {
                await _collection.InsertOneAsync(document);
            }

            // better exception
            throw new Exception("doc exists");
        }
    }
}