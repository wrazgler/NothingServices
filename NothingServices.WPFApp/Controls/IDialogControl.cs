namespace NothingServices.WPFApp.Controls;

/// <summary>
/// Представление, отображаемое в диалоговом окне
/// </summary>
public interface IDialogControl
{
    /// <summary>
    /// Контекст данных в диалоговом окне
    /// </summary>
    public object DataContext { get; set; }
}