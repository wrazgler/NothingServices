using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Services;

namespace NothingServices.ConsoleApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingRpcApi
/// </summary>
/// <param name="consoleService">Сервис работы с консолью</param>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="client">Клиент сервиса NothingRpcApi</param>
public class NothingRpcApiClientStrategy(
    IConsoleService consoleService,
    ILogger<NothingRpcApiClientStrategy> logger,
    NothingRpcService.NothingRpcServiceClient client)
    : NothingApiClientStrategy(consoleService), INothingApiClientStrategy
{
    private readonly ILogger<NothingRpcApiClientStrategy> _logger = logger;
    private readonly NothingRpcService.NothingRpcServiceClient _client = client;

    /// <summary>
    /// Вывести в консоль список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public override async Task GetNothingModels(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new Empty();
            var responseStream = _client.GetStream(request, cancellationToken: cancellationToken).ResponseStream;
            var nothingModels = new List<NothingModelDto>();
            while (await responseStream.MoveNext(cancellationToken))
            {
                nothingModels.Add(responseStream.Current);
            }
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
            var request = GetNothingModelIdDto(cancellationToken);
            var nothingModel = await _client.GetAsync(request, cancellationToken: cancellationToken);
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
            var nothingModel = await _client.CreateAsync(createNothingModel, cancellationToken: cancellationToken);
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
            var nothingModel = await _client.UpdateAsync(updateNothingModel, cancellationToken: cancellationToken);
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
            var request = GetNothingModelIdDto(cancellationToken);
            var nothingModel = await _client.DeleteAsync(request, cancellationToken: cancellationToken);
            ConsoleService.WriteLine(JsonSerializer.Serialize(nothingModel, JsonSerializerOptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    private NothingModelIdDto GetNothingModelIdDto(CancellationToken cancellationToken = default)
    {
        var id = GetId(cancellationToken);
        return new NothingModelIdDto()
        {
            Id = id
        };
    }

    private CreateNothingModelDto GetCreateNothingModelDto(CancellationToken cancellationToken = default)
    {
        var name = GetName(cancellationToken);
        return new CreateNothingModelDto()
        {
            Name = name
        };
    }

    private UpdateNothingModelDto GetUpdateNothingModelDto(CancellationToken cancellationToken = default)
    {
        var id = GetId(cancellationToken);
        var name = GetName(cancellationToken);
        return new UpdateNothingModelDto()
        {
            Id = id,
            Name = name
        };
    }
}