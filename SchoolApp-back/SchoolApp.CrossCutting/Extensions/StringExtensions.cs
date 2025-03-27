using System.Text.RegularExpressions;

namespace SchoolApp.CrossCutting.Extensions;

public static class StringExtensions
{
    public static string RemoveSpacesAndSpecialCharacters(this string? input)
    {
        string pattern = @"[^A-Za-z0-9-_'\(\)\[\]!$%.,]";
        return Regex.Replace(input, pattern, "");
    }
}