using System.IO;
using System.Text.RegularExpressions;

namespace Devon4Net.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static string GetDirectoryFromString(this string fileName)
        {
            var currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            return appPathMatcher.Match(currentPath).Value;
        }
    }
}
