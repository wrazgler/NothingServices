using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна создать модель
/// </summary>
/// <param name="createCommand">
/// Команда создать новую модель
/// </param>
/// <param name="closeDialogCommand">
/// Команда закрыть представление диалогового окна
/// </param>
public class CreateNothingModelVMFactory(
    ICloseDialogCommand closeDialogCommand,
    ICreateCommand createCommand)
    : ICreateNothingModelVMFactory
{
    private readonly ICloseDialogCommand _closeDialogCommand = closeDialogCommand;
    private readonly ICreateCommand _createCommand = createCommand;

    /// <summary>
    /// Создать объект данных представления окна создать модель
    /// </summary>
    /// <returns>Объект данных представления окна создать модель</returns>
    public CreateNothingModelVM Create()
    {
        var cancelButtonVM = new CancelButtonVM(_closeDialogCommand);
        var createButtonVM = new CreateButtonVM(_createCommand);
        var createNothingModelVM = new CreateNothingModelVM(cancelButtonVM, createButtonVM);
        return createNothingModelVM;
    }
}