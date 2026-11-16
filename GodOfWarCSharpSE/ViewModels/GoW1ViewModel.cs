using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GodOfWarCSharpSE.Models;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using System.Reflection;

namespace GodOfWarCSharpSE.ViewModels;

public partial class GoW1ViewModel : ObservableObject
{
    private readonly GoW1Save _save = new();
    private string[] _movieNames = [];
    private TopLevel? _topLevel;

    public GoW1ViewModel()
    {
        LoadEmbeddedData();
    }

    public void SetTopLevel(TopLevel topLevel) => _topLevel = topLevel;

    private void LoadEmbeddedData()
    {
        var asm = Assembly.GetExecutingAssembly();
        using var ms = asm.GetManifestResourceStream("GodOfWarCSharpSE.Data.Gow1Movies.txt");
        if (ms is not null)
        {
            using var sr = new StreamReader(ms);
            _movieNames = sr.ReadToEnd().Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }

    // --- Folder / Slot state ---
    [ObservableProperty] private string _folder = "";
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSlotLoaded))]
    private bool _isLoaded;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSlotLoaded))]
    private bool _isMaster;

    /// <summary>True when a DATA0x slot (not MASTER.BIN) is loaded: drives the editor tabs.</summary>
    public bool IsSlotLoaded => IsLoaded && !IsMaster;

    [ObservableProperty] private string _statusText = "";
    [ObservableProperty] private int _selectedSlot = -1;

    // --- Master flags ---
    [ObservableProperty] private bool _beatGame;
    [ObservableProperty] private bool _beatVH;
    [ObservableProperty] private bool _beatCotG;
    [ObservableProperty] private bool _statues;

    // --- Character tab ---
    [ObservableProperty] private string _health = "";
    [ObservableProperty] private string _magic = "";
    [ObservableProperty] private string _rage = "";
    [ObservableProperty] private string _magicRegen = "";
    [ObservableProperty] private string _redOrbs = "";
    [ObservableProperty] private string _healthExt = "";
    [ObservableProperty] private string _magicExt = "";
    [ObservableProperty] private string _prLevel = "";
    [ObservableProperty] private string _mgLevel = "";
    [ObservableProperty] private string _zfLevel = "";
    [ObservableProperty] private string _aohLevel = "";
    [ObservableProperty] private string _boaLevel = "";
    [ObservableProperty] private string _bocLevel = "";
    [ObservableProperty] private bool _prSelected;
    [ObservableProperty] private bool _mgSelected;
    [ObservableProperty] private bool _aohSelected;
    [ObservableProperty] private bool _zfSelected;
    [ObservableProperty] private string _gorgonEyes = "";
    [ObservableProperty] private string _phoenixFeathers = "";
    [ObservableProperty] private string _museKeys = "";
    [ObservableProperty] private bool _poseidonsTrident;
    [ObservableProperty] private bool _prUnlocked;
    [ObservableProperty] private bool _mgUnlocked;
    [ObservableProperty] private bool _zfUnlocked;
    [ObservableProperty] private bool _aohUnlocked;
    [ObservableProperty] private bool _boaUnlocked;
    [ObservableProperty] private bool _athenasBlades;
    [ObservableProperty] private int _costume;
    [ObservableProperty] private int _difficulty;
    [ObservableProperty] private string _secondsPlayed = "";
    [ObservableProperty] private bool _swimming;
    [ObservableProperty] private string _moviesSeen = "";
    [ObservableProperty] private string _checksum = "";

    // --- Warp tab ---
    [ObservableProperty] private string _wad1 = "";
    [ObservableProperty] private bool _wad1Active;
    [ObservableProperty] private string _wad2 = "";
    [ObservableProperty] private bool _wad2Active;
    [ObservableProperty] private string _xPos = "";
    [ObservableProperty] private string _yPos = "";
    [ObservableProperty] private string _zPos = "";
    [ObservableProperty] private string _camera = "";
    [ObservableProperty] private string _camWad = "";
    [ObservableProperty] private bool _worldIdle;
    [ObservableProperty] private string _selectedCategory = "";
    [ObservableProperty] private string _selectedCheckpoint = "";
    [ObservableProperty] private IReadOnlyList<string> _checkpoints = [];

    public IReadOnlyList<string> Categories => GoW1Checkpoints.Categories;

    partial void OnSelectedCategoryChanged(string value)
    {
        SelectedCheckpoint = "";
        Checkpoints = string.IsNullOrEmpty(value)
            ? []
            : GoW1Checkpoints.All.Where(c => c.Category == value).Select(c => c.Name).ToList();
    }

    partial void OnSelectedCheckpointChanged(string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        var cp = GoW1Checkpoints.All.FirstOrDefault(c => c.Name == value);
        if (cp is null) return;
        Wad1 = cp.Wad1; Wad1Active = cp.Wad1Active;
        Wad2 = cp.Wad2; Wad2Active = cp.Wad2Active;
        Camera = cp.CurrCam; CamWad = cp.CamWad;
        XPos = cp.XPos.ToString("0.##");
        YPos = cp.YPos.ToString("0.##");
        ZPos = cp.ZPos.ToString("0.##");
        Swimming = (cp.Stance & 0x200) != 0;
        WorldIdle = cp.WorldIdle;
    }

    // --- Globals tab ---
    [ObservableProperty] private bool _vibration;
    [ObservableProperty] private bool _widescreen;
    [ObservableProperty] private bool _deFlicker;
    [ObservableProperty] private bool _puft;
    [ObservableProperty] private bool _endGame;
    [ObservableProperty] private string _musicVol = "";
    [ObservableProperty] private string _soundVol = "";
    [ObservableProperty] private string _soundMode = "";

    // --- Commands ---
    [RelayCommand]
    private async Task BrowseAsync()
    {
        if (_topLevel is null) return;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select MASTER.BIN",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("GoW Save File") { Patterns = new[] { "MASTER.BIN" } },
                new FilePickerFileType("All Files") { Patterns = new[] { "*" } }
            }
        });
        if (files.Count > 0)
        {
            var path = files[0].Path.LocalPath;
            Folder = Path.GetDirectoryName(path)!;
        }
    }

    [RelayCommand]
    private void Open()
    {
        if (!Directory.Exists(Folder)) { StatusText = $"Folder not found: {Folder}"; return; }
        if (_save.Load(Folder, -1))
        {
            IsLoaded = true;
            IsMaster = true;
            SelectedSlot = -1;
            LoadMasterValues();
            StatusText = "Loaded MASTER.BIN. Pick a slot above to edit it.";
        }
        else
        {
            StatusText = _save.LastError;
        }
    }

    // CommandParameter="0" in XAML arrives as a string, not an int. With an
    // int parameter the toolkit rejected it, so the slot buttons never worked.
    [RelayCommand]
    private void SelectSlot(object? parameter)
    {
        if (!int.TryParse(parameter?.ToString(), out int slot)) return;
        if (_save.Load(Folder, slot))
        {
            IsLoaded = true;
            IsMaster = false;
            SelectedSlot = slot;
            LoadSlotValues();
            StatusText = $"Loaded slot {slot + 1} ({_save.DataPath})";
        }
        else
        {
            StatusText = _save.LastError;
        }
    }

    [RelayCommand]
    private void SelectMaster()
    {
        if (_save.Load(Folder, -1))
        {
            IsLoaded = true;
            IsMaster = true;
            SelectedSlot = -1;
            LoadMasterValues();
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (!IsLoaded) { StatusText = "Open a save first."; return; }

        if (_save.IsMaster) WriteMasterValues();
        else WriteSlotValues();

        if (_save.Save())
        {
            Checksum = _save.Checksum;
            StatusText = $"Saved {_save.Filename}.";
        }
    }

    [RelayCommand]
    private void Restart()
    {
        if (!IsSlotLoaded) { StatusText = "Select a slot first."; return; }
        var asm = Assembly.GetExecutingAssembly();
        using var ms = asm.GetManifestResourceStream("GodOfWarCSharpSE.Data.G1BlankSave.BIN");
        byte[] blank = [];
        if (ms is not null)
        {
            var buf = new byte[(int)ms.Length];
            ms.ReadExactly(buf);
            blank = buf;
        }
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

    // --- Load helpers ---
    private void LoadMasterValues()
    {
        BeatGame = _save.BeatGame;
        BeatVH = _save.BeatVH;
        BeatCotG = _save.BeatCotG;
        Statues = _save.Statues;
    }

    private void LoadSlotValues()
    {
        Wad1 = _save.Wad1; Wad1Active = _save.Wad1Active;
        Wad2 = _save.Wad2; Wad2Active = _save.Wad2Active;
        XPos = _save.XPos.ToString("0.##");
        YPos = _save.YPos.ToString("0.##");
        ZPos = _save.ZPos.ToString("0.##");
        Swimming = _save.Swimming;
        SecondsPlayed = _save.SecondsPlayed.ToString("0.##");
        Health = _save.Health.ToString("0.##");
        Magic = _save.Magic.ToString("0.##");
        Rage = _save.Rage.ToString("0.##");
        MagicRegen = _save.MagicRegen.ToString("0.##");
        RedOrbs = _save.RedOrbs.ToString();
        HealthExt = _save.HealthExt.ToString();
        MagicExt = _save.MagicExt.ToString();
        PrLevel = _save.PRLevel.ToString();
        MgLevel = _save.MGLevel.ToString();
        ZfLevel = _save.ZFLevel.ToString();
        AohLevel = _save.AoHLevel.ToString();
        BoaLevel = _save.BoALevel.ToString();
        BocLevel = _save.BoCLevel.ToString();
        PrSelected = _save.PRSelected;
        MgSelected = _save.MGSelected;
        AohSelected = _save.AoHSelected;
        ZfSelected = _save.ZFSelected;
        GorgonEyes = _save.GorgonEyes.ToString();
        PhoenixFeathers = _save.PhoenixFeathers.ToString();
        MuseKeys = _save.MuseKeys.ToString();
        PoseidonsTrident = _save.PoseidonsTrident;
        PrUnlocked = _save.PRUnlocked;
        MgUnlocked = _save.MGUnlocked;
        ZfUnlocked = _save.ZFUnlocked;
        AohUnlocked = _save.AoHUnlocked;
        BoaUnlocked = _save.BoAUnlocked;
        AthenasBlades = _save.BoCLevel == 6;
        Costume = _save.Costume;
        Difficulty = _save.Difficulty;
        WorldIdle = _save.WorldIdle;
        MoviesSeen = _save.MoviesSeen.ToString();
        Vibration = _save.Vibration;
        Widescreen = _save.Widescreen;
        DeFlicker = _save.DeFlicker;
        Puft = _save.PUFT;
        EndGame = _save.EndGame;
        MusicVol = _save.MusicVol.ToString();
        SoundVol = _save.SoundVol.ToString();
        SoundMode = _save.SoundMode.ToString();
        Checksum = _save.Checksum;
    }

    // --- Write helpers ---
    private void WriteMasterValues()
    {
        _save.BeatGame = BeatGame;
        _save.BeatVH = BeatVH;
        _save.BeatCotG = BeatCotG;
        _save.Statues = Statues;
    }

    private void WriteSlotValues()
    {
        _save.Wad1 = Wad1; _save.Wad1Active = Wad1Active;
        _save.Wad2 = Wad2; _save.Wad2Active = Wad2Active;
        if (float.TryParse(XPos, out var x)) _save.XPos = x;
        if (float.TryParse(YPos, out var y)) _save.YPos = y;
        if (float.TryParse(ZPos, out var z)) _save.ZPos = z;
        _save.Swimming = Swimming;
        if (float.TryParse(SecondsPlayed, out var sp)) _save.SecondsPlayed = sp;
        if (float.TryParse(Health, out var h)) _save.Health = h;
        if (float.TryParse(Magic, out var m)) _save.Magic = m;
        if (float.TryParse(Rage, out var r)) _save.Rage = r;
        if (float.TryParse(MagicRegen, out var mr)) _save.MagicRegen = mr;
        if (ushort.TryParse(RedOrbs, out var ro)) _save.RedOrbs = ro;
        if (byte.TryParse(HealthExt, out var he)) _save.HealthExt = he;
        if (byte.TryParse(MagicExt, out var me)) _save.MagicExt = me;
        if (byte.TryParse(PrLevel, out var pr)) _save.PRLevel = pr;
        if (byte.TryParse(MgLevel, out var mg)) _save.MGLevel = mg;
        if (byte.TryParse(ZfLevel, out var zf)) _save.ZFLevel = zf;
        if (byte.TryParse(AohLevel, out var aoh)) _save.AoHLevel = aoh;
        if (byte.TryParse(BoaLevel, out var boa)) _save.BoALevel = boa;
        if (byte.TryParse(BocLevel, out var boc)) _save.BoCLevel = boc;
        if (PrSelected) _save.PRSelected = true;
        if (MgSelected) _save.MGSelected = true;
        if (AohSelected) _save.AoHSelected = true;
        if (ZfSelected) _save.ZFSelected = true;
        if (byte.TryParse(GorgonEyes, out var ge)) _save.GorgonEyes = ge;
        if (byte.TryParse(PhoenixFeathers, out var pf)) _save.PhoenixFeathers = pf;
        if (byte.TryParse(MuseKeys, out var mk)) _save.MuseKeys = mk;
        _save.PoseidonsTrident = PoseidonsTrident;
        _save.PRUnlocked = PrUnlocked;
        _save.MGUnlocked = MgUnlocked;
        _save.ZFUnlocked = ZfUnlocked;
        _save.AoHUnlocked = AohUnlocked;
        _save.BoAUnlocked = BoaUnlocked;
        _save.Costume = (byte)Costume;
        _save.Difficulty = (byte)Difficulty;
        _save.WorldIdle = WorldIdle;
        _save.Vibration = Vibration;
        _save.Widescreen = Widescreen;
        _save.DeFlicker = DeFlicker;
        _save.PUFT = Puft;
        _save.EndGame = EndGame;
        if (byte.TryParse(MusicVol, out var mv)) _save.MusicVol = mv;
        if (byte.TryParse(SoundVol, out var sv)) _save.SoundVol = sv;
        if (byte.TryParse(SoundMode, out var sm)) _save.SoundMode = sm;
    }

    // --- Mutual exclusion for selected weapon ---
    partial void OnPrSelectedChanged(bool value) { if (value) { MgSelected = false; AohSelected = false; ZfSelected = false; } }
    partial void OnMgSelectedChanged(bool value) { if (value) { PrSelected = false; AohSelected = false; ZfSelected = false; } }
    partial void OnAohSelectedChanged(bool value) { if (value) { PrSelected = false; MgSelected = false; ZfSelected = false; } }
    partial void OnZfSelectedChanged(bool value) { if (value) { PrSelected = false; MgSelected = false; AohSelected = false; } }

    // --- BoC / Athena's Blades interlock ---
    partial void OnAthenasBladesChanged(bool value)
    {
        if (value) BocLevel = "6";
        else BocLevel = "1";
    }
    partial void OnBocLevelChanged(string value)
    {
        AthenasBlades = value == "6";
    }
}