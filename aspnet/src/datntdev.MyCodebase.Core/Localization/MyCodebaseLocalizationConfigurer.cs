using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace datntdev.MyCodebase.Localization;

public static class MyCodebaseLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(MyCodebaseConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(MyCodebaseLocalizationConfigurer).GetAssembly(),
                    "datntdev.MyCodebase.Localization.SourceFiles"
                )
            )
        );
    }
}
