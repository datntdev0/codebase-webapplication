using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace datntdev.MyCodebase.Editions;

public class EditionManager(
    IRepository<Edition> editionRepository,
    IAbpZeroFeatureValueStore featureValueStore,
    IUnitOfWorkManager unitOfWorkManager
) : AbpEditionManager(editionRepository, featureValueStore, unitOfWorkManager)
{
    public const string DefaultEditionName = "Standard";
}
