using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ProjectX.Backend.DatabaseCollections
{
    using ProjectX.Backend.DatabaseCollections.Serialization;

    public interface ICollectionManager
    {
        Task<List<BsonDocument>> FetchAllDocumentsAsync();

        Task<BsonDocument> FetchDocumentByIdAsync(string id);

        Task<TObject> FetchDocumentByIdAsync<TObject>(string id)
            where TObject : IDocumentSerialization<TObject>, new();

        Task<BsonDocument> FetchDocumentByValueAsync<TValue>(string fieldName, TValue fieldValue);

        Task<TObject> FetchDocumentByValueAsync<TValue, TObject>(string fieldName, TValue fieldValue)
            where TObject : IDocumentSerialization<TObject>, new();

        Task FindAndUpdateDocumentAsync<TValue, TSearch>(
            string searchField,
            TSearch searchValue,
            string fieldName,
            TValue fieldValue);

        Task InsertDocumentAsync(BsonDocument document);
    }
}