using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GodOfWarCSharpSE.ViewModels;

namespace GodOfWarCSharpSE.Views;

public partial class GoW3View : Window
{
    public GoW3View()
    {
        AvaloniaXamlLoader.Load(this);
        var vm = new GoW3ViewModel();
        vm.SetTopLevel(this);
        DataContext = vm;
    }
}