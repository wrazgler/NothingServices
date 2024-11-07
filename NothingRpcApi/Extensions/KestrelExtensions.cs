using Microsoft.AspNetCore.Server.Kestrel.Core;
using NothingServices.Abstractions.Exceptions;

namespace NothingRpcApi.Extensions;

internal static class KestrelExtensions
{
    /// <summary>
    /// Добавить Kestrel в конструктор серверного приложения
    /// </summary>
    /// <param name="webBuilder">Конструктор серверного приложения</param>
    /// <returns>Конструктор серверного приложения с добавленным Kestrel</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    internal static IWebHostBuilder ConfigureKestrel(this IWebHostBuilder webBuilder)
    {
        webBuilder.UseKestrel(options =>
        {
            options.ConfigureEndpointDefaults(listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });
        return webBuilder;
    }
}