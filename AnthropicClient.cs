using Fennorad.AnthropicClient.Models;
using Fennorad.AnthropicClient.SseParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Fennorad.AnthropicClient
{
    public interface IAnthropicClient
    {
        Task<AnthropicResponse> SendMessageAsync(AnthropicRequest request, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SseData<ClaudeStreamEvent>> SendMessageStreamAsync(AnthropicRequest request, CancellationToken cancellationToken = default);
    }

    public class AnthropicClient : IAnthropicClient
    {
        private readonly IHttpClientFactory _clientFactory;
        public AnthropicClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private HttpClient GetClient() => _clientFactory.CreateClient("anthropic");

        /// <summary>
        /// Semd Claude Message Asynchronously
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Anthropic requires a one to one message ratio between a user and Anthropic. Therefore one must have a one to one message history plus one user message that Claude will respond to</exception>
        /// <exception cref="Exception">When Anthropic returns an error</exception>
        public async Task<AnthropicResponse> SendMessageAsync(AnthropicRequest request, CancellationToken cancellationToken = default)
        {
            CheckRequest(request);

            var client = GetClient();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            var requestString = JsonConvert.SerializeObject(request, settings);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
            {
                Content = new StringContent(requestString, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(httpRequest, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error returning response: {content}");
            }

            return JsonConvert.DeserializeObject<AnthropicResponse>(content);
        }

        public async IAsyncEnumerable<SseData<ClaudeStreamEvent>> SendMessageStreamAsync(AnthropicRequest request, CancellationToken cancellationToken = default)
        {
            CheckRequest(request);

            if (request.StreamEvents == null || !request.StreamEvents.Value)
            {
                request.StreamEvents = true;
            }

            var client = GetClient();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            var requestString = JsonConvert.SerializeObject(request, settings);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
            {
                Content = new StringContent(requestString, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"))
            };

            var message = await client.SendAsync(httpRequest);
            var stream = await message.Content.ReadAsStreamAsync();

            try
            {
                using SseReader sseReader = new(stream);
                while (!cancellationToken.IsCancellationRequested)
                {
                    SseLine? sseLine = await sseReader.ReadSingleDataEventAsync(cancellationToken).ConfigureAwait(false);
                    if (sseLine == null)
                    {
                        break; // end of stream
                    }

                    ReadOnlyMemory<char> value = sseLine.Value.FieldValue;
                    if (value.Span.SequenceEqual("[DONE]".AsSpan()))
                    {
                        break;
                    }

                    var sseData = new SseData<ClaudeStreamEvent>(sseLine.Value.EventName, sseLine.Value.FieldValue);
                    if (sseData != null)
                    {
                        yield return sseData;
                    }
                }
            }
            finally
            {
                await stream.DisposeAsync().ConfigureAwait(false);
            }

        }

        private void CheckRequest(AnthropicRequest request)
        {
            if ((request.Messages.Count(x => x.Role == Role.User) - request.Messages.Count(x => x.Role == Role.Assistant)) != 1)
            {
                throw new ArgumentException("Must have a 1:1 ratio of messages between claude and the user plus a new message for claude to respond with");
            }
        }
    }
}
