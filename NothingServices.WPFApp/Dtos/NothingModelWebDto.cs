using System.Text.Json.Serialization;

namespace NothingServices.WPFApp.Dtos;

/// <summary>
/// Данные модели
/// </summary>
public class NothingModelWebDto
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}