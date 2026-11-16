using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GodOfWarCSharpSE.Models;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Reflection;

namespace GodOfWarCSharpSE.ViewModels;

public partial class GoW2ViewModel : ObservableObject
{
    private readonly GoW2Save _save = new();
    private string[] _movieNames = [];
    private TopLevel? _topLevel;

    public GoW2ViewModel()
    {
        var asm = Assembly.GetExecutingAssembly();
        using var ms = asm.GetManifestResourceStream("GodOfWarCSharpSE.Data.Gow2Movies.txt");
        if (ms is not null)
        {
            using var sr = new StreamReader(ms);
            _movieNames = sr.ReadToEnd().Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public void SetTopLevel(TopLevel topLevel) => _topLevel = topLevel;

    // --- State ---
    [ObservableProperty] private string _folder = "";
    [ObservableProperty] private bool _isLoaded;
    [ObservableProperty] private bool _isMaster;
    [ObservableProperty] private int _selectedSlot = -1;

    // --- Master ---
    [ObservableProperty] private bool _cyclopsEyesMaster;
    [ObservableProperty] private bool _beatEMaster;
    [ObservableProperty] private bool _beatNMaster;
    [ObservableProperty] private bool _beatHMaster;
    [ObservableProperty] private bool _beatVHMaster;
    [ObservableProperty] private bool _mortalRankMaster;
    [ObservableProperty] private bool _spartanRankMaster;
    [ObservableProperty] private bool _godRankMaster;
    [ObservableProperty] private bool _titanRankMaster;

    // --- Character ---
    [ObservableProperty] private string _health = "";
    [ObservableProperty] private string _magic = "";
    [ObservableProperty] private string _item = "";
    [ObservableProperty] private string _rage = "";
    [ObservableProperty] private string _magicRegen = "";
    [ObservableProperty] private string _redOrbs = "";
    [ObservableProperty] private string _healthExt = "";
    [ObservableProperty] private string _magicExt = "";
    [ObservableProperty] private string _itemExt = "";
    [ObservableProperty] private string _gorgonEyes = "";
    [ObservableProperty] private string _phoenixFeathers = "";
    [ObservableProperty] private string _cyclopsEyes = "";
    [ObservableProperty] private string _secondsPlayed = "";
    [ObservableProperty] private bool _swimming;
    [ObservableProperty] private int _costume;
    [ObservableProperty] private int _difficulty;
    [ObservableProperty] private bool _infiniteHealth;
    [ObservableProperty] private bool _infiniteMagic;
    [ObservableProperty] private bool _infiniteRage;
    [ObservableProperty] private bool _bonus;
    [ObservableProperty] private string _moviesSeen = "";

    // --- Equipment ---
    [ObservableProperty] private string _boAlvl = "";
    [ObservableProperty] private string _BoOlvl = "";
    [ObservableProperty] private string _SoDlvl = "";
    [ObservableProperty] private string _Bhlvl = "";
    [ObservableProperty] private string _Prlvl = "";
    [ObservableProperty] private string _Crlvl = "";
    [ObservableProperty] private string _Tblvl = "";
    [ObservableProperty] private string _Aqlvl = "";
    [ObservableProperty] private string _Ehlvl = "";
    [ObservableProperty] private int _selectedWeapon;
    [ObservableProperty] private bool _boOUnl;
    [ObservableProperty] private bool _soDUnl;
    [ObservableProperty] private bool _BhUnl;
    [ObservableProperty] private bool _PrAcq;
    [ObservableProperty] private bool _CrAcq;
    [ObservableProperty] private bool _TbAcq;
    [ObservableProperty] private bool _AqAcq;
    [ObservableProperty] private bool _EhAcq;
    [ObservableProperty] private bool _rageAcq;
    [ObservableProperty] private bool _amuletAcq;
    [ObservableProperty] private bool _fleeceAcq;
    [ObservableProperty] private bool _wingsAcq;
    [ObservableProperty] private bool _urnGaiaAcq;
    [ObservableProperty] private bool _urnGorgonAcq;
    [ObservableProperty] private bool _urnOlympusAcq;
    [ObservableProperty] private bool _urnPrometheusAcq;
    [ObservableProperty] private bool _urnFatesAcq;
    [ObservableProperty] private bool _urnPoseidonAcq;
    [ObservableProperty] private bool _urnGaiaActive;
    [ObservableProperty] private bool _urnGorgonsActive;
    [ObservableProperty] private bool _urnOlympusActive;
    [ObservableProperty] private bool _urnPrometheusActive;
    [ObservableProperty] private bool _urnFatesActive;
    [ObservableProperty] private bool _urnPoseidonActive;
    [ObservableProperty] private bool _unkUnl1_02;
    [ObservableProperty] private bool _unkUnl2_01;
    [ObservableProperty] private bool _unkUnl2_08;
    [ObservableProperty] private bool _unkUnl2_10;
    [ObservableProperty] private bool _unkUnl3_08;
    [ObservableProperty] private bool _unkUnl3_80;

    // --- Misc ---
    [ObservableProperty] private string _wad1 = "";
    [ObservableProperty] private bool _wad1Active;
    [ObservableProperty] private string _wad2 = "";
    [ObservableProperty] private bool _wad2Active;
    [ObservableProperty] private string _xPos = "";
    [ObservableProperty] private string _height = "";
    [ObservableProperty] private string _yPos = "";
    [ObservableProperty] private string _camera = "";
    [ObservableProperty] private string _camWad = "";
    [ObservableProperty] private bool _forcedSubweapon;
    [ObservableProperty] private bool _frozen;

    // --- Stats ---
    [ObservableProperty] private string _timesSaved = "";

    // --- Commands ---
    [RelayCommand]
    private async Task BrowseAsync()
    {
        if (_topLevel is null) return;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select MASTER.BIN",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("GoW Save File") { Patterns = new[] { "MASTER.BIN" } } }
        });
        if (files.Count > 0)
            Folder = Path.GetDirectoryName(files[0].Path.LocalPath)!;
    }

    [RelayCommand]
    private void Open()
    {
        if (!Directory.Exists(Folder)) return;
        if (_save.Load(Folder, -1))
        {
            IsLoaded = true; IsMaster = true; SelectedSlot = -1;
            LoadMasterValues();
        }
    }

    [RelayCommand]
    private void SelectSlot(int slot)
    {
        if (_save.Load(Folder, slot))
        {
            IsLoaded = true; IsMaster = false; SelectedSlot = slot;
            LoadSlotValues();
        }
    }

    [RelayCommand]
    private void SelectMaster()
    {
        if (_save.Load(Folder, -1))
        {
            IsLoaded = true; IsMaster = true; SelectedSlot = -1;
            LoadMasterValues();
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (_save.IsMaster) WriteMasterValues();
        else WriteSlotValues();
        if (_save.Save()) Checksum = _save.Checksum;
    }

    [RelayCommand]
    private void Restart()
    {
        var asm = Assembly.GetExecutingAssembly();
        using var ms = asm.GetManifestResourceStream("GodOfWarCSharpSE.Data.G2BlankSave.BIN");
        byte[] blank = [];
        if (ms is not null) { var buf = new byte[(int)ms.Length]; ms.ReadExactly(buf); blank = buf; }
        _save.NewGame(blank);
        LoadSlotValues();
    }

    [RelayCommand]
    private void MakeMoviesSkippable()
    {
        if (!_save.IsMaster)
        {
            _save.MakeMoviesSkippable(_movieNames);
            MoviesSeen = _save.MoviesSeen.ToString();
        }
    }

    [ObservableProperty] private string _checksum = "";

    // --- Load ---
    private void LoadMasterValues()
    {
        CyclopsEyesMaster = _save.CyclopsEyesMaster;
        BeatEMaster = _save.BeatEMaster;
        BeatNMaster = _save.BeatNMaster;
        BeatHMaster = _save.BeatHMaster;
        BeatVHMaster = _save.BeatVHMaster;
        MortalRankMaster = _save.MortalRankMaster;
        SpartanRankMaster = _save.SpartanRankMaster;
        GodRankMaster = _save.GodRankMaster;
        TitanRankMaster = _save.TitanRankMaster;
    }

    private void LoadSlotValues()
    {
        Costume = _save.Costume;
        Wad1 = _save.Wad1; Wad1Active = _save.Wad1Active;
        Wad2 = _save.Wad2; Wad2Active = _save.Wad2Active;
        XPos = _save.XPos.ToString("0.##");
        Height = _save.Height.ToString("0.##");
        YPos = _save.YPos.ToString("0.##");
        Swimming = _save.Swimming;
        BoOUnl = _save.BoOUnl; UnkUnl1_02 = _save.UnkUnl1_02;
        UrnGaiaAcq = _save.UrnGaiaAcq; UrnGorgonAcq = _save.UrnGorgonAcq;
        UrnOlympusAcq = _save.UrnOlympusAcq; UrnPrometheusAcq = _save.UrnPrometheusAcq;
        UrnFatesAcq = _save.UrnFatesAcq; UrnPoseidonAcq = _save.UrnPoseidonAcq;
        UnkUnl2_01 = _save.UnkUnl2_01; RageAcq = _save.RageAcq;
        AmuletAcq = _save.AmuletAcq; UnkUnl2_08 = _save.UnkUnl2_08;
        UnkUnl2_10 = _save.UnkUnl2_10; SoDUnl = _save.SoDUnl;
        BhUnl = _save.BHUnl; EhAcq = _save.EHAcq;
        UnkUnl3_80 = _save.UnkUnl3_80; WingsAcq = _save.WingsAcq;
        AqAcq = _save.AQAcq; FleeceAcq = _save.FleeceAcq;
        UnkUnl3_08 = _save.UnkUnl3_08; TbAcq = _save.TBAcq;
        CrAcq = _save.CRAcq; PrAcq = _save.PRAcq;
        Health = _save.Health.ToString("0.##");
        Magic = _save.Magic.ToString("0.##");
        Item = _save.Item.ToString("0.##");
        Rage = _save.Rage.ToString("0.##");
        MagicRegen = _save.MagicRegen.ToString("0.##");
        RedOrbs = _save.RedOrbs.ToString();
        HealthExt = _save.HealthExt.ToString();
        MagicExt = _save.MagicExt.ToString();
        ItemExt = _save.ItemExt.ToString();
        BoAlvl = _save.BoALvl.ToString();
        Aqlvl = _save.AQLvl.ToString();
        Crlvl = _save.CRLvl.ToString();
        Prlvl = _save.PRLvl.ToString();
        Tblvl = _save.TBLvl.ToString();
        SoDlvl = _save.SoDLvl.ToString();
        Bhlvl = _save.BHLvl.ToString();
        Ehlvl = _save.EHLvl.ToString();
        BoOlvl = _save.BoOLvl.ToString();
        SelectedWeapon = _save.SelectedWeapon;
        ForcedSubweapon = _save.ForcedSubweapon;
        GorgonEyes = _save.GorgonEyes.ToString();
        PhoenixFeathers = _save.PhoenixFeathers.ToString();
        CyclopsEyes = _save.CyclopsEyes.ToString();
        Camera = _save.Camera;
        CamWad = _save.CamWad;
        MoviesSeen = _save.MoviesSeen.ToString();
        SecondsPlayed = _save.SecondsPlayed.ToString();
        Frozen = _save.Frozen;
        Difficulty = _save.Difficulty;
        InfiniteHealth = _save.InfiniteHealth;
        UrnOlympusActive = _save.UrnOlympusActive;
        UrnPrometheusActive = _save.UrnPrometheusActive;
        UrnGorgonsActive = _save.UrnGorgonsActive;
        UrnPoseidonActive = _save.UrnPoseidonActive;
        UrnGaiaActive = _save.UrnGaiaActive;
        UrnFatesActive = _save.UrnFatesActive;
        Bonus = _save.Bonus;
        TimesSaved = _save.TimesSaved.ToString();
        Checksum = _save.Checksum;
    }

    // --- Write ---
    private void WriteMasterValues()
    {
        _save.CyclopsEyesMaster = CyclopsEyesMaster;
        _save.BeatEMaster = BeatEMaster;
        _save.BeatNMaster = BeatNMaster;
        _save.BeatHMaster = BeatHMaster;
        _save.BeatVHMaster = BeatVHMaster;
        _save.MortalRankMaster = MortalRankMaster;
        _save.SpartanRankMaster = SpartanRankMaster;
        _save.GodRankMaster = GodRankMaster;
        _save.TitanRankMaster = TitanRankMaster;
    }

    private void WriteSlotValues()
    {
        _save.Costume = (byte)Costume;
        _save.Wad1 = Wad1; _save.Wad1Active = Wad1Active;
        _save.Wad2 = Wad2; _save.Wad2Active = Wad2Active;
        if (float.TryParse(XPos, out var x)) _save.XPos = x;
        if (float.TryParse(Height, out var h)) _save.Height = h;
        if (float.TryParse(YPos, out var y)) _save.YPos = y;
        _save.Swimming = Swimming;
        _save.BoOUnl = BoOUnl; _save.UnkUnl1_02 = UnkUnl1_02;
        _save.UrnGaiaAcq = UrnGaiaAcq; _save.UrnGorgonAcq = UrnGorgonAcq;
        _save.UrnOlympusAcq = UrnOlympusAcq; _save.UrnPrometheusAcq = UrnPrometheusAcq;
        _save.UrnFatesAcq = UrnFatesAcq; _save.UrnPoseidonAcq = UrnPoseidonAcq;
        _save.UnkUnl2_01 = UnkUnl2_01; _save.RageAcq = RageAcq;
        _save.AmuletAcq = AmuletAcq; _save.UnkUnl2_08 = UnkUnl2_08;
        _save.UnkUnl2_10 = UnkUnl2_10; _save.SoDUnl = SoDUnl;
        _save.BHUnl = BhUnl; _save.EHAcq = EhAcq;
        _save.UnkUnl3_80 = UnkUnl3_80; _save.WingsAcq = WingsAcq;
        _save.AQAcq = AqAcq; _save.FleeceAcq = FleeceAcq;
        _save.UnkUnl3_08 = UnkUnl3_08; _save.TBAcq = TbAcq;
        _save.CRAcq = CrAcq; _save.PRAcq = PrAcq;
        if (float.TryParse(Health, out var hp)) _save.Health = hp;
        if (float.TryParse(Magic, out var mg)) _save.Magic = mg;
        if (float.TryParse(Item, out var it)) _save.Item = it;
        if (float.TryParse(Rage, out var rg)) _save.Rage = rg;
        if (float.TryParse(MagicRegen, out var mr)) _save.MagicRegen = mr;
        if (int.TryParse(RedOrbs, out var ro)) _save.RedOrbs = ro;
        if (byte.TryParse(HealthExt, out var he)) _save.HealthExt = he;
        if (byte.TryParse(MagicExt, out var me)) _save.MagicExt = me;
        if (byte.TryParse(ItemExt, out var ie)) _save.ItemExt = ie;
        if (byte.TryParse(BoAlvl, out var ba)) _save.BoALvl = ba;
        if (byte.TryParse(Aqlvl, out var aq)) _save.AQLvl = aq;
        if (byte.TryParse(Crlvl, out var cr)) _save.CRLvl = cr;
        if (byte.TryParse(Prlvl, out var pr)) _save.PRLvl = pr;
        if (byte.TryParse(Tblvl, out var tb)) _save.TBLvl = tb;
        if (byte.TryParse(SoDlvl, out var sd)) _save.SoDLvl = sd;
        if (byte.TryParse(Bhlvl, out var bh)) _save.BHLvl = bh;
        if (byte.TryParse(Ehlvl, out var eh)) _save.EHLvl = eh;
        if (byte.TryParse(BoOlvl, out var bo)) _save.BoOLvl = bo;
        _save.SelectedWeapon = (byte)SelectedWeapon;
        _save.ForcedSubweapon = ForcedSubweapon;
        if (byte.TryParse(GorgonEyes, out var ge)) _save.GorgonEyes = ge;
        if (byte.TryParse(PhoenixFeathers, out var pf)) _save.PhoenixFeathers = pf;
        if (byte.TryParse(CyclopsEyes, out var ce)) _save.CyclopsEyes = ce;
        _save.Camera = Camera;
        _save.CamWad = CamWad;
        if (int.TryParse(SecondsPlayed, out var sp)) _save.SecondsPlayed = sp;
        _save.Frozen = Frozen;
        _save.Difficulty = (byte)Difficulty;
        _save.InfiniteHealth = InfiniteHealth;
        _save.UrnOlympusActive = UrnOlympusActive;
        _save.UrnPrometheusActive = UrnPrometheusActive;
        _save.UrnGorgonsActive = UrnGorgonsActive;
        _save.UrnPoseidonActive = UrnPoseidonActive;
        _save.UrnGaiaActive = UrnGaiaActive;
        _save.UrnFatesActive = UrnFatesActive;
        _save.Bonus = Bonus;
        if (byte.TryParse(TimesSaved, out var ts)) _save.TimesSaved = ts;
    }

    // --- Interlocks ---
    partial void OnUrnOlympusActiveChanged(bool value) { if (value) InfiniteMagic = true; else InfiniteMagic = false; }
    partial void OnInfiniteMagicChanged(bool value) { if (value) UrnOlympusActive = true; else UrnOlympusActive = false; }
    partial void OnUrnPrometheusActiveChanged(bool value) { if (value) InfiniteRage = true; else InfiniteRage = false; }
    partial void OnInfiniteRageChanged(bool value) { if (value) UrnPrometheusActive = true; else UrnPrometheusActive = false; }
}

