using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;

namespace datntdev.MyCodebase.Features;

public class FeatureValueStore(
    ICacheManager cacheManager,
    IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
    IRepository<Tenant> tenantRepository,
    IRepository<EditionFeatureSetting, long> editionFeatureRepository,
    IFeatureManager featureManager,
    IUnitOfWorkManager unitOfWorkManager
) : AbpFeatureValueStore<Tenant, User>(
    cacheManager,
    tenantFeatureRepository,
    tenantRepository,
    editionFeatureRepository,
    featureManager,
    unitOfWorkManager)
{ }
