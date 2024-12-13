namespace NothingServices.ConsoleApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingApi
/// </summary>
public interface INothingApiClientStrategy
{
    /// <summary>
    /// Вывести в консоль список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task GetNothingModels(CancellationToken cancellationToken = default);

    /// <summary>
    /// Вывести в консоль модель с указанным идентификатором
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task GetNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CreateNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task UpdateNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteNothingModel(CancellationToken cancellationToken = default);
}