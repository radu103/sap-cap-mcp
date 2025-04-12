
namespace SAPCAPTools.StringHelpers
{
    public static class CDSStringHelpers
    {
        public static string ToUpperFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string ToLowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}