using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда создать новую модель
/// </summary>
public class CreateCommand(INothingApiClientStrategy strategy)
    : BaseCommand
{
    private readonly INothingApiClientStrategy _strategy = strategy;
    private readonly CancellationTokenSource _cancellationTokenSource = new(10000);

    /// <summary>
    /// Проверка возможности выполнить команду создать новую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not CreateNothingModelVM createNothingModelVM)
            return false;

        if (string.IsNullOrEmpty(createNothingModelVM.Name))
            return false;

        return true;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            if (parameter is not CreateNothingModelVM createNothingModelVM)
                throw new ArgumentException(parameter?.GetType().Name);
            var nothingModelVM = await _strategy.CreateNothingModelAsync(
                createNothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            return;
        }
    }
}