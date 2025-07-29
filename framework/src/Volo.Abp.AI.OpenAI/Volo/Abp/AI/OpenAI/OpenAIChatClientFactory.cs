using System;
using System.ClientModel;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Volo.Abp.DependencyInjection;
using Volo.Abp;
using Microsoft.Extensions.Logging;
using Volo.Abp.AI.Configuration;

namespace Volo.Abp.AI.OpenAI;

[ExposeServices(typeof(IChatClientFactory), typeof(OpenAIChatClientFactory))]
public class OpenAIChatClientFactory : IChatClientFactory, ITransientDependency
{
    public const string ProviderName = "OpenAI";
    public string Provider => ProviderName;

    public virtual Task<IChatClient> CreateAsync(ChatClientConfigurationItem configuration)
    {
        CheckConfiguration(configuration);

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(configuration.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = GetEndpoint(configuration.ApiBaseUrl),
            }
        );

        var client = openAIClient.GetChatClient(configuration.ModelName);

        if (client is null)
        {
            throw new UserFriendlyException($"Cannot create OpenAI chat client for '{configuration.Name}' named chat client.");
        }

        return Task.FromResult(client.AsIChatClient());
    }

    protected virtual void CheckConfiguration(ChatClientConfigurationItem configuration)
    {
        if (!configuration.IsActive)
        {
            throw new UserFriendlyException($"Chat client with name '{configuration.Name}' is not active. Provider: {ProviderName}");
        }

        if (configuration.ApiKey.IsNullOrEmpty())
        {
            throw new UserFriendlyException($"Chat client with name '{configuration.Name}' has no API key. Provider: {ProviderName}");
        }

        if (configuration.ModelName.IsNullOrEmpty())
        {
            throw new UserFriendlyException($"Chat client with name '{configuration.Name}' has no model name. Provider: {ProviderName}");
        }
    }

    protected virtual Uri GetEndpoint(string? apiBaseUrl)
    {
        if (string.IsNullOrEmpty(apiBaseUrl))
        {
            return new Uri("https://api.openai.com/v1");
        }
        
        return new Uri(apiBaseUrl);
    }
}