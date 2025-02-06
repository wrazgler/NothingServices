using System.Text.Json.Serialization;

namespace NothingKafka.Dtos;

/// <summary>
/// Данные модели
/// </summary>
public sealed class NothingModelDto
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