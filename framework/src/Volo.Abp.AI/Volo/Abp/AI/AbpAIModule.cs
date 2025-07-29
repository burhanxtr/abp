using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Volo.Abp.AI.Configuration;
using Volo.Abp.AI.Options;
using Volo.Abp.Modularity;

namespace Volo.Abp.AI;

[DependsOn(typeof(AbpAIAbstractionsModule))]
public class AbpAIModule : AbpModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = services.GetConfiguration();

        // services.AddKeyedTransient<AbpChatClient>(
        //     (string key, IServiceProvider sp) => 
        //     new AbpChatClient(
        //         ...
        //     )
        // );

        // services.AddKeyedTransient<IChatClient, AbpChatClient>(
        //     (object key, IServiceProvider sp) => (IChatClient)sp.GetRequiredKeyedService<AbpChatClient>(key?.ToString())
        // );

        services.AddTransient(typeof(IChatClient<>), typeof(AbpChatClient<>));

        Configure((ChatClientProviderOptions options) =>
        {
            options.ChatClients = configuration.GetSection("ChatClients").Get<Dictionary<string, ChatClientOptions>>() ?? new();
        });

        return Task.CompletedTask;
    }
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions<AbpAIOptions>();

        foreach (var chatClientConfig in options.ChatClients.Values)
        {
            if (chatClientConfig.Builder == null)
            {
                throw new AbpException("ChatClientBuilder is not properly configured. Set the Builder property.");
            }

            foreach (var builderConfigurer in chatClientConfig.BuilderConfigurers)
            {
                builderConfigurer.Action(chatClientConfig.Builder);
            }

            context.Services.AddKeyedChatClient(
                AbpAIOptions.GetChatClientServiceKeyName(chatClientConfig.Name),
                provider => chatClientConfig.Builder.Build(provider)
            );

            if (chatClientConfig.Name == ChatClientConfigurationDictionary.DefaultChatClientName)
            {
                context.Services.AddTransient<IChatClient>(sp => sp.GetRequiredKeyedService<IChatClient>(
                        AbpAIOptions.GetChatClientServiceKeyName(chatClientConfig.Name)
                    )
                );
            }
        }
        
        context.Services.TryAddTransient(typeof(IChatClient<>), typeof(TypedChatClient<>));
    }
}