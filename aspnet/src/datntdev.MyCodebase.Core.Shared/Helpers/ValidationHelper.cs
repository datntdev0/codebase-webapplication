using Abp.Extensions;
using System.Text.RegularExpressions;

namespace datntdev.MyCodebase.Helpers;

public static class ValidationHelper
{
    public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
    public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

    public static bool IsEmail(string value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        var regex = new Regex(EmailRegex);
        return regex.IsMatch(value);
    }
}
