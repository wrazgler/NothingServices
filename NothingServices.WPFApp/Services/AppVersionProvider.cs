using System.Reflection;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Провайдер версии приложения
/// </summary>
public class AppVersionProvider : IAppVersionProvider
{
    /// <summary>
    /// Получить текущую версию приложения
    /// </summary>
    /// <returns>Текущая версия приложения</returns>
    public Version GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version
            ?? throw new NullReferenceException();
        return version;
    }
}