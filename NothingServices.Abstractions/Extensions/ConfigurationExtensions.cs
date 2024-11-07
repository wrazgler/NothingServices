using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using NothingServices.Abstractions.Exceptions;

namespace NothingServices.Abstractions.Extensions;

/// <summary>
/// Методы расширений для конфигурации приложения
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary >
    /// Получить конфигурацию приложения типа <see href="TConfig"/>
    /// </summary>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Конфигурация приложения типа <see href="TConfig"/></returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    public static TConfig GetConfig<TConfig>(this IConfiguration configuration)
        where TConfig : class
    {
        var config = configuration.Get<TConfig>()
                     ?? throw new ConfigurationNullException<TConfig>();
        var configProperties = config.GetType().GetProperties();
        configProperties.ForEach(property => 
        {
            var value = property.GetValue(config);
            var required = property.GetCustomAttribute<RequiredMemberAttribute>() != null;
            if (required && value == null)
                throw new ConfigurationNullException<TConfig>();
        });
        return config;
    }
}