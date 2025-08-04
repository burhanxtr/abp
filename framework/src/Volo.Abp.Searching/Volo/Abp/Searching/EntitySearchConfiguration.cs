using System;
using System.Collections.Generic;

namespace Volo.Abp.Searching;

public class EntitySearchConfiguration
{
    public Type EntityType { get; }

    public List<SearchableProperty> SearchableProperties { get; }

    public string? UrlPattern { get; set; }

    public EntitySearchConfiguration(Type entityType)
    {
        EntityType = entityType;
        SearchableProperties = new List<SearchableProperty>();
    }
}
