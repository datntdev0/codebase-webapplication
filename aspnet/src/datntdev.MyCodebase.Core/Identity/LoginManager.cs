using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.AspNetCore.Identity;

namespace datntdev.MyCodebase.Identity;

public class LoginManager(
    UserManager userManager,
    IMultiTenancyConfig multiTenancyConfig,
    IRepository<Tenant> tenantRepository,
    IUnitOfWorkManager unitOfWorkManager,
    ISettingManager settingManager,
    IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
    IUserManagementConfig userManagementConfig,
    IIocResolver iocResolver,
    IPasswordHasher<User> passwordHasher,
    RoleManager roleManager,
    UserClaimsPrincipalFactory claimsPrincipalFactory
) : AbpLogInManager<Tenant, Role, User>(
    userManager,
    multiTenancyConfig,
    tenantRepository,
    unitOfWorkManager,
    settingManager,
    userLoginAttemptRepository,
    userManagementConfig,
    iocResolver,
    passwordHasher,
    roleManager,
    claimsPrincipalFactory)
{ }
