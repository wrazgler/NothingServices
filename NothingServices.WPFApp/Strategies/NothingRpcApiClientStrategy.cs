using System.Collections.ObjectModel;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingRpcApi
/// </summary>
/// <param name="client">Клиент сервиса NothingRpcApi</param>
public class NothingRpcApiClientStrategy(NothingRpcService.NothingRpcServiceClient client)
    : INothingApiClientStrategy
{
    private readonly NothingRpcService.NothingRpcServiceClient _client = client;

    /// <summary>
    /// Вывести список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция данных представления модели</returns>
    public async Task<ObservableCollection<NothingModelVM>> GetNothingModelsAsync(CancellationToken cancellationToken = default)
    {
        return null;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelVM">Данные представления создать модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    public async Task<NothingModelVM> CreateNothingModelAsync(
        CreateNothingModelVM createNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelVM">Данные представления обновить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    public async Task<NothingModelVM> UpdateNothingModelAsync(
        UpdateNothingModelVM updateNothingModelVM,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="nothingModelVM">Данные представления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    public async Task<NothingModelVM> DeleteNothingModelAsync(
        NothingModelVM nothingModelVM,
        CancellationToken cancellationToken = default)
    {
        return null;
    }
}