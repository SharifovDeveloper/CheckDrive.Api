namespace CheckDrive.Api.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToUpper(this string str)
        {
            return str[..1].ToUpper() + str[1..].ToLower();
        }
    }
}
