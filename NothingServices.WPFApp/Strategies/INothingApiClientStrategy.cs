using System.Collections.ObjectModel;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingApi
/// </summary>
public interface INothingApiClientStrategy
{
    /// <summary>
    /// Вывести список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция данных представления модели</returns>
    Task<ObservableCollection<INothingModelVM>> GetNothingModels(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelVM">Данные представления создать модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<INothingModelVM> CreateNothingModel(
        CreateNothingModelVM createNothingModelVM,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelVM">Данные представления обновить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<INothingModelVM> UpdateNothingModel(
        UpdateNothingModelVM updateNothingModelVM,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="deleteNothingModelVM">Данные представления удалить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<INothingModelVM> DeleteNothingModel(
        DeleteNothingModelVM deleteNothingModelVM,
        CancellationToken cancellationToken = default);
}