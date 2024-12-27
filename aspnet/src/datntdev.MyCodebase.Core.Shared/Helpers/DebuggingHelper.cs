namespace datntdev.MyCodebase.Helpers;

public static class DebuggingHelper
{
    public static bool IsDebug
    {
#pragma warning disable CS0162 // Unreachable code detected
        get
        {
#if DEBUG
            return true;
#endif
            return false;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
