using Abp.Authorization;
using Abp.Domain.Uow;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace datntdev.MyCodebase.Identity;

public class SecurityStampValidator(
    IOptions<SecurityStampValidatorOptions> options,
    SignInManager signInManager,
    ILoggerFactory loggerFactory,
    IUnitOfWorkManager unitOfWorkManager
) : AbpSecurityStampValidator<Tenant, Role, User>(
    options, signInManager, loggerFactory, unitOfWorkManager)
{ }
