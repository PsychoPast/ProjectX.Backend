using MongoDB.Bson;

namespace ProjectX.Backend.DatabaseCollections.Serialization
{
    public interface IDocumentSerialization<TObject>
    {
        /// <summary>
        /// Deserialize the document into the given type.
        /// </summary>
        /// <param name="document">The document to deserialize.</param>
        /// <returns></returns>
        TObject Deserialize(BsonDocument document);

        /// <summary>
        /// Serialize the type into a BsonDocument.
        /// </summary>
        /// <returns></returns>
        BsonDocument Serialize();
    }
}