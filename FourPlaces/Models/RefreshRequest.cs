using Newtonsoft.Json;

namespace FourPlaces.Models
{
    public class RefreshRequest
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}