using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления окна создать модель
/// </summary>
public interface ICreateNothingModelVMFactory
{
    /// <summary>
    /// Создать объект данных представления окна создать модель
    /// </summary>
    /// <returns>Объект данных представления окна создать модель</returns>
    CreateNothingModelVM Create();
}