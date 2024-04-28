using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fennorad.AnthropicClient.Models
{
    public enum Type
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "image")]
        Image,
    }
}
