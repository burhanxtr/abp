using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Volo.Abp.AI;

public class AbpAIAbstractionsModule : AbpModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        Configure<ChatClientFactoryOptions>(options =>
        {
            // Extensions (OpenAI, Ollama etc.) will configure this options.
        });

        return Task.CompletedTask;
    }
}