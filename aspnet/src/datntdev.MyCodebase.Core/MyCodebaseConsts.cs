using datntdev.MyCodebase.Debugging;

namespace datntdev.MyCodebase;

public class MyCodebaseConsts
{
    public const string LocalizationSourceName = "MyCodebase";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "2432bc2b15ce481f8840636aca01a2e5";
}
