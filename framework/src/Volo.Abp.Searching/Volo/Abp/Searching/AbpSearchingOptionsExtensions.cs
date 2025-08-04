using System;

namespace Volo.Abp.Searching;

public static class AbpSearchingOptionsExtensions
{
    public static void AddEntity<TEntity>(this AbpSearchingOptions options, Action<EntitySearchConfiguration> configure)
        where TEntity : class, ISearchableEntity
    {
        var entitySearchConfiguration = new EntitySearchConfiguration(typeof(TEntity));
        
        configure(entitySearchConfiguration);
        
        options.Entities.Add(typeof(TEntity).FullName, entitySearchConfiguration);
    }
}