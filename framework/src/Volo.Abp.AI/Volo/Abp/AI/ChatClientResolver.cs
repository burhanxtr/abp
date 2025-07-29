using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AI.Configuration;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

public class ChatClientResolver : IChatClientResolver, ITransientDependency
{
    protected ILogger<ChatClientResolver> Logger { get; }
    protected ChatClientFactoryOptions Options { get; }
    protected IChatClientConfigurationStore ChatClientConfigurationStore { get; }
    protected IServiceProvider ServiceProvider {get;}

    public ChatClientResolver(
        ILogger<ChatClientResolver> logger,
        IOptions<ChatClientFactoryOptions> chatClientConfigurationOptions,
        IChatClientConfigurationStore chatClientConfigurationStore,
        IServiceProvider serviceProvider)
    {
        Logger = logger;
        Options = chatClientConfigurationOptions.Value;
        ChatClientConfigurationStore = chatClientConfigurationStore;
        ServiceProvider = serviceProvider;
    }

    public async Task<IChatClient> ResolveAsync(string name)
    {
        var chatClientConfiguration = await ChatClientConfigurationStore.GetOrNullAsync(name);

        if(chatClientConfiguration == null)
        {
            // TODO: Proper Exception Type
            throw new Exception($"Chat client configuration with name '{name}' not found. ({ChatClientConfigurationStore.GetType().FullName}) Please configure the chat client first.");
        }

        if(!Options.ChatClientFactories.TryGetValue(chatClientConfiguration.Provider, out var factoryType))
        {
            // TODO: Proper Exception Type
            throw new UserFriendlyException($"Provider '{chatClientConfiguration.Provider}' not found! Available providers: {string.Join(", ", Options.ChatClientFactories.Keys)}");
        }

        var factory = (IChatClientFactory) ServiceProvider.GetRequiredService(factoryType);
        var underlyingClient = await factory.CreateAsync(chatClientConfiguration);

        // Wrap the underlying client in AbpChatClient proxy
        var logger = ServiceProvider.GetRequiredService<ILogger<AbpChatClient>>();
        return new AbpChatClient(underlyingClient, name, chatClientConfiguration, logger);
    }

    public Task<IChatClient> ResolveAsync<T>()
        where T : class
    {
        return ResolveAsync(typeof(T).Name);
    }
}
