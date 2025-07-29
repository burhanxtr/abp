using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace Volo.Abp.AI;

public interface IChatClientResolver
{
    Task<IChatClient> ResolveAsync(string name);
    Task<IChatClient> ResolveAsync<T>() where T : class;
}