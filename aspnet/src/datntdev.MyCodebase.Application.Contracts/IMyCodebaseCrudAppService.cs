using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase;

public interface IMyCodebaseCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input);
    Task<TEntityDto> CreateAsync(TCreateInput input);
    Task<TEntityDto> UpdateAsync(TUpdateInput input);
    Task<TEntityDto> GetAsync(TPrimaryKey id);
    Task DeleteAsync(TPrimaryKey id);
}
