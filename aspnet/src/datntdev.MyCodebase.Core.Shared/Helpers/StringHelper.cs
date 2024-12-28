using System.Text.RegularExpressions;

namespace datntdev.MyCodebase.Helpers
{
    public static partial class StringHelper
    {
        [GeneratedRegex("([a-z])([A-Z])")]
        private static partial Regex KebabConversionRegex();

        public static string ToKebabCase(this string input)
        {
            return KebabConversionRegex().Replace(input, "$1-$2").ToLower();
        }
    }
}
