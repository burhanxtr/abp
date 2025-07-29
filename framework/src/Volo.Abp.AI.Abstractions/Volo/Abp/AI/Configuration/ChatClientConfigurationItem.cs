using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AI.Configuration;

public class ChatClientConfigurationItem
{
    public string Name { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string? SystemPrompt { get; set; }

    public float? Temperature { get; set; }

    public string? ApiBaseUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}