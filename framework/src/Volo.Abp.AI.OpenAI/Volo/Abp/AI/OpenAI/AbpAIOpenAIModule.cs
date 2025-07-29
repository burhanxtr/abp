using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AI.OpenAI;
using Volo.Abp.Modularity;

namespace Volo.Abp.AI;

[DependsOn(typeof(AbpAIModule))]
public class AbpAIOpenAIModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<ChatClientFactoryOptions>(options =>
        {
            options.AddFactory<OpenAIChatClientFactory>(OpenAIChatClientFactory.ProviderName);
        });
    }
}