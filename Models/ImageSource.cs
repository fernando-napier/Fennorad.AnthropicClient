using Newtonsoft.Json;

namespace Fennorad.AnthropicClient.Models
{
    public class ImageSource
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "base64";

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
