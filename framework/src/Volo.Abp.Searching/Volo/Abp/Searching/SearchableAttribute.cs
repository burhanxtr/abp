using System;

namespace Volo.Abp.Searching;

//TODO: currently, the attribute is not used. We need to use it to mark properties as searchable later!
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class SearchableAttribute : Attribute
{
    public bool IsSearchable { get; set; }

    public SearchableAttribute(bool isSearchable = true)
    {
        IsSearchable = isSearchable;
    }
}