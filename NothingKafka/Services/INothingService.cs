using NothingKafka.Dtos;

namespace NothingKafka.Services;

/// <summary>
/// Сервис бизнес логики
/// </summary>
public interface INothingService
{
    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="getNothingsModelsDto">Данные для получения моделей</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task Get(GetNothingModelsDto getNothingsModelsDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="getNothingModelDto">Данные для получения модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task Get(GetNothingModelDto getNothingModelDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelDto">Данные для создания модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    Task Create(CreateNothingModelDto createNothingModelDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelDto">Данные для обновления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    Task Update(UpdateNothingModelDto updateNothingModelDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="deleteNothingModelDto">Данные для удаления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task Delete(DeleteNothingModelDto deleteNothingModelDto, CancellationToken cancellationToken = default);
}