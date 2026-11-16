using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GodOfWarCSharpSE.ViewModels;

namespace GodOfWarCSharpSE.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}