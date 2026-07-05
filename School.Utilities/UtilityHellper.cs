using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using System.Reflection;
using System.Text;

namespace School.Utilities
{
    public static class UtilityHellper
    {
        private static IdentityOptions _identityOptions = new IdentityOptions();
        private static readonly Random _random = new Random();
        public static string GetTransKey()
        {
            string transKey = $"{DateTime.Now:ddMMyyyyHHmmssffffff}{new Random().Next(123, 9999)}{new Random().Next(1, 122)}";
            return transKey;
        }
        public static string GeneratePassword()
        {

            var passwordOptions = _identityOptions.Password;

            const string digits = "0123456789";
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string symbols = "!@#$%^&*()-=+[]{}|;<>?";

            var allChars = "";
            var password = new StringBuilder();

            if (passwordOptions.RequireDigit)
            {
                password.Append(digits[_random.Next(digits.Length)]);
                allChars += digits;
            }

            if (passwordOptions.RequireLowercase)
            {
                password.Append(lowers[_random.Next(lowers.Length)]);
                allChars += lowers;
            }

            if (passwordOptions.RequireUppercase)
            {
                password.Append(uppers[_random.Next(uppers.Length)]);
                allChars += uppers;
            }

            if (passwordOptions.RequireNonAlphanumeric)
            {
                password.Append(symbols[_random.Next(symbols.Length)]);
                allChars += symbols;
            }

            if (string.IsNullOrEmpty(allChars))
                allChars = digits + lowers + uppers + symbols;

            while (password.Length < passwordOptions.RequiredLength + 2)
            {
                password.Append(allChars[_random.Next(allChars.Length)]);
            }

            return new string(password.ToString()
                .OrderBy(_ => _random.Next())
                .ToArray());
        }

        public static string FullName(this string firstName, string lastName)
        {
            return string.Join(" ", new[] { firstName, lastName }
                                             .Where(x => !string.IsNullOrWhiteSpace(x)));
        }
        public static string RemoveSpace(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input.Trim(), @"\s+", " ");
        }

    }

}


