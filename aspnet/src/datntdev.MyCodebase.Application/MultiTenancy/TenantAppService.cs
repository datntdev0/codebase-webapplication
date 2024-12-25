using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using datntdev.MyCodebase.Authorization;
using datntdev.MyCodebase.Authorization.Accounts.Dto;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.Editions;
using datntdev.MyCodebase.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.MultiTenancy;

[AbpAuthorize(PermissionNames.Pages_Tenants)]
public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, GetAllRequestDto, CreateRequestDto, TenantDto>, ITenantAppService
{
    private readonly TenantManager _tenantManager;
    private readonly EditionManager _editionManager;
    private readonly UserManager _userManager;
    private readonly RoleManager _roleManager;
    private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

    public TenantAppService(
        IRepository<Tenant, int> repository,
        TenantManager tenantManager,
        EditionManager editionManager,
        UserManager userManager,
        RoleManager roleManager,
        IAbpZeroDbMigrator abpZeroDbMigrator)
        : base(repository)
    {
        _tenantManager = tenantManager;
        _editionManager = editionManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _abpZeroDbMigrator = abpZeroDbMigrator;
    }

    [AbpAllowAnonymous]
    public async Task<IsAvailableResultDto> IsAvailableAsync(IsAvailableRequestDto input)
    {
        var tenant = await _tenantManager.FindByTenancyNameAsync(input.TenancyName);
        if (tenant == null)
        {
            return new IsAvailableResultDto(TenantAvailabilityState.NotFound);
        }

        if (!tenant.IsActive)
        {
            return new IsAvailableResultDto(TenantAvailabilityState.InActive);
        }

        return new IsAvailableResultDto(TenantAvailabilityState.Available, tenant.Id);
    }

    public override async Task<TenantDto> CreateAsync(CreateRequestDto input)
    {
        CheckCreatePermission();

        // Create tenant
        var tenant = ObjectMapper.Map<Tenant>(input);
        tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
            ? null
            : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

        var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
        if (defaultEdition != null)
        {
            tenant.EditionId = defaultEdition.Id;
        }

        await _tenantManager.CreateAsync(tenant);
        await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

        // Create tenant database
        _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

        // We are working entities of new tenant, so changing tenant filter
        using (CurrentUnitOfWork.SetTenantId(tenant.Id))
        {
            // Create static roles for new tenant
            CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

            await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

            // Grant all permissions to admin role
            var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
            await _roleManager.GrantAllPermissionsAsync(adminRole);

            // Create admin user for the tenant
            var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
            await _userManager.InitializeOptionsAsync(tenant.Id);
            CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
            await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

            // Assign admin user to role!
            CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        return MapToEntityDto(tenant);
    }

    protected override IQueryable<Tenant> CreateFilteredQuery(GetAllRequestDto input)
    {
        return Repository.GetAll()
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
    }

    protected override IQueryable<Tenant> ApplySorting(IQueryable<Tenant> query, GetAllRequestDto input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected override void MapToEntity(TenantDto updateInput, Tenant entity)
    {
        // Manually mapped since TenantDto contains non-editable properties too.
        entity.Name = updateInput.Name;
        entity.TenancyName = updateInput.TenancyName;
        entity.IsActive = updateInput.IsActive;
    }

    public override async Task DeleteAsync(EntityDto<int> input)
    {
        CheckDeletePermission();

        var tenant = await _tenantManager.GetByIdAsync(input.Id);
        await _tenantManager.DeleteAsync(tenant);
    }

    private void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}

