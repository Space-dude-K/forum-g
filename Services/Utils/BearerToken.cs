using Newtonsoft.Json;

namespace Services.Utils
{
    public sealed class BearerToken
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
