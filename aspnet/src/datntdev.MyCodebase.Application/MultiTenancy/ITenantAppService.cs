using Abp.Application.Services;
using datntdev.MyCodebase.MultiTenancy.Dto;

namespace datntdev.MyCodebase.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

