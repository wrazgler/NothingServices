using System.Text.Json.Serialization;

namespace NothingKafka.Dtos;

/// <summary>
/// Данные для удаления модели
/// </summary>
public class DeleteNothingModelDto
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }
}