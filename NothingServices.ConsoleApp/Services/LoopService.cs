using Microsoft.Extensions.Hosting;
using NothingServices.ConsoleApp.Strategies;

namespace NothingServices.ConsoleApp.Services;

/// <summary>
/// Сервис бизнес логики приложения
/// </summary>
/// <param name="consoleService">Сервис работы с консолью</param>
/// <param name="lifetime">Информация о жизненном цикле приложения</param>
/// <param name="nothingRpcApiClientStrategy">Стратегия взаимодействия с клиентом NothingRpcApi</param>
/// <param name="nothingWebApiClientStrategy">Стратегия взаимодействия с клиентом NothingWebApi</param>
public class LoopService(
    IConsoleService consoleService,
    IHostApplicationLifetime lifetime,
    NothingRpcApiClientStrategy nothingRpcApiClientStrategy,
    NothingWebApiClientStrategy nothingWebApiClientStrategy)
    : ILoopService
{
    private readonly IConsoleService _consoleService = consoleService;
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly NothingRpcApiClientStrategy _nothingRpcApiClientStrategy = nothingRpcApiClientStrategy;
    private readonly NothingWebApiClientStrategy _nothingWebApiClientStrategy = nothingWebApiClientStrategy;

    /// <summary>
    /// Выполнение работы приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task DoWork(CancellationToken cancellationToken = default)
    {
        var work = true;
        while (work && !cancellationToken.IsCancellationRequested)
        {
            _consoleService.WriteLine("Выберите клиент");
            _consoleService.WriteLine("1: NothingRpcApi");
            _consoleService.WriteLine("2: NothingWebApi");
            _consoleService.WriteLine("e: Завершить работу");
            var inputKey = _consoleService.ReadLine();
            _consoleService.WriteLine();
            var task = inputKey switch
            {
                "1" => CommunicateNothingApi(_nothingRpcApiClientStrategy, cancellationToken),
                "2" => CommunicateNothingApi(_nothingWebApiClientStrategy, cancellationToken),
                "e" => Task.Run(() => work = false, CancellationToken.None),
                _ => Task.Run(() => PrintReadError(inputKey), CancellationToken.None),
            };
            await task;
        }
        _consoleService.WriteLine("Работа приложения завершена");
        _lifetime.StopApplication();
    }

    private async Task CommunicateNothingApi(
        INothingApiClientStrategy nothingWebApiClientStrategy,
        CancellationToken cancellationToken)
    {
        var communicate = true;
        while (communicate && !cancellationToken.IsCancellationRequested)
        {
            _consoleService.WriteLine("Выберите действие");
            _consoleService.WriteLine("1: Получить список моделей");
            _consoleService.WriteLine("2: Получить модель с указанным идентификатором");
            _consoleService.WriteLine("3: Создать новую модель");
            _consoleService.WriteLine("4: Обновить существующую модель");
            _consoleService.WriteLine("5: Удалить модель с указанным идентификатором");
            _consoleService.WriteLine("e: Вернуться назад");
            var inputKey = _consoleService.ReadLine();
            _consoleService.WriteLine();
            var task = inputKey switch
            {
                "1" => nothingWebApiClientStrategy.GetNothingModels(cancellationToken),
                "2" => nothingWebApiClientStrategy.GetNothingModel(cancellationToken),
                "3" => nothingWebApiClientStrategy.CreateNothingModel(cancellationToken),
                "4" => nothingWebApiClientStrategy.UpdateNothingModel(cancellationToken),
                "5" => nothingWebApiClientStrategy.DeleteNothingModel(cancellationToken),
                "e" => Task.Run(() => communicate = false, CancellationToken.None),
                _ => Task.Run(() => PrintReadError(inputKey), CancellationToken.None),
            };
            await task;
        }
    }

    private void PrintReadError(string? inputKey)
    {
        _consoleService.WriteLine($"Отсутствует соответствие выбранной команде: {inputKey}");
    }
}