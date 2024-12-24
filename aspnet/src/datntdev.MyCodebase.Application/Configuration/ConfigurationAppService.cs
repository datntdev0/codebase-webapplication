using Abp.Authorization;
using Abp.Runtime.Session;
using datntdev.MyCodebase.Configuration.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : MyCodebaseAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
