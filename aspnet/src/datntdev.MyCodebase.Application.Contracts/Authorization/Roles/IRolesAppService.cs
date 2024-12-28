using Abp.Application.Services.Dto;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Roles;

public interface IRolesAppService : IMyCodebaseCrudAppService<RoleDto, int, GetAllRequestDto, CreateRequestDto, RoleDto>
{
    Task<ListResultDto<PermissionDto>> GetPermissionsAsync();
}
