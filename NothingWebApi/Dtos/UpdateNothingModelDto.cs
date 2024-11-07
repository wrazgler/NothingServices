using System.Text.Json.Serialization;

namespace NothingWebApi.Dtos;

/// <summary>
/// Данные для обновления модели
/// </summary>
public class UpdateNothingModelDto
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}