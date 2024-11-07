namespace NothingServices.ConsoleApp.Services;

/// <summary>
/// Сервис работы с консолью
/// </summary>
public class ConsoleService : IConsoleService
{
    /// <summary>
    /// Запись сообщения в консоль
    /// </summary>
    /// <param name="message">Сообщение</param>
    public void WriteLine(string? message = null)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Чтения строки из консоли
    /// </summary>
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}