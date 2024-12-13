using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки gRpc Api
/// </summary>
public interface IGRpcApiButtonVM : IButtonVM
{
    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    public INothingApiClientStrategy Strategy { get; }
}