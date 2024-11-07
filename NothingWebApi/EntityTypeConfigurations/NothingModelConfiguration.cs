using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NothingWebApi.Models;

namespace NothingWebApi.EntityTypeConfigurations;

/// <summary>
/// Конфигурация таблицы модели
/// </summary>
public class NothingModelConfiguration: IEntityTypeConfiguration<NothingModel>
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