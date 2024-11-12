using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления создать модель
/// </summary>
public class CreateNothingModelVM : ObservableObject
{
    private string _name = string.Empty;

    /// <summary>
    /// Имя модели
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
                return;

            _name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Заголовок окна создать модель
    /// </summary>
    public string Title { get; } = "Введите имя новой модели";
}