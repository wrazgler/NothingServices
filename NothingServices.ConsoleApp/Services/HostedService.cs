using Microsoft.Extensions.Hosting;

namespace NothingServices.ConsoleApp.Services;

/// <summary>
/// Сервис бизнес логики хоста
/// </summary>
/// <param name="lifetime">Информация о жизненном цикле приложения</param>
/// <param name="loopService">Сервис бизнес логики приложения</param>
public class HostedService(
    IHostApplicationLifetime lifetime,
    ILoopService loopService)
    : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly ILoopService _loopService = loopService;

    /// <summary>
    /// Запуск приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _loopService.DoWorkAsync(cancellationToken);
    }

    /// <summary>
    /// Остановка приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _lifetime.StopApplication();
        return Task.CompletedTask;
    }
}