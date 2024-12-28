using Abp.Application.Services.Dto;
using datntdev.MyCodebase.Authorization.Roles.Dto;
using datntdev.MyCodebase.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

public interface IUsersAppService : IMyCodebaseCrudAppService<UserDto, long, GetAllRequestDto, Dto.CreateRequestDto, UserDto>
{
    Task DeactivateAsync(EntityDto<long> user);
    Task ActivateAsync(EntityDto<long> user);
    Task PatchLanguageAsync(ChangeUserLanguageDto input);
    Task ResetPasswordAsync(ResetPasswordDto input);
    Task PatchPasswordAsync(ChangePasswordDto input);
    Task<ListResultDto<RoleDto>> GetRolesAsync();
}
