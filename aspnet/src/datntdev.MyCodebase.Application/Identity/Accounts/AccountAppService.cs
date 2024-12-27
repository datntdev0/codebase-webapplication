using Abp.Configuration;
using Abp.Zero.Configuration;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.Identity.Accounts.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Identity.Accounts;

public class AccountAppService(UserRegistrationManager userRegistrationManager) 
    : MyCodebaseAppServiceBase, IAccountAppService
{
    public async Task<RegisterResultDto> Register(RegisterRequestDto input)
    {
        var user = await userRegistrationManager.RegisterAsync(
            input.Name,
            input.Surname,
            input.EmailAddress,
            input.UserName,
            input.Password,
            true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
        );

        var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

        return new RegisterResultDto
        {
            CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
        };
    }
}
