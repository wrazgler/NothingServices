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
/// <param name="openNothingModelsListCommand">
/// Команда открыть представление окна списка моделей
/// </param>
public class DeleteNothingModelVMFactory(
    DeleteCommand deleteCommand,
    OpenNothingModelsListCommand openNothingModelsListCommand)
    : IDeleteNothingModelVMFactory
{
    private readonly DeleteCommand _deleteCommand = deleteCommand;
    private readonly OpenNothingModelsListCommand _openNothingModelsListCommand = openNothingModelsListCommand;

    /// <summary>
    /// Создать объект данных представления окна удалить модель
    /// </summary>
    /// <param name="nothingModelVM">Объект данных представления модели</param>
    /// <returns>Объект данных представления окна удалить модель</returns>
    public DeleteNothingModelVM Create(INothingModelVM nothingModelVM)
    {
        var cancelButtonVM = new CancelButtonVM(_openNothingModelsListCommand);
        var deleteButtonVM = new DeleteButtonVM(_deleteCommand);
        var deleteNothingModelVM = new DeleteNothingModelVM(cancelButtonVM, deleteButtonVM, nothingModelVM);
        return deleteNothingModelVM;
    }
}