using System.Windows.Input;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Базовый класс команды
/// </summary>
public abstract class BaseCommand : ICommand
{
    /// <summary>
    /// Обработчик события команды
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Проверка возможности выполнить команду
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public abstract bool CanExecute(object? parameter);

    /// <summary>
    /// Бизнес логика команды
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public abstract void Execute(object? parameter);
}