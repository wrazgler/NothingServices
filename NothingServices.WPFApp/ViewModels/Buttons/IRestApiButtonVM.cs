using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки Rest Api
/// </summary>
public interface IRestApiButtonVM : IButtonVM
{
    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    public INothingApiClientStrategy Strategy { get; }
}