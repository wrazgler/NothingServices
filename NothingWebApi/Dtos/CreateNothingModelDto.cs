using System.Text.Json.Serialization;

namespace NothingWebApi.Dtos;

/// <summary>
/// Данные для создания модели
/// </summary>
public sealed class CreateNothingModelDto
{
    /// <summary>
    /// Имя модели
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}