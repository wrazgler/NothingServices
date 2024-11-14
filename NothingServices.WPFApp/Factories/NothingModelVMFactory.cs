using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления модели
/// </summary>
/// <param name="openDeleteNothingModelCommand">
/// Команда открыть представление окна удалить существующую модель
/// </param>
/// <param name="openUpdateNothingModelCommand">
/// Команда открыть представление окна обновления существующей модели
/// </param>
public class NothingModelVMFactory(
    OpenDeleteNothingModelCommand openDeleteNothingModelCommand,
    OpenUpdateNothingModelCommand openUpdateNothingModelCommand)
    : INothingModelVMFactory
{
    private readonly OpenDeleteNothingModelCommand _openDeleteNothingModelCommand = openDeleteNothingModelCommand;
    private readonly OpenUpdateNothingModelCommand _openUpdateNothingModelCommand = openUpdateNothingModelCommand;

    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelDto">Данные модели</param>
    /// <returns>Объект данных представления модели</returns>
    public NothingModelVM Create(NothingModelDto nothingModelDto)
    {
        var nothingModelVM = new NothingModelVM()
        {
            Id = nothingModelDto.Id,
            Name = nothingModelDto.Name,
            DeleteButtonVM = new DeleteButtonVM(_openDeleteNothingModelCommand),
            UpdateButtonVM = new UpdateButtonVM(_openUpdateNothingModelCommand),
        };
        return nothingModelVM;
    }

    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelWebDto">Данные модели</param>
    /// <returns>Объект данных представления модели</returns>
    public NothingModelVM Create(NothingModelWebDto nothingModelWebDto)
    {
        var nothingModelVM = new NothingModelVM()
        {
            Id = nothingModelWebDto.Id,
            Name = nothingModelWebDto.Name,
            DeleteButtonVM = new DeleteButtonVM(_openDeleteNothingModelCommand),
            UpdateButtonVM = new UpdateButtonVM(_openUpdateNothingModelCommand),
        };
        return nothingModelVM;
    }
}