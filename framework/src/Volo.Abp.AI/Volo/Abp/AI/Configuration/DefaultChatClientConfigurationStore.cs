using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.AI.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI.Configuration;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IChatClientConfigurationStore))]
public class DefaultChatClientConfigurationStore(
    IOptions<ChatClientProviderOptions> options) : IChatClientConfigurationStore
{
    protected ChatClientProviderOptions Options { get; private set; } = options.Value;

    public virtual Task<ChatClientConfigurationItem?> GetOrNullAsync(string name)
    {
        if (Options.ChatClients.TryGetValue(name, out var chatClient))
        {
            return Task.FromResult<ChatClientConfigurationItem?>(new ChatClientConfigurationItem
            {
                Provider = chatClient.Provider,
                ApiBaseUrl = chatClient.ApiBaseUrl,
                ApiKey = chatClient.ApiKey,
                ModelName = chatClient.ModelName,
                SystemPrompt = chatClient.SystemPrompt,
                Temperature = chatClient.Temperature,
                Description = chatClient.Description,
                IsActive = chatClient.IsActive,
                Name = name,
            });
        }

        return Task.FromResult<ChatClientConfigurationItem?>(null);
    }
}