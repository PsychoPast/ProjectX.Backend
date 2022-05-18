using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using ProjectX.Backend.DatabaseCollections;
using ProjectX.Backend.DatabaseCollections.Serialization;

namespace ProjectX.Backend.Core
{
    public class LevelManager : ILevelManager
    {
        private readonly ulong _discordId;
        private readonly ICollectionManager _collection;

        public LevelManager(ulong discordId)
        {
            _discordId = discordId;
            _collection = new CollectionManager("profiles");
        }

        /// <summary>
        /// Adds a specified amount of experience to an account.
        /// </summary>
        public async Task AddExperienceAsync(ulong amt)
        {
            ProfileModel profile = await GetProfileAsync();

            await _collection.FindAndUpdateDocumentAsync("discordUserId", _discordId, "currentExperience", profile.CurrentExperience + amt);

            while (profile.CurrentExperience >= profile.NextLevelExperience)
            {
                profile.CurrentExperience -= profile.NextLevelExperience;
                await IncrementLevelAsync();
            }
        }

        /// <summary>
        /// Increments a users level by 1.
        /// </summary>
        public async Task IncrementLevelAsync()
        {
            ProfileModel profile = await GetProfileAsync();

            // increment their level.
            await _collection.FindAndUpdateDocumentAsync("discordUserId", _discordId, "level", profile.Level++);

            // shift the required experience for level over.
            await _collection.FindAndUpdateDocumentAsync("discordUserId", _discordId, "currentLevelExperience", profile.NextLevelExperience);

            // make the next level required experience a new multiple based on the previous one.
            await _collection.FindAndUpdateDocumentAsync("discordUserId", _discordId, "nextLevelExperience", (ulong) (profile.CurrentLevelExperience * 1.69));
        }

        public async Task<ProfileModel> GetProfileAsync()
        {
            BsonDocument playerDoc = await _collection.FetchDocumentByValueAsync("discordUserId", _discordId);

            ProfileModel profile = new ProfileModel();
            return profile.Deserialize(playerDoc);
        }
    }
}