using Fennorad.AnthropicClient.Models;
using Newtonsoft.Json;
using Type = System.Type;

namespace Fennorad.AnthropicClient.SseParser
{
    public sealed class SseData<T>
    {
        /// <summary>
        /// The name of the sse event.
        /// </summary>
        public string? EventName { get; }

        /// <summary>
        /// Represents the type of data parsed from SSE message.
        /// </summary>
        public Type DataType { get; }

        /// <summary>
        /// Represents the data parsed from SSE message.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Represents a single Server-Sent Events (SSE) data object.
        /// </summary>
        /// <param name="eventName">The name of the sse event.</param>
        /// <param name="data">The data parsed from SSE message.</param>
        public SseData(string? eventName, object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.EventName = eventName;
            this.DataType = data.GetType();
            var charArray = (ReadOnlyMemory<char>) data;
            this.Data = JsonConvert.DeserializeObject<T>(charArray.ToString());
        }
    }
}
