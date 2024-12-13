using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна обновить модель
/// </summary>
public interface IUpdateNothingModelVMFactory
{
    /// <summary>
    /// Создать объект данных представления окна обновить модель
    /// </summary>
    /// <param name="nothingModelVM">Объект данных представления модели</param>
    /// <returns>Объект данных представления окна обновить модель</returns>
    UpdateNothingModelVM Create(INothingModelVM nothingModelVM);
}