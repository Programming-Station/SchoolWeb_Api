using System.Collections.Generic;

namespace School.Infrastructure.Email
{
    public interface ITemplateRenderer
    {
        string Render(string bodyTemplate, Dictionary<string, string>? placeholders);
    }

    public class EmailTemplateRenderer : ITemplateRenderer
    {
        private readonly PlaceholderResolver _placeholderResolver;

        public EmailTemplateRenderer(PlaceholderResolver placeholderResolver)
        {
            _placeholderResolver = placeholderResolver;
        }

        public string Render(string bodyTemplate, Dictionary<string, string>? placeholders)
        {
            return _placeholderResolver.Resolve(bodyTemplate, placeholders);
        }
    }
}
