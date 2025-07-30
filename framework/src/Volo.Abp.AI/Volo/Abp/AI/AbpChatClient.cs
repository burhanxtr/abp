using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Volo.Abp.AI.Configuration;

namespace Volo.Abp.AI;

public class AbpChatClient<T> : IChatClient<T> where T : class
{
    protected IChatClientResolver ChatClientResolver { get; }
    public string Name { get; }
    private readonly SemaphoreSlim _clientInitLock = new(1, 1);

    public AbpChatClient(IChatClientResolver chatClientResolver)
    {
        ChatClientResolver = chatClientResolver;
        Name = ChatClientNameAttribute.GetChatClientName<T>();
    }
    protected IChatClient? ChatClient { get; private set; }
    protected virtual async ValueTask EnsureClientCreatedAsync()
    {
        if (ChatClient != null)
        {
            return;
        }
        await _clientInitLock.WaitAsync();
        try
        {
            if (ChatClient != null)
            {
                return;
            }
            ChatClient = await ChatClientResolver.ResolveAsync(Name);
        }
        finally
        {
            _clientInitLock.Release();
        }
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        await EnsureClientCreatedAsync();

        return await ChatClient!.GetResponseAsync(messages, options, cancellationToken);
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return ChatClient?.GetService(serviceType, serviceKey);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureClientCreatedAsync();

        await foreach(var update in ChatClient!.GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            yield return update;
        }
    }

    public void Dispose()
    {
        ChatClient?.Dispose();
        _clientInitLock.Dispose();
    }
}

public class AbpChatClient : IChatClient
{
    public string Name { get; }
    protected ILogger<AbpChatClient> Logger { get; }
    protected IChatClient ChatClient { get; }
    protected ChatClientConfigurationItem? ChatClientConfiguration { get; }

    // Constructor for wrapping an existing chat client
    public AbpChatClient(
        IChatClient chatClient,
        string name,
        ChatClientConfigurationItem? configuration,
        ILogger<AbpChatClient> logger)
    {
        Name = name;
        Logger = logger;
        ChatClient = chatClient;
        ChatClientConfiguration = configuration;
    }

    public virtual async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        await CheckClientConfigurationAsync();
        PrepareOptions(ref options);

        return await ChatClient.GetResponseAsync(
            PrepareMessages(messages), options, cancellationToken);
    }

    public virtual async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await CheckClientConfigurationAsync();  
        PrepareOptions(ref options);

        await foreach(var update in ChatClient.GetStreamingResponseAsync(
            PrepareMessages(messages), options, cancellationToken))
        {
            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return ChatClient?.GetService(serviceType, serviceKey);
    }

    protected virtual Task CheckClientConfigurationAsync()
    {
        if(ChatClientConfiguration == null)
        {
            throw new AbpException("Chat client configuration is not set. Please set it in the configuration.");
        }

        if(!ChatClientConfiguration.IsActive)
        {
            throw new BusinessException($"The Chat Client '{Name}' is not active currently!");
        }
        return Task.CompletedTask;
    }

    protected virtual void PrepareOptions(ref ChatOptions? options)
    {
        options ??= new();
        options.Temperature ??= ChatClientConfiguration?.Temperature;
    }

    protected virtual List<ChatMessage> PrepareMessages(IEnumerable<ChatMessage> messages)
    {
        Logger.LogInformation("Preparing messages for AbpChatClient. Name: {Name}", Name);
        var messagesList = messages.ToList();

        if(messagesList.Any(x => x.Role == ChatRole.System))
        {
            // If there is a system message, skip it. It might be continued conversation.
            // No need to add a new one to prevent duplication.

            // If developer provided system message, then it's overridden, still skipping.
            Logger.LogWarning("System message is not supported in AbpChatClient. Skipping.");
            return messagesList;
        }

        if(!ChatClientConfiguration?.SystemPrompt.IsNullOrEmpty() ?? false)
        {
            messagesList.Insert(0, new ChatMessage(ChatRole.System, ChatClientConfiguration!.SystemPrompt));
            Logger.LogInformation("System message is provided in AbpChatClient. Adding to the messages list: \"{SystemPrompt}\"", ChatClientConfiguration!.SystemPrompt);
        }
        else
        {
            Logger.LogWarning("System message is not provided in AbpChatClient. Skipping.");
        }

        return messagesList;
    }

    public virtual void Dispose()
    {
        ChatClient?.Dispose();
    }
}
