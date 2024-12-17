using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NothingRpcApi.Models;

/// <summary>
/// Модель
/// </summary>
[Table("nothing_models")]
public sealed class NothingModel
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }
}