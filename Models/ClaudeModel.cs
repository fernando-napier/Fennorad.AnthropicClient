using System.Runtime.Serialization;

namespace Fennorad.AnthropicClient.Models
{
    public enum ClaudeModel
    {
        [EnumMember(Value = "claude-3-opus-20240229")]
        Claude_3_Opus,
        [EnumMember(Value = "claude-3-sonnet-20240229")]
        Claude_3_Sonnet,
        [EnumMember(Value = "claude-3-haiku-20240307")]
        Claude_3_Haiku,
    }
}
