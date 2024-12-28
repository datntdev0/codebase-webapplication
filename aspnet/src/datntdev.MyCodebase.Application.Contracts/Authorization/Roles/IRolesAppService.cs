using Abp.Application.Services;
using Abp.Application.Services.Dto;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Roles;

public interface IRolesAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRequestDto, RoleDto>
{
    Task<ListResultDto<PermissionDto>> GetAllPermissions();

    Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);

    Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
}
