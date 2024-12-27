using Abp.Authorization;
using Abp.Domain.Uow;
using datntdev.MyCodebase.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace datntdev.MyCodebase.Authorization.Users;

public class UserClaimsPrincipalFactory(
    UserManager userManager,
    RoleManager roleManager,
    IOptions<IdentityOptions> optionsAccessor,
    IUnitOfWorkManager unitOfWorkManager
) : AbpUserClaimsPrincipalFactory<User, Role>(
    userManager,
    roleManager,
    optionsAccessor,
    unitOfWorkManager)
{ }