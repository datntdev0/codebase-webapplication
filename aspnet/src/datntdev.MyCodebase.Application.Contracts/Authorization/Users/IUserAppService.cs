using Abp.Application.Services;
using Abp.Application.Services.Dto;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using datntdev.MyCodebase.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

public interface IUserAppService : IAsyncCrudAppService<UserDto, long, GetAllRequestDto, Dto.CreateRequestDto, UserDto>
{
    Task DeActivate(EntityDto<long> user);
    Task Activate(EntityDto<long> user);
    Task<ListResultDto<RoleDto>> GetRoles();
    Task ChangeLanguage(ChangeUserLanguageDto input);
    Task<bool> ChangePassword(ChangePasswordDto input);
}
