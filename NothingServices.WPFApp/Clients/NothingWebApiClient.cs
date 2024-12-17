using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NothingServices.WPFApp.Configs;
using NothingServices.WPFApp.Dtos;

namespace NothingServices.WPFApp.Clients;

/// <summary>
/// Клиент сервиса NothingWebApi
/// </summary>
/// <param name="httpClientFactory">Фабрика Http клиента</param>
/// <param name="clientOptions">Конфигурация подключения к внешнему сервису NothingWebApi</param>
public sealed class NothingWebApiClient(
    IHttpClientFactory httpClientFactory,
    IOptions<NothingWebApiClientConfig> clientOptions)
    : INothingWebApiClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(nameof(NothingWebApiClient));
    private readonly NothingWebApiClientConfig _clientConfig = clientOptions.Value;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    /// <exception cref="HttpRequestException">Ошибка получения ответа внешнего сервиса</exception>
    /// <exception cref="JsonException">Ошибка десериализации ответа внешнего сервиса</exception>
    public async Task<NothingModelWebDto[]> Get(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(_clientConfig.NothingWebApiUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(message);
        }
        var nothingModels = await response.Content.ReadFromJsonAsync<NothingModelWebDto[]>(cancellationToken)
            ?? throw new JsonException("Не удалось десериализовать ответ внешнего сервиса");
        return nothingModels;
    }

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="HttpRequestException">Ошибка получения ответа внешнего сервиса</exception>
    /// <exception cref="JsonException">Ошибка десериализации ответа внешнего сервиса</exception>
    public async Task<NothingModelWebDto> Get(int id, CancellationToken cancellationToken = default)
    {
        var requestUrl = Path.Combine(_clientConfig.NothingWebApiUrl, id.ToString());
        using var response = await _httpClient.GetAsync(requestUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(message);
        }
        var nothingModel = await response.Content.ReadFromJsonAsync<NothingModelWebDto>(cancellationToken)
            ?? throw new JsonException("Не удалось десериализовать ответ внешнего сервиса");
        return nothingModel;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelWebDto">Данные для создания модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="HttpRequestException">Ошибка получения ответа внешнего сервиса</exception>
    /// <exception cref="JsonException">Ошибка десериализации ответа внешнего сервиса</exception>
    public async Task<NothingModelWebDto> Create(
        CreateNothingModelWebDto createNothingModelWebDto,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(createNothingModelWebDto);
        using var response = await _httpClient.PostAsync(_clientConfig.NothingWebApiUrl, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(message);
        }
        var nothingModel = await response.Content.ReadFromJsonAsync<NothingModelWebDto>(cancellationToken)
            ?? throw new JsonException("Не удалось десериализовать ответ внешнего сервиса");
        return nothingModel;
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelWebDto">Данные для обновления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="HttpRequestException">Ошибка получения ответа внешнего сервиса</exception>
    /// <exception cref="JsonException">Ошибка десериализации ответа внешнего сервиса</exception>
    public async Task<NothingModelWebDto> Update(
        UpdateNothingModelWebDto updateNothingModelWebDto,
        CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(updateNothingModelWebDto);
        using var response = await _httpClient.PutAsync(_clientConfig.NothingWebApiUrl, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(message);
        }
        var nothingModel = await response.Content.ReadFromJsonAsync<NothingModelWebDto>(cancellationToken)
            ?? throw new JsonException("Не удалось десериализовать ответ внешнего сервиса");
        return nothingModel;
    }

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="HttpRequestException">Ошибка получения ответа внешнего сервиса</exception>
    /// <exception cref="JsonException">Ошибка десериализации ответа внешнего сервиса</exception>
    public async Task<NothingModelWebDto> Delete(int id, CancellationToken cancellationToken = default)
    {
        var requestUrl = Path.Combine(_clientConfig.NothingWebApiUrl, id.ToString());
        using var response = await _httpClient.DeleteAsync(requestUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(message);
        }
        var nothingModel = await response.Content.ReadFromJsonAsync<NothingModelWebDto>(cancellationToken)
            ?? throw new JsonException("Не удалось десериализовать ответ внешнего сервиса");
        return nothingModel;
    }
}