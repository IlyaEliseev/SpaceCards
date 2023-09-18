using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;

namespace SpaceCards.API
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserInformation"/> class.
    /// </summary>
    /// <param name="nickname">User nickname.</param>
    /// <param name="userId">User Id.</param>
    public record UserInformation
    {
        public UserInformation(string nickname, Guid userId)
        {
            Nickname = nickname;
            UserId = userId;
        }

        [JsonProperty(ClaimTypes.Name)]
        public string Nickname { get; init; }

        [JsonProperty(ClaimTypes.NameIdentifier)]
        public Guid UserId { get; init; }
    }
}
