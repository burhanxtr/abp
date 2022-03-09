// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBlogAdminAppService), typeof(BlogAdminClientProxy))]
public partial class BlogAdminClientProxy : ClientProxyBase<IBlogAdminAppService>, IBlogAdminAppService
{
    public virtual async Task<BlogDto> GetAsync(Guid id)
    {
        return await RequestAsync<BlogDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<PagedResultDto<BlogDto>> GetListAsync(BlogGetListInput input)
    {
        return await RequestAsync<PagedResultDto<BlogDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(BlogGetListInput), input }
        });
    }

    public virtual async Task<BlogDto> CreateAsync(CreateBlogDto input)
    {
        return await RequestAsync<BlogDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(CreateBlogDto), input }
        });
    }

    public virtual async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
    {
        return await RequestAsync<BlogDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id },
            { typeof(UpdateBlogDto), input }
        });
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }
}