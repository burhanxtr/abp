using System;
using System.Collections.Generic;

namespace Volo.Abp.Searching;

public static class AbpSearchingOptionsExtensions
{
    public static AbpSearchingOptions AddEntity<TEntity>(this AbpSearchingOptions options, Action<EntitySearchConfiguration> configure)
        where TEntity : class, ISearchableEntity
    {
        var entitySearchConfiguration = new EntitySearchConfiguration(typeof(TEntity));
        
        configure(entitySearchConfiguration);
        
        options.Entities.Add(typeof(TEntity).FullName!, entitySearchConfiguration);

        return options;
    }

    public static AbpSearchingOptions AddEntity<TEntity>(
        this AbpSearchingOptions options,
        params Func<TEntity, string>[] propertySelectors)
        where TEntity : class, ISearchableEntity
    {
        options.AddEntity<TEntity>(opts =>
        {
            foreach (var propertySelector in propertySelectors)
            {
                opts.SearchableProperties.Add(new SearchableProperty(propertySelector(default(TEntity)!)));
            }
        });

        return options;
    }
}