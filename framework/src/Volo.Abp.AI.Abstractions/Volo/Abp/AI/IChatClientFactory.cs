using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Volo.Abp.AI.Configuration;

namespace Volo.Abp.AI;

public interface IChatClientFactory
{
    string Provider { get; }

    Task<IChatClient> CreateAsync(ChatClientConfigurationItem chatClientConfiguration);
}
