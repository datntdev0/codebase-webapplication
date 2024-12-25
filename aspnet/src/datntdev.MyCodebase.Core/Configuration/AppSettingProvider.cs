using Abp.Configuration;
using System.Collections.Generic;

namespace datntdev.MyCodebase.Configuration;

public class AppSettingProvider : SettingProvider
{
    public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
    {
        return [];
    }
}
