using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NothingRpcApi.Dtos;

namespace NothingRpcApi.Services;

/// <summary>
/// Сервис бизнес логики
/// </summary>
public interface INothingService
{
    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="request">Пустой запрос</param>
    /// <param name="responseStream">Стрим, возвращающий результаты метода</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Список моделей</returns>
    public Task GetStream(
        Empty request,
        IServerStreamWriter<NothingModelDto> responseStream,
        ServerCallContext context);

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="nothingModelIdDto">Идентификатор модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    public Task<NothingModelDto> Get(
        NothingModelIdDto nothingModelIdDto,
        ServerCallContext context);

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelDto">Данные для создания модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public Task<NothingModelDto> Create(
        CreateNothingModelDto createNothingModelDto,
        ServerCallContext context);

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelDto">Данные для обновления модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public Task<NothingModelDto> Update(
        UpdateNothingModelDto updateNothingModelDto,
        ServerCallContext context);

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="nothingModelIdDto">Идентификатор модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    public Task<NothingModelDto> Delete(
        NothingModelIdDto nothingModelIdDto,
        ServerCallContext context);
}