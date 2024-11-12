using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления обновить модель
/// </summary>
/// <param name="nothingModelVM">Данные представления модели</param>
public class UpdateNothingModelVM(NothingModelVM nothingModelVM) : ObservableObject
{
    private const string TitleFormat = "Введите новое имя модели: {0}";
    private string _name = nothingModelVM.Name;

    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; } = nothingModelVM.Id;

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
    /// Заголовок окна обновить модель
    /// </summary>
    public string Title { get; } = string.Format(TitleFormat, nothingModelVM.Name);
}