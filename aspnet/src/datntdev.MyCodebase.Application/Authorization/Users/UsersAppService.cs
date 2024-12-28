using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using datntdev.MyCodebase.Authorization.Permissions;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using datntdev.MyCodebase.Authorization.Users.Dto;
using datntdev.MyCodebase.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
) : AsyncCrudAppService<User, UserDto, long, GetAllRequestDto, Dto.CreateRequestDto, UserDto>(repository), IUsersAppService
{
    public override async Task<UserDto> CreateAsync(Dto.CreateRequestDto input)
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

        return await GetAsync(input);
    }

    public override async Task DeleteAsync(EntityDto<long> input)
    {
        var user = await userManager.GetUserByIdAsync(input.Id);
        await userManager.DeleteAsync(user);
    }

    public async Task Activate(EntityDto<long> user)
    {
        await Repository.UpdateAsync(user.Id, async (entity) =>
        {
            entity.IsActive = true;
        });
    }

    public async Task DeActivate(EntityDto<long> user)
    {
        await Repository.UpdateAsync(user.Id, async (entity) =>
        {
            entity.IsActive = false;
        });
    }

    public async Task<ListResultDto<RoleDto>> GetRoles()
    {
        var roles = await roleRepository.GetAllListAsync();
        return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
    }

    public async Task ChangeLanguage(ChangeUserLanguageDto input)
    {
        await SettingManager.ChangeSettingForUserAsync(
            AbpSession.ToUserIdentifier(),
            LocalizationSettingNames.DefaultLanguage,
            input.LanguageName
        );
    }

    protected override User MapToEntity(Dto.CreateRequestDto createInput)
    {
        var user = ObjectMapper.Map<User>(createInput);
        user.SetNormalizedNames();
        return user;
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

    protected override IQueryable<User> CreateFilteredQuery(GetAllRequestDto input)
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

    protected override IQueryable<User> ApplySorting(IQueryable<User> query, GetAllRequestDto input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }

    public async Task<bool> ChangePassword(ChangePasswordDto input)
    {
        await userManager.InitializeOptionsAsync(AbpSession.TenantId);

        var user = await userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        if (user == null)
        {
            throw new Exception("There is no current user!");
        }

        if (await userManager.CheckPasswordAsync(user, input.CurrentPassword))
        {
            CheckErrors(await userManager.ChangePasswordAsync(user, input.NewPassword));
        }
        else
        {
            CheckErrors(IdentityResult.Failed(new IdentityError
            {
                Description = "Incorrect password."
            }));
        }

        return true;
    }

    public async Task<bool> ResetPassword(ResetPasswordDto input)
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

        if (currentUser.IsDeleted || !currentUser.IsActive)
        {
            return false;
        }

        var roles = await userManager.GetRolesAsync(currentUser);
        if (!roles.Contains(StaticRoleNames.Tenants.Admin))
        {
            throw new UserFriendlyException("Only administrators may reset passwords.");
        }

        var user = await userManager.GetUserByIdAsync(input.UserId);
        if (user != null)
        {
            user.Password = passwordHasher.HashPassword(user, input.NewPassword);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        return true;
    }
}

