namespace NothingServices.WPFApp.Services;

/// <summary>
/// Провайдер версии приложения
/// </summary>
public interface IAppVersionProvider
{
    /// <summary>
    /// Получить текущую версию приложения
    /// </summary>
    /// <returns>Текущая версия приложения</returns>
    Version GetVersion();
}