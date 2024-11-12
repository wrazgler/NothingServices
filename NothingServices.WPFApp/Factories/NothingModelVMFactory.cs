using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления модели
/// </summary>
/// <param name="deleteButtonVM">Кнопка удалить модель</param>
/// <param name="updateButtonVM">Кнопка обновить модель</param>
public class NothingModelVMFactory(
    DeleteButtonVM deleteButtonVM,
    UpdateButtonVM updateButtonVM)
    : INothingModelVMFactory
{
    private readonly DeleteButtonVM _deleteButtonVM = deleteButtonVM;
    private readonly UpdateButtonVM _updateButtonVM = updateButtonVM;

    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelDto">Данные модели</param>
    public NothingModelVM Create(NothingModelDto nothingModelDto)
    {
        var nothingModelVM = new NothingModelVM()
        {
            Id = nothingModelDto.Id,
            Name = nothingModelDto.Name,
            DeleteButtonVM = _deleteButtonVM,
            UpdateButtonVM = _updateButtonVM,
        };
        return nothingModelVM;
    }

    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelWebDto">Данные модели</param>
    public NothingModelVM Create(NothingModelWebDto nothingModelWebDto)
    {
        var nothingModelVM = new NothingModelVM()
        {
            Id = nothingModelWebDto.Id,
            Name = nothingModelWebDto.Name,
            DeleteButtonVM = _deleteButtonVM,
            UpdateButtonVM = _updateButtonVM,
        };
        return nothingModelVM;
    }
}