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
/// <param name="openNothingModelsListCommand">
/// Команда открыть представление окна списка моделей
/// </param>
public class CreateNothingModelVMFactory(
    CreateCommand createCommand,
    OpenNothingModelsListCommand openNothingModelsListCommand)
    : ICreateNothingModelVMFactory
{
    private readonly CreateCommand _createCommand = createCommand;
    private readonly OpenNothingModelsListCommand _openNothingModelsListCommand = openNothingModelsListCommand;

    /// <summary>
    /// Создать объект данных представления окна создать модель
    /// </summary>
    /// <returns>Объект данных представления окна создать модель</returns>
    public CreateNothingModelVM Create()
    {
        var cancelButtonVM = new CancelButtonVM(_openNothingModelsListCommand);
        var createButtonVM = new CreateButtonVM(_createCommand);
        var createNothingModelVM = new CreateNothingModelVM(cancelButtonVM, createButtonVM);
        return createNothingModelVM;
    }
}