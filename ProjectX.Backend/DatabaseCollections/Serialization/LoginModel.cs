using System;
using MongoDB.Bson.IO;
using MongoDB.Bson;

namespace ProjectX.Backend.DatabaseCollections.Serialization
{
    public class LoginModel : IDocumentSerialization<LoginModel>
    {
        /// <summary>
        /// The user's ID.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// The user's discord ID.
        /// </summary>
        public ulong DiscordID { get; set; }

        /// <summary>
        /// When the account was created.
        /// </summary>
        public DateTime AccountCreationTime { get; private set; }

        /// <summary>
        /// The user's discord name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's discord discriminator.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// The user's avatar ID.
        /// </summary>
        public ulong AvatarID { get; set; }

        //TODO check if this actually works (I doubt it does atm but we will see)
        /// <inheritdoc/>
        public LoginModel Deserialize(BsonDocument document)
        {
            // we initialize the document reader
            BsonDocumentReader reader = new(document);
            reader.ReadStartDocument();
            ObjectId docId =  reader.ReadObjectId();
            AccountCreationTime = docId.CreationTime;
            UserID = reader.ReadString();
            DiscordID = (ulong)reader.ReadInt64();
            Username = reader.ReadString();
            Discriminator = reader.ReadString();
            AvatarID = (ulong)reader.ReadInt64();
            reader.ReadEndDocument();
            return this;
        }

        /// <inheritdoc/>
        public BsonDocument Serialize()
        {
            // we initialize the document writer
            BsonDocumentWriter doc = new(new BsonDocument());
            doc.WriteStartDocument();
            // the user id is actually the document's ID which is automatically generated
            // the account creation time is included in the document snowflake
            doc.WriteInt64("discordId", (long)DiscordID);
            doc.WriteString("username", Username);
            doc.WriteString("discriminator", Discriminator);
            doc.WriteInt64("avatarId", (long)AvatarID);
            doc.WriteEndDocument();
            return doc.Document;
        }
    }
}