using System.ComponentModel;

namespace GodOfWarCSharpSE.ViewModels;

public class InventoryItem : INotifyPropertyChanged
{
    public string Name { get; }
    private bool _isChecked;

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }
    }

    public InventoryItem(string name, bool isChecked = false)
    {
        Name = name;
        _isChecked = isChecked;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}