using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using GodOfWarCSharpSE.Views;

namespace GodOfWarCSharpSE.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand]
    private void OpenGoW1()
    {
        var window = new GoW1View();
        window.Show();

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Windows.FirstOrDefault(w => w is MainWindow)?.Hide();
    }

   [RelayCommand]
private void OpenGoW2()
{
    var window = new GoW2View();
    window.Show();
    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        desktop.Windows.FirstOrDefault(w => w is MainWindow)?.Hide();
}
   [RelayCommand]
private void OpenGoW3()
{
    var window = new GoW3View();
    window.Show();
    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        desktop.Windows.FirstOrDefault(w => w is MainWindow)?.Hide();
}
    [RelayCommand] private void OpenGoA() { }
}