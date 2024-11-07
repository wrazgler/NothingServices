using System.Text.Json;
using NothingServices.ConsoleApp.Dtos;

namespace NothingServices.ConsoleApp.Clients;

/// <summary>
/// Клиент сервиса NothingWebApi
/// </summary>
public interface INothingWebApiClient
{
    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    /// <exception cref="JsonException">
    /// Ошибка десериализации ответа внешнего сервиса
    /// </exception>
    Task<NothingModelWebDto[]> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="JsonException">
    /// Ошибка десериализации ответа внешнего сервиса
    /// </exception>
    Task<NothingModelWebDto> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelWebDto">Данные для создания модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="JsonException">
    /// Ошибка десериализации ответа внешнего сервиса
    /// </exception>
    Task<NothingModelWebDto> CreateAsync(
        CreateNothingModelWebDto createNothingModelWebDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelWebDto">Данные для обновления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="JsonException">
    /// Ошибка десериализации ответа внешнего сервиса
    /// </exception>
    Task<NothingModelWebDto> UpdateAsync(
        UpdateNothingModelWebDto updateNothingModelWebDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="JsonException">
    /// Ошибка десериализации ответа внешнего сервиса
    /// </exception>
    Task<NothingModelWebDto> DeleteAsync(int id, CancellationToken cancellationToken = default);
}