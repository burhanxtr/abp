using System.Collections.Generic;

namespace Volo.Abp.AI.Options;

public class ChatClientProviderOptions
{
    public Dictionary<string, ChatClientOptions> ChatClients { get; set; } = new();
}