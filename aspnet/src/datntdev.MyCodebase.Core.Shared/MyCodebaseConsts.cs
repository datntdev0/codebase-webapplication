using System;

namespace datntdev.MyCodebase;

public class MyCodebaseConsts
{
    /// <summary> 
    /// Gets the name of the localization source. 
    /// It's used to define the source of localizable strings.
    /// </summary>
    public const string LocalizationSourceName = "MyCodebase";

    /// <summary> 
    /// Gets the name of the default connection string.
    /// It's used to connect to the default database.
    /// </summary>
    public const string ConnectionStringName = "Default";

    /// <summary>
    /// Indicates if multi-tenancy is enabled
    /// True if the application supports multiple tenants.
    /// </summary>
    public const bool MultiTenancyEnabled = true;

    /// <summary>
    /// Gets the startup time of the application.
    /// </summary>
    public readonly static DateTime StartupTime = new();

    /// <summary>
    /// Gets current version of the application.
    /// It's also shown in the web page.
    /// </summary>
    public const string Version = "9.3.0";

    /// <summary>
    /// Gets release (last build) date of the application.
    /// It's shown in the web page.
    /// </summary>
    public readonly static DateTime ReleaseDate = new(2025, 01, 01);
}
