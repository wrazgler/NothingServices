namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Представление, отображаемое на главном окне
/// </summary>
public interface IMainWindowContentVM
{
    /// <summary>
    /// Нужно ли отображать контент на главном окне
    /// </summary>
    public bool Visible { get; set; }
}