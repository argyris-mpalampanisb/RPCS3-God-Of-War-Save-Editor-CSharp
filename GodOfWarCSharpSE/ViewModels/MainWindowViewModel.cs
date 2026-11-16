using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using GodOfWarCSharpSE.Views;

namespace GodOfWarCSharpSE.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand] private void OpenGoW1() => OpenEditor(new GoW1View());
    [RelayCommand] private void OpenGoW2() => OpenEditor(new GoW2View());
    [RelayCommand] private void OpenGoW3() => OpenEditor(new GoW3View());
    [RelayCommand] private void OpenGoA()  => OpenEditor(new GoWAView());

    /// <summary>
    /// Shows an editor window and hides the launcher while it is open.
    /// When the editor closes, the launcher comes back. Previously the launcher
    /// stayed hidden forever, and because a hidden window still counts as "open",
    /// the process never exited after the editor was closed.
    /// </summary>
    private static void OpenEditor(Window editor)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            editor.Show();
            return;
        }

        var launcher = desktop.MainWindow;
        editor.Closed += (_, _) =>
        {
            if (launcher is { IsVisible: false })
            {
                launcher.Show();
                launcher.Activate();
            }
        };

        editor.Show();
        launcher?.Hide();
    }
}
