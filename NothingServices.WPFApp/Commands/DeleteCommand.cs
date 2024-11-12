using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда удалить модель
/// </summary>
public class DeleteCommand(INothingApiClientStrategy strategy)
    : BaseCommand
{
    private readonly INothingApiClientStrategy _strategy = strategy;
    private readonly CancellationTokenSource _cancellationTokenSource = new(10000);

    /// <summary>
    /// Проверка возможности выполнить команду удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not NothingModelVM nothingModelVM)
            return false;

        if (nothingModelVM.Id == 0)
            return false;

        return true;
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            if (parameter is not NothingModelVM nothingModelVM)
                throw new ArgumentException(parameter?.GetType().Name);
            await _strategy.DeleteNothingModelAsync(
                nothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            return;
        }
    }
}