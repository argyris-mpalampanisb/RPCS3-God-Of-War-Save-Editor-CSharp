using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GodOfWarCSharpSE.ViewModels;

namespace GodOfWarCSharpSE.Views;

public partial class GoW2View : Window
{
    public GoW2View()
    {
        AvaloniaXamlLoader.Load(this);
        var vm = new GoW2ViewModel();
        vm.SetTopLevel(this);
        DataContext = vm;
    }
}