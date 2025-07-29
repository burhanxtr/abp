using System;
using System.Collections.Generic;

namespace Volo.Abp.AI.Options;

public class ChatClientProviderOptions
{
    public Dictionary<string, ChatClientOptions> ChatClients { get; set; } = new();
}

public class ChatClientOptions
{
    public string Provider { get; set; } = null!;
    public string ApiBaseUrl { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string? SystemPrompt { get; set; }
    public float? Temperature { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ApplicationName { get; set; }
}