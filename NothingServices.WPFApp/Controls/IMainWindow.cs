namespace NothingServices.WPFApp.Controls;

/// <summary>
/// Представление главного окна
/// </summary>
public interface IMainWindow
{
    /// <summary>
    /// Контекст данных главного окна
    /// </summary>
    public object DataContext { get; set; }

    /// <summary>
    /// Показать элементы главного окна
    /// </summary>
    public void Show();
}