using System.Collections.Generic;

namespace Volo.Abp.Searching;

public class AbpSearchingOptions
{
    public Dictionary<string, EntitySearchConfiguration> Entities { get; }

    public AbpSearchingOptions()
    {
        Entities = new Dictionary<string, EntitySearchConfiguration>();
    }
}
