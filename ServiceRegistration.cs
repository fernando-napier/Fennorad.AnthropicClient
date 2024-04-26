using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fennorad.AnthropicClient
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterAnthropicClient(this IServiceCollection services, string apiKey, string anthropicVersion = "2023-06-01")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("apiKey cannot be null");
            }
            services.AddScoped<IAnthropicClient, AnthropicClient>();
            services.AddHttpClient("anthropic", (serviceProvider, client) =>
            {
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                client.DefaultRequestHeaders.Add("anthropic-version", anthropicVersion);
                client.BaseAddress = new Uri("https://api.anthropic.com/");
            });

            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            });

            return services;
        }
    }
}
