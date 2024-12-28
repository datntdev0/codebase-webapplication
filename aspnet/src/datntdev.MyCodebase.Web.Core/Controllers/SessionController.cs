using Abp.Auditing;
using Abp.Localization;
using Abp.Runtime.Session;
using datntdev.MyCodebase.Session.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Controllers
{
    [Route("api/session")]
    public class SessionController : MyCodebaseControllerBase
    {
        [DisableAuditing]
        [HttpGet]
        public async Task<SessionDto> GetSessionAsync()
        {
            var output = new SessionDto
            {
                Application = new ApplicationInfoDto
                {
                    Version = MyCodebaseConsts.Version,
                    ReleaseDate = MyCodebaseConsts.ReleaseDate,
                    Features = [],
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }

        [HttpPatch("language")]
        public async Task ChangeLanguageAsync(ChangeLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        /// <summary>
        /// Change password of the current user.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPatch("password")]
        public async Task ChangePasswordAsync(ChangePasswordDto input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString())
                ?? throw new Exception("There is no current user!");

            if (await UserManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }
        }
    }
}
