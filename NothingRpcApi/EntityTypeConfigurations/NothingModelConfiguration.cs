using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NothingRpcApi.Models;

namespace NothingRpcApi.EntityTypeConfigurations;

/// <summary>
/// Конфигурация таблицы модели
/// </summary>
internal sealed class NothingModelConfiguration: IEntityTypeConfiguration<NothingModel>
{
    /// <summary>
    /// Настройка таблицы модели
    /// </summary>
    public void Configure(EntityTypeBuilder<NothingModel> entity)
    {
        entity.HasKey(nothingModel => nothingModel.Id);
        entity.HasIndex(nothingModel => nothingModel.Id)
            .IsUnique();
        entity.Property(nothingModel => nothingModel.Id)
            .UseIdentityByDefaultColumn();
        entity.Property(nothingModel => nothingModel.Name)
            .IsRequired();
    }
}