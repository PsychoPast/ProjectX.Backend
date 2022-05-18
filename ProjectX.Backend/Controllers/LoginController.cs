using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using ProjectX.Backend.Core;
using ProjectX.Backend.DatabaseCollections;
using ProjectX.Backend.DatabaseCollections.Serialization;

namespace ProjectX.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfileHandler([FromQuery] string userDiscordId)
        {
            ICollectionManager collection = await Singleton.Instance.Database.DynamicDatabase.GetCollectionAsync(CollectionName.Login.ToString());

            var doc = await collection.FetchDocumentByValueAsync("discordUserId", userDiscordId);
            //TODO add better error broadcasting
            return doc == null ? (IActionResult)NotFound() : Ok();
        }
        
        [HttpPost("signup")]
        public async Task<IActionResult> AddProfile([FromForm] ulong avatarId, [FromForm] ulong discordId, [FromForm] string discrim, [FromForm] string userId, [FromForm] string username)
        {
            LoginModel loginInfo = new()
                                       {
                                           AvatarID = avatarId,
                                           DiscordID = discordId,
                                           Discriminator = discrim,
                                           UserID = userId,
                                           Username = username
                                       };

            ICollectionManager collection =
                await Singleton.Instance.Database.DynamicDatabase.GetCollectionAsync(CollectionName.Login.ToString());
            //TODO add better error broadcasting
            if (await collection.FetchDocumentByValueAsync("discordId", discordId) == null)
            {
                await collection.InsertDocumentAsync(loginInfo.Serialize());
                return Ok();
            }

            return Conflict();
        }
    }
}