using System.ComponentModel.DataAnnotations.Schema;

namespace NothingRpcApi.Models;

/// <summary>
/// Модель
/// </summary>
[Table("nothing_models")]
public class NothingModel
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    public required string Name { get; set; }
}