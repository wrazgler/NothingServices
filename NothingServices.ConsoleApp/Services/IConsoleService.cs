namespace NothingServices.ConsoleApp.Services;

/// <summary>
/// Сервис работы с консолью
/// </summary>
public interface IConsoleService
{
    /// <summary>
    /// Запись сообщения в консоль
    /// </summary>
    /// <param name="message">Сообщение</param>
    void WriteLine(string? message = null);

    /// <summary>
    /// Чтения строки из консоли
    /// </summary>
    string? ReadLine();
}