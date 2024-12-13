using System.Text.Json;
using NothingServices.ConsoleApp.Services;

namespace NothingServices.ConsoleApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingApi
/// </summary>
/// <param name="consoleService">Сервис работы с консолью</param>
public abstract class NothingApiClientStrategy(IConsoleService consoleService)
    : INothingApiClientStrategy
{
    /// <summary>
    /// Сервис работы с консолью
    /// </summary>
    protected readonly IConsoleService ConsoleService = consoleService;

    /// <summary>
    /// Настройки Json сериализации
    /// </summary>
    protected readonly JsonSerializerOptions JsonSerializerOptions = new () { WriteIndented = true };

    /// <summary>
    /// Вывести в консоль список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public abstract Task GetNothingModels(CancellationToken cancellationToken = default);

    /// <summary>
    /// Вывести в консоль модель с указанным идентификатором
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public abstract Task GetNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public abstract Task CreateNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public abstract Task UpdateNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public abstract Task DeleteNothingModel(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить идентификатор модели из консоли
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор модели</returns>
    protected int GetId(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleService.WriteLine("Введите идентификатор");
            var inputId = ConsoleService.ReadLine();
            if(inputId == "e")
                break;
            var convert = int.TryParse(inputId, out var id);
            if (!convert)
                continue;
            return id;
        }
        throw new TaskCanceledException(nameof(GetId));
    }

    /// <summary>
    /// Получить имя модели из консоли
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Имя модели</returns>
    protected string GetName(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleService.WriteLine("Введите имя модели");
            var name = ConsoleService.ReadLine();
            if(name == "e")
                break;
            if (string.IsNullOrEmpty(name))
                continue;
            return name;
        }
        throw new TaskCanceledException(nameof(GetName));
    }
}