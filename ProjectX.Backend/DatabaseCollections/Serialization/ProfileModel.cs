using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace ProjectX.Backend.DatabaseCollections.Serialization
{
    using System;

    public class ProfileModel : IDocumentSerialization<ProfileModel>
    {
        /// <summary>
        /// The level of the player.
        /// </summary>
        public short Level { get; set; }

        /// <summary>
        /// The amount of experience the player has in the current level.
        /// </summary>
        public ulong CurrentExperience { get; set; }

        /// <summary>
        /// The experience milestone for the players current level.
        /// </summary>
        public ulong CurrentLevelExperience { get; set; }

        /// <summary>
        /// The experience milestone for the players next level.
        /// </summary>
        public ulong NextLevelExperience { get; set; }

        /// <summary>
        /// The last time the user has claimed their reward.
        /// </summary>
        public DateTime LastTimeRewardClaimed { get; set; }

        // copied from LoginModel.cs, as note says, this may also not work.
        /// <inheritdoc/>
        public ProfileModel Deserialize(BsonDocument document)
        {
            BsonDocumentReader reader = new(document);
            reader.ReadStartDocument();
            ObjectId docId = reader.ReadObjectId();
            Level = (short) reader.ReadInt32();
            CurrentExperience = (ulong) reader.ReadInt64();
            CurrentLevelExperience = (ulong) reader.ReadInt64();
            NextLevelExperience = (ulong) reader.ReadInt64();
            reader.ReadEndDocument();
            return this;
        }

        /// <inheritdoc/>
        public BsonDocument Serialize()
        {
            // we initialize the document writer
            BsonDocumentWriter docWriter = new(new BsonDocument());
            docWriter.WriteEndDocument();
            return docWriter.Document;
        }

        /// <summary>
        /// Whether or not the user can claim their reward.
        /// </summary>
        /// <param name="claimTime">The current time.</param>
        /// <param name="hoursBeforeNextClaim">Remaining hours before being able to claim again.</param>
        /// <returns></returns>
        public bool CanClaimReward(DateTime claimTime, out int hoursBeforeNextClaim)
        {
            // next claim time is basically the last time the user has claimed their reward + 24 hours
            DateTime nextClaimTime = LastTimeRewardClaimed.AddDays(1);

            hoursBeforeNextClaim = 0;
            if (claimTime >= nextClaimTime)
            {
                return true;
            }

            hoursBeforeNextClaim = (nextClaimTime - claimTime).Hours;
            return false;
        }
    }
}