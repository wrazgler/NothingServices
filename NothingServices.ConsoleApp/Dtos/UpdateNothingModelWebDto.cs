using System.Text.Json.Serialization;

namespace NothingServices.ConsoleApp.Dtos;

/// <summary>
/// Данные для обновления модели
/// </summary>
public class UpdateNothingModelWebDto
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