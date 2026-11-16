using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GodOfWarCSharpSE.Models;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace GodOfWarCSharpSE.ViewModels;

public partial class GoW3ViewModel : ObservableObject
{
    private readonly GoW3Save _save = new();
    private TopLevel? _topLevel;

    public void SetTopLevel(TopLevel topLevel) => _topLevel = topLevel;

    // --- State ---
    [ObservableProperty] private string _folder = "";
    [ObservableProperty] private bool _isLoaded;
    [ObservableProperty] private bool _isUserData;

    // --- UserData ---
    [ObservableProperty] private bool _ch5;
    [ObservableProperty] private bool _ch6;
    [ObservableProperty] private bool _ch7;
    [ObservableProperty] private bool _ch1;
    [ObservableProperty] private bool _ch2;
    [ObservableProperty] private bool _ch3;
    [ObservableProperty] private bool _ch4;
    [ObservableProperty] private bool _beatEasy;
    [ObservableProperty] private bool _beatNormal;
    [ObservableProperty] private bool _beatHard;
    [ObservableProperty] private bool _beatVeryHard;
    [ObservableProperty] private string _musVol = "";
    [ObservableProperty] private string _speechVol = "";
    [ObservableProperty] private string _sfxVol = "";
    [ObservableProperty] private string _cineVol = "";
    [ObservableProperty] private string _soundMode = "";
    [ObservableProperty] private string _brightness = "";
    [ObservableProperty] private string _screenScale = "";
    [ObservableProperty] private string _screenLocX = "";
    [ObservableProperty] private string _screenLocY = "";
    [ObservableProperty] private string _tutDisplay = "";
    [ObservableProperty] private bool _showSubs;
    [ObservableProperty] private bool _invertFlight;
    [ObservableProperty] private bool _invertFreeCam;
    [ObservableProperty] private int _defaultCostume;

    // --- SaveData ---
    [ObservableProperty] private string _saveDesc = "";
    [ObservableProperty] private string _saveArea = "";
    [ObservableProperty] private string _xPos = "";
    [ObservableProperty] private string _yPos = "";
    [ObservableProperty] private string _zPos = "";
    [ObservableProperty] private string _currHealth = "";
    [ObservableProperty] private string _currMagic = "";
    [ObservableProperty] private string _currItem = "";
    [ObservableProperty] private string _redOrbs = "";
    [ObservableProperty] private string _healthExt = "";
    [ObservableProperty] private string _magicExt = "";
    [ObservableProperty] private string _itemExt = "";
    [ObservableProperty] private string _bowLevel = "";
    [ObservableProperty] private string _bootsLevel = "";
    [ObservableProperty] private string _headLevel = "";
    [ObservableProperty] private string _cestusLevel = "";
    [ObservableProperty] private string _whipLevel = "";
    [ObservableProperty] private string _bladesLevel = "";
    [ObservableProperty] private string _clawsLevel = "";
    [ObservableProperty] private bool _aphroditesGarter;
    [ObservableProperty] private bool _hephaestusRing;
    [ObservableProperty] private bool _herasChalice;
    [ObservableProperty] private bool _zeusEagle;
    [ObservableProperty] private bool _daedalusSchematics;
    [ObservableProperty] private bool _whipUnlock;
    [ObservableProperty] private bool _bladesUnlock;
    [ObservableProperty] private bool _clawsUnlock;
    [ObservableProperty] private bool _hadesHelm;
    [ObservableProperty] private bool _heliosShield;
    [ObservableProperty] private bool _hermesCoin;
    [ObservableProperty] private bool _herculesShouldGuard;
    [ObservableProperty] private bool _poseidonsConchShell;
    [ObservableProperty] private bool _bladeOfOlympusRage;
    [ObservableProperty] private bool _bootsUnlock;
    [ObservableProperty] private bool _poseidonsTrident;
    [ObservableProperty] private bool _bladeOfOlympus;
    [ObservableProperty] private bool _cestusUnlock;
    [ObservableProperty] private bool _bowUnlock;
    [ObservableProperty] private bool _headUnlock;
    [ObservableProperty] private bool _boreasIcestorm;
    [ObservableProperty] private bool _goldenFleece;
    [ObservableProperty] private bool _icarusWings;
    [ObservableProperty] private int _currDiff;
    [ObservableProperty] private int _currCost;
    [ObservableProperty] private bool _healthInfinite;
    [ObservableProperty] private bool _magicInfinite;
    [ObservableProperty] private bool _itemInfinite;

    // --- Commands ---
    [RelayCommand]
    private async Task BrowseAsync()
    {
        if (_topLevel is null) return;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select SAVEDATA",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("GoW3 Save File") { Patterns = new[] { "SAVEDATA" } } }
        });
        if (files.Count > 0)
            Folder = Path.GetDirectoryName(files[0].Path.LocalPath)!;
    }

    [RelayCommand]
    private void Open()
    {
        if (!Directory.Exists(Folder)) return;
        if (_save.Load(Folder))
        {
            IsLoaded = true;
            IsUserData = _save.IsUserData;
            if (IsUserData) LoadUserDataValues();
            else LoadSaveDataValues();
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (IsUserData) WriteUserDataValues();
        else WriteSaveDataValues();
        _save.Save();
    }

    // --- Load ---
    private void LoadUserDataValues()
    {
        Ch5 = _save.Ch5; Ch6 = _save.Ch6; Ch7 = _save.Ch7;
        Ch1 = _save.Ch1; Ch2 = _save.Ch2; Ch3 = _save.Ch3; Ch4 = _save.Ch4;
        BeatEasy = _save.BeatEasy; BeatNormal = _save.BeatNormal;
        BeatHard = _save.BeatHard; BeatVeryHard = _save.BeatVeryHard;
        MusVol = _save.MusVol.ToString();
        SpeechVol = _save.SpeechVol.ToString();
        SfxVol = _save.SfxVol.ToString();
        CineVol = _save.CineVol.ToString();
        SoundMode = _save.SoundMode.ToString();
        Brightness = _save.Brightness.ToString();
        ScreenScale = _save.ScreenScale.ToString();
        ScreenLocX = _save.ScreenLocX.ToString();
        ScreenLocY = _save.ScreenLocY.ToString();
        TutDisplay = _save.TutDisplay.ToString();
        ShowSubs = _save.ShowSubs;
        InvertFlight = _save.InvertFlight;
        InvertFreeCam = _save.InvertFreeCam;
        DefaultCostume = _save.DefaultCostume;
    }

    private void LoadSaveDataValues()
    {
        SaveDesc = _save.SaveDesc;
        SaveArea = _save.SaveArea;
        XPos = _save.XPos.ToString("0.##");
        YPos = _save.YPos.ToString("0.##");
        ZPos = _save.ZPos.ToString("0.##");
        CurrHealth = _save.CurrHealth.ToString("0.##");
        CurrMagic = _save.CurrMagic.ToString("0.##");
        CurrItem = _save.CurrItem.ToString("0.##");
        RedOrbs = _save.RedOrbs.ToString("0.##");
        HealthExt = _save.HealthExt.ToString();
        MagicExt = _save.MagicExt.ToString();
        ItemExt = _save.ItemExt.ToString();
        BowLevel = _save.BowLevel.ToString();
        BootsLevel = _save.BootsLevel.ToString();
        HeadLevel = _save.HeadLevel.ToString();
        CestusLevel = _save.CestusLevel.ToString();
        WhipLevel = _save.WhipLevel.ToString();
        BladesLevel = _save.BladesLevel.ToString();
        ClawsLevel = _save.ClawsLevel.ToString();
        AphroditesGarter = _save.AphroditesGarter;
        HephaestusRing = _save.HephaestusRing;
        HerasChalice = _save.HerasChalice;
        ZeusEagle = _save.ZeusEagle;
        DaedalusSchematics = _save.DaedalusSchematics;
        WhipUnlock = _save.WhipUnlock;
        BladesUnlock = _save.BladesUnlock;
        ClawsUnlock = _save.ClawsUnlock;
        HadesHelm = _save.HadesHelm;
        HeliosShield = _save.HeliosShield;
        HermesCoin = _save.HermesCoin;
        HerculesShouldGuard = _save.HerculesShouldGuard;
        PoseidonsConchShell = _save.PoseidonsConchShell;
        BladeOfOlympusRage = _save.BladeOfOlympusRage;
        BootsUnlock = _save.BootsUnlock;
        PoseidonsTrident = _save.PoseidonsTrident;
        BladeOfOlympus = _save.BladeOfOlympus;
        CestusUnlock = _save.CestusUnlock;
        BowUnlock = _save.BowUnlock;
        HeadUnlock = _save.HeadUnlock;
        BoreasIcestorm = _save.BoreasIcestorm;
        GoldenFleece = _save.GoldenFleece;
        IcarusWings = _save.IcarusWings;
        CurrDiff = _save.CurrDiff;
        CurrCost = _save.CurrCost;
        HealthInfinite = _save.HealthInfinite;
        MagicInfinite = _save.MagicInfinite;
        ItemInfinite = _save.ItemInfinite;
    }

    // --- Write ---
    private void WriteUserDataValues()
    {
        _save.Ch5 = Ch5; _save.Ch6 = Ch6; _save.Ch7 = Ch7;
        _save.Ch1 = Ch1; _save.Ch2 = Ch2; _save.Ch3 = Ch3; _save.Ch4 = Ch4;
        _save.BeatEasy = BeatEasy; _save.BeatNormal = BeatNormal;
        _save.BeatHard = BeatHard; _save.BeatVeryHard = BeatVeryHard;
        if (byte.TryParse(MusVol, out var mv)) _save.MusVol = mv;
        if (byte.TryParse(SpeechVol, out var sv)) _save.SpeechVol = sv;
        if (byte.TryParse(SfxVol, out var fx)) _save.SfxVol = fx;
        if (byte.TryParse(CineVol, out var cv)) _save.CineVol = cv;
        if (byte.TryParse(SoundMode, out var sm)) _save.SoundMode = sm;
        if (byte.TryParse(Brightness, out var br)) _save.Brightness = br;
        if (byte.TryParse(ScreenScale, out var ss)) _save.ScreenScale = ss;
        if (byte.TryParse(ScreenLocX, out var sx)) _save.ScreenLocX = sx;
        if (byte.TryParse(ScreenLocY, out var sy)) _save.ScreenLocY = sy;
        if (byte.TryParse(TutDisplay, out var td)) _save.TutDisplay = td;
        _save.ShowSubs = ShowSubs;
        _save.InvertFlight = InvertFlight;
        _save.InvertFreeCam = InvertFreeCam;
        _save.DefaultCostume = (byte)DefaultCostume;
    }

    private void WriteSaveDataValues()
    {
        _save.SaveDesc = SaveDesc;
        _save.SaveArea = SaveArea;
        if (float.TryParse(XPos, out var x)) _save.XPos = x;
        if (float.TryParse(YPos, out var y)) _save.YPos = y;
        if (float.TryParse(ZPos, out var z)) _save.ZPos = z;
        if (float.TryParse(CurrHealth, out var h)) _save.CurrHealth = h;
        if (float.TryParse(CurrMagic, out var m)) _save.CurrMagic = m;
        if (float.TryParse(CurrItem, out var i)) _save.CurrItem = i;
        if (float.TryParse(RedOrbs, out var r)) _save.RedOrbs = r;
        if (byte.TryParse(HealthExt, out var he)) _save.HealthExt = he;
        if (byte.TryParse(MagicExt, out var me)) _save.MagicExt = me;
        if (byte.TryParse(ItemExt, out var ie)) _save.ItemExt = ie;
        if (byte.TryParse(BowLevel, out var bl)) _save.BowLevel = bl;
        if (byte.TryParse(BootsLevel, out var bo)) _save.BootsLevel = bo;
        if (byte.TryParse(HeadLevel, out var hd)) _save.HeadLevel = hd;
        if (byte.TryParse(CestusLevel, out var ce)) _save.CestusLevel = ce;
        if (byte.TryParse(WhipLevel, out var wh)) _save.WhipLevel = wh;
        if (byte.TryParse(BladesLevel, out var bd)) _save.BladesLevel = bd;
        if (byte.TryParse(ClawsLevel, out var cl)) _save.ClawsLevel = cl;
        _save.AphroditesGarter = AphroditesGarter;
        _save.HephaestusRing = HephaestusRing;
        _save.HerasChalice = HerasChalice;
        _save.ZeusEagle = ZeusEagle;
        _save.DaedalusSchematics = DaedalusSchematics;
        _save.WhipUnlock = WhipUnlock;
        _save.BladesUnlock = BladesUnlock;
        _save.ClawsUnlock = ClawsUnlock;
        _save.HadesHelm = HadesHelm;
        _save.HeliosShield = HeliosShield;
        _save.HermesCoin = HermesCoin;
        _save.HerculesShouldGuard = HerculesShouldGuard;
        _save.PoseidonsConchShell = PoseidonsConchShell;
        _save.BladeOfOlympusRage = BladeOfOlympusRage;
        _save.BootsUnlock = BootsUnlock;
        _save.PoseidonsTrident = PoseidonsTrident;
        _save.BladeOfOlympus = BladeOfOlympus;
        _save.CestusUnlock = CestusUnlock;
        _save.BowUnlock = BowUnlock;
        _save.HeadUnlock = HeadUnlock;
        _save.BoreasIcestorm = BoreasIcestorm;
        _save.GoldenFleece = GoldenFleece;
        _save.IcarusWings = IcarusWings;
        _save.CurrDiff = (byte)CurrDiff;
        _save.CurrCost = (byte)CurrCost;
        _save.HealthInfinite = HealthInfinite;
        _save.MagicInfinite = MagicInfinite;
        _save.ItemInfinite = ItemInfinite;
    }
}