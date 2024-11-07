using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NothingRpcApi.Models;

namespace NothingRpcApi.EntityTypeConfigurations;

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
        entity.Property(nothingModel => nothingModel.Id)
            .UseIdentityByDefaultColumn();
        entity.Property(nothingModel => nothingModel.Name)
            .IsRequired();
    }
}