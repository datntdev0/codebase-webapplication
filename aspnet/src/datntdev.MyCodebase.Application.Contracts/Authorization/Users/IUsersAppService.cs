using datntdev.MyCodebase.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Users;

public interface IUsersAppService : IMyCodebaseCrudAppService<UserDto, long, GetAllUsersDto, Dto.CreateUserDto, UserDto>
{
    Task ResetPasswordAsync(long id, ResetUserPasswordDto input);
    Task DeactivateAsync(long id);
    Task ActivateAsync(long id);
}
