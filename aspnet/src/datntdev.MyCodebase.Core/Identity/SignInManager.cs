using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace datntdev.MyCodebase.Identity;

public class SignInManager(
    UserManager userManager,
    IHttpContextAccessor contextAccessor,
    UserClaimsPrincipalFactory claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<User>> logger,
    IUnitOfWorkManager unitOfWorkManager,
    ISettingManager settingManager,
    IAuthenticationSchemeProvider schemes,
    IUserConfirmation<User> userConfirmation
) : AbpSignInManager<Tenant, Role, User>(
    userManager,
    contextAccessor,
    claimsFactory,
    optionsAccessor,
    logger,
    unitOfWorkManager,
    settingManager,
    schemes,
    userConfirmation)
{ }
