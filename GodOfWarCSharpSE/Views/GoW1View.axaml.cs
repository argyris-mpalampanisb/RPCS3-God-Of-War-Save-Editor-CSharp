using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GodOfWarCSharpSE.ViewModels;

namespace GodOfWarCSharpSE.Views;

public partial class GoW1View : Window
{
    public GoW1View()
    {
        InitializeComponent();
        var vm = new GoW1ViewModel();
        vm.SetTopLevel(this);
        DataContext = vm;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}   