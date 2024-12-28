using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace datntdev.MyCodebase;

public class MyCodebaseCrudAppServicee<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
    IMyCodebaseCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
   
    public MyCodebaseCrudAppServicee(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
    {
        AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
    }

    public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
    {
        CheckGetAllPermission();

        var query = CreateFilteredQuery(input);

        var totalCount = await AsyncQueryableExecuter.CountAsync(query);

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var entities = await AsyncQueryableExecuter.ToListAsync(query);

        return new PagedResultDto<TEntityDto>(
            totalCount,
            entities.Select(MapToEntityDto).ToList()
        );
    }

    public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
    {
        CheckCreatePermission();

        var entity = MapToEntity(input);

        await Repository.InsertAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
    {
        CheckUpdatePermission();

        var entity = await GetEntityByIdAsync(input.Id);

        MapToEntity(input, entity);
        await CurrentUnitOfWork.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> GetAsync(TPrimaryKey id)
    {
        CheckGetPermission();

        var entity = await GetEntityByIdAsync(id);
        return MapToEntityDto(entity);
    }

    public virtual Task DeleteAsync(TPrimaryKey id)
    {
        CheckDeletePermission();

        return Repository.DeleteAsync(id);
    }

    protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
    {
        return Repository.GetAsync(id);
    }
}
