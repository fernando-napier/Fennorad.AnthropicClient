using Newtonsoft.Json;

namespace Fennorad.AnthropicClient.Models
{
    public class ClaudeStreamEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("delta")]
        public Delta Delta { get; set; }

        [JsonProperty("content_block")]
        public ContentBlock ContentBlock  { get; set; }
        
    }

    public class Message
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("stop_sequence")]
        public object StopSequence { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        [JsonProperty("content")]
        public List<object> Content { get; set; }

        [JsonProperty("stop_reason")]
        public object StopReason { get; set; }
    }

    public class Delta
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("stop_reason")]
        public object StopReason { get; set; }

        [JsonProperty("stop_sequence")]
        public object StopSequence { get; set; }
    }

    public class ContentBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
