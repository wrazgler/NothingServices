using System.Text.Json;
using Microsoft.Extensions.Logging;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Services;

namespace NothingServices.ConsoleApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingWebApi
/// </summary>
/// <param name="consoleService">Сервис работы с консолью</param>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="client">Клиент сервиса NothingWebApi</param>
public class NothingWebApiClientStrategy(
    IConsoleService consoleService,
    ILogger<NothingWebApiClientStrategy> logger,
    INothingWebApiClient client)
    : NothingApiClientStrategy(consoleService), INothingApiClientStrategy
{
    private readonly ILogger<NothingWebApiClientStrategy> _logger = logger;
    private readonly INothingWebApiClient _client = client;

    /// <summary>
    /// Вывести в консоль список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task GetNothingModels(CancellationToken cancellationToken = default)
    {
        try
        {
            var nothingModels = await _client.Get(cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModels, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Вывести в консоль модель с указанным идентификатором
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task GetNothingModel(CancellationToken cancellationToken = default)
    {
        try
        {
            var id = GetId(cancellationToken);
            var nothingModel = await _client.Get(id, cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModel, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task CreateNothingModel(CancellationToken cancellationToken = default)
    {
        try
        {
            var createNothingModel = GetCreateNothingModelDto(cancellationToken);
            var nothingModel = await _client
                .Create(createNothingModel, cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModel, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task UpdateNothingModel(CancellationToken cancellationToken = default)
    {
        try
        {
            var updateNothingModel = GetUpdateNothingModelDto(cancellationToken);
            var nothingModel = await _client
                .Update(updateNothingModel, cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModel, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task DeleteNothingModel(CancellationToken cancellationToken = default)
    {
        try
        {
            var id = GetId(cancellationToken);
            var nothingModel = await _client.Delete(id, cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModel, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    private CreateNothingModelWebDto GetCreateNothingModelDto(CancellationToken cancellationToken = default)
    {
        var name = GetName(cancellationToken);
        return new CreateNothingModelWebDto()
        {
            Name = name
        };
    }

    private UpdateNothingModelWebDto GetUpdateNothingModelDto(CancellationToken cancellationToken = default)
    {
        var id = GetId(cancellationToken);
        var name = GetName(cancellationToken);
        return new UpdateNothingModelWebDto()
        {
            Id = id,
            Name = name
        };
    }
}