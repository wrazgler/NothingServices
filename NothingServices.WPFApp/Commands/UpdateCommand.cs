using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда обновить существующую модель
/// </summary>
public class UpdateCommand(
    INotificator notificator,
    INothingApiClientStrategy strategy)
    : BaseCommand
{
    private readonly INotificator _notificator = notificator;
    private readonly INothingApiClientStrategy _strategy = strategy;
    private readonly CancellationTokenSource _cancellationTokenSource = new(10000);

    /// <summary>
    /// Проверка возможности выполнить команду обновить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not UpdateNothingModelVM updateNothingModelVM)
            return false;

        if (updateNothingModelVM.Id == 0)
            return false;

        if (string.IsNullOrEmpty(updateNothingModelVM.Name))
            return false;

        return true;
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            if (parameter is not UpdateNothingModelVM updateNothingModelVM)
                throw new ArgumentException(parameter?.GetType().Name);
            var nothingModelVM = await _strategy.UpdateNothingModelAsync(
                updateNothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}