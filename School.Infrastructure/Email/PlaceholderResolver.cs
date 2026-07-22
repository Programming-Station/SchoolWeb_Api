using System.Text.RegularExpressions;

namespace School.Infrastructure.Email
{
    public class PlaceholderResolver
    {
        public string Resolve(string template, Dictionary<string, string>? placeholders)
        {
            if (string.IsNullOrEmpty(template)) return template;

            var result = template;

            // Merge user placeholders with auto-generated system placeholders
            var allPlaceholders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Standard system variables
            allPlaceholders["CurrentDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            allPlaceholders["CurrentTime"] = DateTime.Now.ToString("HH:mm:ss");
            allPlaceholders["Year"] = DateTime.Now.ToString("yyyy");
            allPlaceholders["Month"] = DateTime.Now.ToString("MMMM");

            if (placeholders != null)
            {
                foreach (var kvp in placeholders)
                {
                    allPlaceholders[kvp.Key] = kvp.Value ?? string.Empty;
                }
            }

            foreach (var kvp in allPlaceholders)
            {
                string doubleCurly = "{{" + kvp.Key + "}}";
                string singleCurly = "{" + kvp.Key + "}";

                // Replace double curly braces case-insensitively
                result = Regex.Replace(
                    result,
                    Regex.Escape(doubleCurly),
                    kvp.Value ?? string.Empty,
                    RegexOptions.IgnoreCase
                );

                // Replace single curly braces case-insensitively
                result = Regex.Replace(
                    result,
                    Regex.Escape(singleCurly),
                    kvp.Value ?? string.Empty,
                    RegexOptions.IgnoreCase
                );
            }

            return result;
        }
    }
}
