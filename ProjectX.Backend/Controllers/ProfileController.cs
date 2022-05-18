using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectX.Backend.Core;
using ProjectX.Backend.Core.Models;
using ProjectX.Backend.DatabaseCollections.Serialization;

namespace ProjectX.Backend.Controllers
{
    [Route("/api/v1/[controller]")]
    public class ProfileController : ControllerBase
    {
        [HttpPost]
        [Route("claimDaily")]
        public async Task<IActionResult> DailyRewardsHandler([FromHeader] string discordId)
        {
            // time the user made the request
            DateTime currentTime = DateTime.Now;

            var collection = await Singleton.Instance.Database.DynamicDatabase.GetCollectionAsync(CollectionName.Profiles.ToString());

            BsonDocument doc = await collection.FetchDocumentByValueAsync("discordId", discordId);

            // if the method returns null, it means the user doesn't have a profile saved in the database
            if (doc == null)
            {
                var error = new HttpError()
                                {
                                    Message = $"User {discordId} doesn't have an account.",
                                    Service = "DailyRewardsClaiming",
                                    StatusCode = 404,
                                    InternalCode = 14069
                                };

                return NotFound(new JsonResult(error));
            }

            ProfileModel user = new ProfileModel().Deserialize(new(doc));

            // we check if the user can claim their reward.
            if (!user.CanClaimReward(currentTime, out int hoursBeforeNextClaim))
            {
                var error = new HttpError()
                                {
                                    Message = $"User {discordId} has already claimed their reward today. Come back in {hoursBeforeNextClaim} hours.",
                                    Service = "DailyRewardsClaiming",
                                    StatusCode = 400,
                                    InternalCode = 14070
                                };

                return BadRequest(error);
            }

            //TODO Write Reward Handling Logic

            // we save the new time in the database
            user.LastTimeRewardClaimed = DateTime.Now;
            await collection.InsertDocumentAsync(user.Serialize());
            return Ok();
        }
    }
}
