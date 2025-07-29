using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AI;

public class ChatClientFactoryOptions
{
    public Dictionary<string, Type> ChatClientFactories { get; } = new();

    public void AddFactory<TFactory>(string provider) where TFactory : IChatClientFactory
    {
        ChatClientFactories[provider] = typeof(TFactory);
    }
}