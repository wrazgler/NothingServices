using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Factories;

/// <summary>
/// Фабрика создания объекта данных представления модели
/// </summary>
public interface INothingModelVMFactory
{
    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelDto">Данные модели</param>
    /// <returns>Объект данных представления модели</returns>
    NothingModelVM Create(NothingModelDto nothingModelDto);

    /// <summary>
    /// Создать объект данных представления модели
    /// </summary>
    /// <param name="nothingModelWebDto">Данные модели</param>
    /// <returns>Объект данных представления модели</returns>
    NothingModelVM Create(NothingModelWebDto nothingModelWebDto);
}