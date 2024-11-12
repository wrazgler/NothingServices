using System.Collections.ObjectModel;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Strategies;

/// <summary>
/// Стратегия взаимодействия с клиентом NothingApi
/// </summary>
public interface INothingApiClientStrategy
{
    /// <summary>
    /// Вывести в консоль список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция данных представления модели</returns>
    Task<ObservableCollection<NothingModelVM>> GetNothingModelsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Вывести в консоль модель с указанным идентификатором
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    Task<NothingModelVM> GetNothingModelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelVM">Данные представления создать модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    Task<NothingModelVM> CreateNothingModelAsync(
        CreateNothingModelVM createNothingModelVM,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelVM">Данные представления обновить модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    Task<NothingModelVM> UpdateNothingModelAsync(
        UpdateNothingModelVM updateNothingModelVM,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="nothingModelVM">Данные представления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные представления модели</returns>
    Task<NothingModelVM> DeleteNothingModelAsync(
        NothingModelVM nothingModelVM,
        CancellationToken cancellationToken = default);
}