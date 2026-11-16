using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using GodOfWarCSharpSE.ViewModels;

namespace GodOfWarCSharpSE.Views;

public partial class GoWAView : Window
{
    private GoWAViewModel _vm;

    public GoWAView()
    {
        InitializeComponent();
        _vm = new GoWAViewModel();
        DataContext = _vm;
        WireBrowse();
    }

    public GoWAView(GoWAViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;
        WireBrowse();
    }

    private void WireBrowse()
    {
        _vm.BrowseRequested += async () =>
        {
            var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select your RPCS3 save folder (containing DAT-1)",
                AllowMultiple = false
            });
            if (folders.Count > 0)
                _vm.Folder = folders[0].Path.LocalPath;
        };
    }
}
