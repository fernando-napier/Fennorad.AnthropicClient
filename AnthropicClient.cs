using Fennorad.AnthropicClient.Models;
using Newtonsoft.Json;

namespace Fennorad.AnthropicClient
{
    public interface IAnthropicClient
    {
        Task<AnthropicResponse> SendMessageAsync(AnthropicRequest request);
        AnthropicResponse SendMessage(AnthropicRequest request);
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
        /// Semd Claude Message Synchronously
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Anthropic requires a one to one message ratio between a user and Anthropic. Therefore one must have a one to one message history plus one user message that Claude will respond to</exception>
        /// <exception cref="Exception">When Anthropic returns an error</exception>
        public AnthropicResponse SendMessage(AnthropicRequest request)
        {
            CheckRequest(request);

            var client = GetClient();

            var requestString = JsonConvert.SerializeObject(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
            {
                Content = new StringContent(requestString, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"))
            };

            var response = client.Send(httpRequest);
            var content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error returning response: {content}");
            }

            return JsonConvert.DeserializeObject<AnthropicResponse>(content);

        }

        /// <summary>
        /// Semd Claude Message Asynchronously
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Anthropic requires a one to one message ratio between a user and Anthropic. Therefore one must have a one to one message history plus one user message that Claude will respond to</exception>
        /// <exception cref="Exception">When Anthropic returns an error</exception>
        public async Task<AnthropicResponse> SendMessageAsync(AnthropicRequest request)
        {
            CheckRequest(request);
            
            var client = GetClient();
            var requestString = JsonConvert.SerializeObject(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
            {
                Content = new StringContent(requestString, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"))
            };

            var response = await client.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error returning response: {content}");
            }

            return JsonConvert.DeserializeObject<AnthropicResponse>(content);
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
