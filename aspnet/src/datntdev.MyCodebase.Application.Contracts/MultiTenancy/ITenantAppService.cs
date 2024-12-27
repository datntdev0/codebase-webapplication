using Abp.Application.Services;
using datntdev.MyCodebase.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, GetAllRequestDto, CreateRequestDto, TenantDto>
{
    Task<IsAvailableResultDto> IsAvailableAsync(IsAvailableRequestDto input);
}
