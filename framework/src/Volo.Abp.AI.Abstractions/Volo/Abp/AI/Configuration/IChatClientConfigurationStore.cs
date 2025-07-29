using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AI.Configuration;

public interface IChatClientConfigurationStore
{
    Task<ChatClientConfigurationItem?> GetOrNullAsync(string name);
}