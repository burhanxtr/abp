namespace Volo.Abp.Searching;

public class SearchableProperty
{
    public string Name { get; }

    public SearchableProperty(string name)
    {
        Name = name;
    }
}