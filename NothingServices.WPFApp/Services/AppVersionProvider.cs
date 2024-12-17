using System.Reflection;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Провайдер версии приложения
/// </summary>
public sealed class AppVersionProvider : IAppVersionProvider
{
    /// <summary>
    /// Получить текущую версию приложения
    /// </summary>
    /// <returns>Текущая версия приложения</returns>
    /// <exception cref="NullReferenceException">
    /// Ошибка, возникшая при получении версии сборки
    /// </exception>
    public Version GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version
            ?? throw new NullReferenceException();
        return version;
    }
}