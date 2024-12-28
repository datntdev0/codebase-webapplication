using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using datntdev.MyCodebase.Authorization.Permissions;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using datntdev.MyCodebase.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Roles;

[AbpAuthorize(PermissionNames.Pages_Roles)]
public class RolesAppService(
    IRepository<Role> repository,
    RoleManager roleManager,
    UserManager userManager
) : MyCodebaseCrudAppServicee<Role, RoleDto, int, GetAllRequestDto, CreateRequestDto, RoleDto>(repository), IRolesAppService
{
    public override async Task<RoleDto> CreateAsync(CreateRequestDto input)
    {
        CheckCreatePermission();

        var role = ObjectMapper.Map<Role>(input);
        role.SetNormalizedName();

        CheckErrors(await roleManager.CreateAsync(role));

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    public override async Task<RoleDto> UpdateAsync(RoleDto input)
    {
        CheckUpdatePermission();

        var role = await roleManager.GetRoleByIdAsync(input.Id);

        ObjectMapper.Map(input, role);

        CheckErrors(await roleManager.UpdateAsync(role));

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    public override async Task DeleteAsync(int id)
    {
        CheckDeletePermission();

        var role = await roleManager.FindByIdAsync(id.ToString());
        var users = await userManager.GetUsersInRoleAsync(role.NormalizedName);

        foreach (var user in users)
        {
            CheckErrors(await userManager.RemoveFromRoleAsync(user, role.NormalizedName));
        }

        CheckErrors(await roleManager.DeleteAsync(role));
    }

    public Task<ListResultDto<PermissionDto>> GetPermissionsAsync()
    {
        var permissions = PermissionManager.GetAllPermissions();

        return Task.FromResult(new ListResultDto<PermissionDto>(
            ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
        ));
    }

    protected override IQueryable<Role> CreateFilteredQuery(GetAllRequestDto input)
    {
        return Repository.GetAllIncluding(x => x.Permissions)
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword))
            .WhereIf(!input.Permission.IsNullOrWhiteSpace(),
                x => x.Permissions.Any(y => y.Name.Contains(input.Permission) && y.IsGranted));
    }

    protected override async Task<Role> GetEntityByIdAsync(int id)
    {
        return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
    }

    protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, GetAllRequestDto input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}

