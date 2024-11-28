namespace NothingServices.ConsoleApp.Services;

/// <summary>
/// Сервис бизнес логики приложения
/// </summary>
public interface ILoopService
{
    /// <summary>
    /// Выполнение работы приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DoWork(CancellationToken cancellationToken = default);
}