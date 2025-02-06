using System.Text.Json.Serialization;

namespace NothingKafka.Dtos;

/// <summary>
/// Данные для получения модели
/// </summary>
public sealed class GetNothingModelDto
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }
}