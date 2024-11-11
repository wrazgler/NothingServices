namespace NothingServices.WPFApp.ViewModels.MainWindowContent;

/// <summary>
/// Контракт отображения контента на главном окне
/// </summary>
public interface IMainWindowContentVM
{
    /// <summary>
    /// Нужно ли отображать контент на главном окне
    /// </summary>
    public bool Visible { get; set; }
}