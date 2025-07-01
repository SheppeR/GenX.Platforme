using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GenX.Common.Extensions;

public static class MD5Extentions
{
    public static bool IsMD5(this string? input)
    {
        if (string.IsNullOrEmpty(input) || input.Length != 32)
        {
            return false;
        }

        return Regex.IsMatch(input, "^[0-9a-fA-F]{32}$");
    }

    public static string? CreateMD5(this string? input)
    {
        using var md5 = MD5.Create();
        if (input != null)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToUpper();
        }

        return null;
    }
}