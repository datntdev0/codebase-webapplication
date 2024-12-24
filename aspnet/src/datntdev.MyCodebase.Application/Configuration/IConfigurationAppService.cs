using datntdev.MyCodebase.Configuration.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
