namespace KJ1012.Core.Extensions
{
    public static class StringExtension
    {
        public static string NullToEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }
    }
}
