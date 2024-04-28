using Newtonsoft.Json;

namespace Fennorad.AnthropicClient.Models
{
    public class AnthropicMessage
    {
        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("content")]
        public List<Content> Content { get; set; }
    }

    public class AnthropicRequest
    {
        [JsonProperty("model")]
        public ClaudeModel Model { get; set; }

        [JsonProperty("messages")]
        public List<AnthropicMessage> Messages { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }
       
        [JsonProperty("stream")]
        public bool? StreamEvents { get; set; }

    }


}
