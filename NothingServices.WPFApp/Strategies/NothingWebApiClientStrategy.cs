using System.Collections.ObjectModel;
using AutoMapper;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingWebApi
/// </summary>
/// <param name="client">Клиент сервиса NothingWebApi</param>
/// <param name="factory">Фабрика создания объекта данных представления модели</param>
/// <param name="mapper"><see cref="Mapper"/></param>
public class NothingWebApiClientStrategy(
    IMapper mapper,
    INothingModelVMFactory factory,
    INothingWebApiClient client)
    : INothingApiClientStrategy
{
    private readonly IMapper _mapper = mapper;
    private readonly INothingModelVMFactory _factory = factory;
    private readonly INothingWebApiClient _client = client;

    /// <summary>
    /// Вывести список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция данных представления модели</returns>
    public async Task<ObservableCollection<NothingModelVM>> GetNothingModelsAsync(CancellationToken cancellationToken = default)
    {
        var nothingModels = await _client.GetAsync(cancellationToken);
        var nothingModelVMs = nothingModels
            .Select(_factory.Create)
            .ToObservableCollection();
        return nothingModelVMs;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelVM">Данные представления создать модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task CreateNothingModelAsync(
        CreateNothingModelVM createNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        var createNothingModel = _mapper.Map<CreateNothingModelWebDto>(createNothingModelVM);
        return _client.CreateAsync(createNothingModel, cancellationToken);
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelVM">Данные представления обновить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task UpdateNothingModelAsync(
        UpdateNothingModelVM updateNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        var updateNothingModel = _mapper.Map<UpdateNothingModelWebDto>(updateNothingModelVM);
        return _client.UpdateAsync(updateNothingModel, cancellationToken);
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="deleteNothingModelVM">Данные представления удалить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task DeleteNothingModelAsync(
        DeleteNothingModelVM deleteNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        return _client.DeleteAsync(deleteNothingModelVM.Id, cancellationToken);
    }
}