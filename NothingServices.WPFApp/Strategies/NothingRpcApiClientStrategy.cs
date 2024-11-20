using System.Collections.ObjectModel;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingRpcApi
/// </summary>
/// <param name="client">Клиент сервиса NothingRpcApi</param>
/// <param name="factory">Фабрика создания объекта данных представления модели</param>
/// <param name="mapper"><see cref="Mapper"/></param>
public class NothingRpcApiClientStrategy(
    IMapper mapper,
    INothingModelVMFactory factory,
    NothingRpcService.NothingRpcServiceClient client)
    : INothingApiClientStrategy
{
    private readonly IMapper _mapper = mapper;
    private readonly INothingModelVMFactory _factory = factory;
    private readonly NothingRpcService.NothingRpcServiceClient _client = client;

    /// <summary>
    /// Вывести список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция данных представления модели</returns>
    public async Task<ObservableCollection<NothingModelVM>> GetNothingModelsAsync(CancellationToken cancellationToken = default)
    {
        var request = new Empty();
        var responseStream = _client.GetStream(request, cancellationToken: cancellationToken).ResponseStream;
        var nothingModelVMs = new ObservableCollection<NothingModelVM>();
        while (await responseStream.MoveNext(cancellationToken))
        {
            var nothingModelVM = _factory.Create(responseStream.Current);
            nothingModelVMs.Add(nothingModelVM);
        }
        return nothingModelVMs;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelVM">Данные представления создать модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task<NothingModelVM> CreateNothingModelAsync(
        CreateNothingModelVM createNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        var createNothingModel = _mapper.Map<CreateNothingModelDto>(createNothingModelVM);
        var nothingModel = await _client.CreateAsync(createNothingModel, cancellationToken: cancellationToken);
        var nothingModelVM = _factory.Create(nothingModel);
        return nothingModelVM;
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelVM">Данные представления обновить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task<NothingModelVM> UpdateNothingModelAsync(
        UpdateNothingModelVM updateNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        var updateNothingModel = _mapper.Map<UpdateNothingModelDto>(updateNothingModelVM);
        var nothingModel = await _client.UpdateAsync(updateNothingModel, cancellationToken: cancellationToken);
        var nothingModelVM = _factory.Create(nothingModel);
        return nothingModelVM;
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="deleteNothingModelVM">Данные представления удалить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task<NothingModelVM> DeleteNothingModelAsync(
        DeleteNothingModelVM deleteNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        var request = _mapper.Map<NothingModelIdDto>(deleteNothingModelVM);
        var nothingModel = await _client.DeleteAsync(request, cancellationToken: cancellationToken);
        var nothingModelVM = _factory.Create(nothingModel);
        return nothingModelVM;
    }
}