using Abp.Application.Services;
using datntdev.MyCodebase.MultiTenancy.Dto;

namespace datntdev.MyCodebase.MultiTenancy;

public interface ITenantsAppService : IAsyncCrudAppService<TenantDto, int, GetAllRequestDto, CreateRequestDto, TenantDto>
{
}
