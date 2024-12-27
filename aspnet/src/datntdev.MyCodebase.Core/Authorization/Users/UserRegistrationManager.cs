using Abp.Authorization.Users;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.UI;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

public class UserRegistrationManager(
    TenantManager tenantManager,
    UserManager userManager,
    RoleManager roleManager) : DomainService
{
    public IAbpSession AbpSession { get; set; } = NullAbpSession.Instance;

    public async Task<User> RegisterAsync(
        string name,
        string surname,
        string emailAddress,
        string userName,
        string plainPassword,
        bool isEmailConfirmed)
    {
        CheckForTenant();

        var tenant = await GetActiveTenantAsync();

        var user = new User
        {
            TenantId = tenant.Id,
            Name = name,
            Surname = surname,
            EmailAddress = emailAddress,
            IsActive = true,
            UserName = userName,
            IsEmailConfirmed = isEmailConfirmed,
            Roles = [],
        };

        user.SetNormalizedNames();

        foreach (var defaultRole in await roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
        {
            user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
        }

        await userManager.InitializeOptionsAsync(tenant.Id);

        CheckErrors(await userManager.CreateAsync(user, plainPassword));
        await CurrentUnitOfWork.SaveChangesAsync();

        return user;
    }

    private void CheckForTenant()
    {
        if (!AbpSession.TenantId.HasValue)
        {
            throw new InvalidOperationException("Can not register host users!");
        }
    }

    private async Task<Tenant> GetActiveTenantAsync()
    {
        if (!AbpSession.TenantId.HasValue)
        {
            return null;
        }

        return await GetActiveTenantAsync(AbpSession.TenantId.Value);
    }

    private async Task<Tenant> GetActiveTenantAsync(int tenantId)
    {
        var tenant = await tenantManager.FindByIdAsync(tenantId)
            ?? throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));

        if (!tenant.IsActive)
        {
            throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
        }

        return tenant;
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}
