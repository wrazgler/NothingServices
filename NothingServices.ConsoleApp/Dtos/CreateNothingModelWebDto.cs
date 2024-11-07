using System.Text.Json.Serialization;

namespace NothingServices.ConsoleApp.Dtos;

/// <summary>
/// Данные для создания модели
/// </summary>
public class CreateNothingModelWebDto
{
    /// <summary>
    /// Имя модели
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}