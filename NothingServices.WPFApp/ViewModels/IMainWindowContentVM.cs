namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления, отображаемое на главном окне
/// </summary>
public interface IMainWindowContentVM
{
    /// <summary>
    /// Нужно ли отображать контент на главном окне
    /// </summary>
    public bool Active { get; set; }
}