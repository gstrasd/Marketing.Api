using System;
using Microsoft.Extensions.Configuration;

namespace Marketing.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T Bind<T>(this IConfiguration configuration) where T : class, new()
        {
            var settings = Activator.CreateInstance<T>();
            configuration.Bind(settings);
            return settings;
        }
    }
}
