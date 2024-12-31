using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using datntdev.MyCodebase.Authorization.Permissions;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users.Dto;
using datntdev.MyCodebase.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

[AbpAuthorize(PermissionNames.Pages_Users)]
public class UsersAppService(
    IRepository<User, long> repository,
    UserManager userManager,
    RoleManager roleManager,
    IRepository<Role> roleRepository,
    IPasswordHasher<User> passwordHasher,
    IAbpSession abpSession,
    LoginManager logInManager
) : MyCodebaseCrudAppServicee<User, UserDto, long, Dto.GetAllUsersDto, Dto.CreateUserDto, UserDto>(repository), IUsersAppService
{
    public override async Task<UserDto> CreateAsync(Dto.CreateUserDto input)
    {
        CheckCreatePermission();

        var user = ObjectMapper.Map<User>(input);

        user.TenantId = AbpSession.TenantId;
        user.IsEmailConfirmed = true;

        await userManager.InitializeOptionsAsync(AbpSession.TenantId);

        CheckErrors(await userManager.CreateAsync(user, input.Password));

        if (input.RoleNames != null)
        {
            CheckErrors(await userManager.SetRolesAsync(user, input.RoleNames));
        }

        CurrentUnitOfWork.SaveChanges();

        return MapToEntityDto(user);
    }

    public override async Task<UserDto> UpdateAsync(UserDto input)
    {
        CheckUpdatePermission();

        var user = await userManager.GetUserByIdAsync(input.Id);

        MapToEntity(input, user);

        CheckErrors(await userManager.UpdateAsync(user));

        if (input.RoleNames != null)
        {
            CheckErrors(await userManager.SetRolesAsync(user, input.RoleNames));
        }

        return await GetAsync(input.Id);
    }

    public override async Task DeleteAsync(long id)
    {
        var user = await userManager.GetUserByIdAsync(id);
        await userManager.DeleteAsync(user);
    }

    [Route("{id}/activate")]
    public async Task ActivateAsync(long id)
    {
        await Repository.UpdateAsync(id, (entity) =>
        {
            entity.IsActive = true;
            return Task.CompletedTask;
        });
    }

    [Route("{id}/deactivate")]
    public async Task DeactivateAsync(long id)
    {
        await Repository.UpdateAsync(id, (entity) =>
        {
            entity.IsActive = false;
            return Task.CompletedTask;
        });
    }

    [Route("{id}/password")]
    public async Task ResetPasswordAsync(long id, ResetUserPasswordDto input)
    {
        if (abpSession.UserId == null)
        {
            throw new UserFriendlyException("Please log in before attempting to reset password.");
        }

        var currentUser = await userManager.GetUserByIdAsync(abpSession.GetUserId());
        var loginAsync = await logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
        if (loginAsync.Result != AbpLoginResultType.Success)
        {
            throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
        }

        var roles = await userManager.GetRolesAsync(currentUser);
        if (!roles.Contains(StaticRoleNames.Tenants.Admin))
        {
            throw new UserFriendlyException("Only administrators may reset passwords.");
        }

        var user = await userManager.GetUserByIdAsync(input.UserId)
            ?? throw new UserFriendlyException("User not found.");

        user.Password = passwordHasher.HashPassword(user, input.NewPassword);
        await CurrentUnitOfWork.SaveChangesAsync();
    }

    protected override void MapToEntity(UserDto input, User user)
    {
        ObjectMapper.Map(input, user);
        user.SetNormalizedNames();
    }

    protected override UserDto MapToEntityDto(User user)
    {
        var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

        var roles = roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

        var userDto = base.MapToEntityDto(user);
        userDto.RoleNames = roles.ToArray();

        return userDto;
    }

    protected override IQueryable<User> CreateFilteredQuery(Dto.GetAllUsersDto input)
    {
        return Repository.GetAllIncluding(x => x.Roles)
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
    }

    protected override async Task<User> GetEntityByIdAsync(long id)
    {
        var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(User), id);
        }

        return user;
    }

    protected override IQueryable<User> ApplySorting(IQueryable<User> query, Dto.GetAllUsersDto input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}

