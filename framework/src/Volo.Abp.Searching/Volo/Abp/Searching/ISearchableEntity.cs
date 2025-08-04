using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Searching;

public interface ISearchableEntity : IEntity
{
    string LastSearchIndexHash { get; }
}