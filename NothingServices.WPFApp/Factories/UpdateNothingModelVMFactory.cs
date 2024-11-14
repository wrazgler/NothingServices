using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна обновить модель
/// </summary>
/// <param name="updateCommand">
/// Команда создать обновить модель
/// </param>
/// <param name="openNothingModelsListCommand">
/// Команда открыть представление окна списка моделей
/// </param>
public class UpdateNothingModelVMFactory(
    UpdateCommand updateCommand,
    OpenNothingModelsListCommand openNothingModelsListCommand)
    : IUpdateNothingModelVMFactory
{
    private readonly UpdateCommand _updateCommand = updateCommand;
    private readonly OpenNothingModelsListCommand _openNothingModelsListCommand = openNothingModelsListCommand;

    /// <summary>
    /// Создать объект данных представления окна обновить модель
    /// </summary>
    /// <param name="nothingModelVM">Объект данных представления модели</param>
    /// <returns>Объект данных представления окна обновить модель</returns>
    public UpdateNothingModelVM Create(INothingModelVM nothingModelVM)
    {
        var cancelButtonVM = new CancelButtonVM(_openNothingModelsListCommand);
        var updateButtonVM = new UpdateButtonVM(_updateCommand);
        var updateNothingModelVM = new UpdateNothingModelVM(cancelButtonVM, updateButtonVM, nothingModelVM);
        return updateNothingModelVM;
    }
}