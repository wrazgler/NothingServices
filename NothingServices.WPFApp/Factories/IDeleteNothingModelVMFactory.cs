using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна удалить модель
/// </summary>
public interface IDeleteNothingModelVMFactory
{
    /// <summary>
    /// Создать объект данных представления окна удалить модель
    /// </summary>
    /// <param name="nothingModelVM">Объект данных представления модели</param>
    /// <returns>Объект данных представления окна удалить модель</returns>
    DeleteNothingModelVM Create(INothingModelVM nothingModelVM);
}