using System.Collections.Generic;

namespace datntdev.MyCodebase.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
