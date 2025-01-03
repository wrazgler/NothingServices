using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна удалить модель
/// </summary>
/// <param name="deleteCommand">
/// Команда создать удалить модель
/// </param>
/// <param name="closeDialogCommand">
/// Команда закрыть представление диалогового окна
/// </param>
public sealed class DeleteNothingModelVMFactory(
    ICloseDialogCommand closeDialogCommand,
    IDeleteCommand deleteCommand)
    : IDeleteNothingModelVMFactory
{
    private readonly ICloseDialogCommand _closeDialogCommand = closeDialogCommand;
    private readonly IDeleteCommand _deleteCommand = deleteCommand;

    /// <summary>
    /// Создать объект данных представления окна удалить модель
    /// </summary>
    /// <param name="nothingModelVM">Объект данных представления модели</param>
    /// <returns>Объект данных представления окна удалить модель</returns>
    public DeleteNothingModelVM Create(INothingModelVM nothingModelVM)
    {
        var cancelButtonVM = new CancelButtonVM(_closeDialogCommand);
        var deleteButtonVM = new DeleteButtonVM(_deleteCommand);
        var deleteNothingModelVM = new DeleteNothingModelVM(cancelButtonVM, deleteButtonVM, nothingModelVM);
        return deleteNothingModelVM;
    }
}