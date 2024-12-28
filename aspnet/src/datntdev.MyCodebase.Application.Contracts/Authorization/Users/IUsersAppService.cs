using datntdev.MyCodebase.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

public interface IUsersAppService : IMyCodebaseCrudAppService<UserDto, long, GetAllRequestDto, Dto.CreateRequestDto, UserDto>
{
    Task ResetPasswordAsync(long id, ResetPasswordDto input);
    Task DeactivateAsync(long id);
    Task ActivateAsync(long id);
}
