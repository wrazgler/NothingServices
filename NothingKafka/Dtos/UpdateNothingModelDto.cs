using System.Text.Json.Serialization;

namespace NothingKafka.Dtos;

/// <summary>
/// Данные для обновления модели
/// </summary>
public sealed class UpdateNothingModelDto
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