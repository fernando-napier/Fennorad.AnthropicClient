using System.Runtime.Serialization;

namespace Fennorad.AnthropicClient.Models
{
    public enum Role
    {
        [EnumMember(Value = "user")]
        User,
        [EnumMember(Value = "assistant")]
        Assistant,
    }
}
