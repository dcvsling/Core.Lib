namespace Core.Lib.Ast.Abstractions
{
    using System.Text.RegularExpressions;
    using Models;
    public static class LocationHelper
    {
        public static Location CreateLocation(this string source, int position)
        {
            var lineRegex = new Regex("\r\n|[\n\r]", RegexOptions.ECMAScript);
            var location = new Location
            {
                Line = 1,
                Column = position + 1
            };

            var matches = lineRegex.Matches(source);
            foreach (Match match in matches)
            {
                if (match.Index >= position)
                    break;

                location.Line++;
                location.Column = position + 1 - (match.Index + matches[0].Length);
            }
            return location;
        }
    }

}