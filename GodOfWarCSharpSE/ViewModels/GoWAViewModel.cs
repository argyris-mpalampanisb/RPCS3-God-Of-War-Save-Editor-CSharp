using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia.Platform.Storage;
using GodOfWarCSharpSE.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GodOfWarCSharpSE.ViewModels;

// Ported from ReactiveUI to CommunityToolkit.Mvvm (same as the GoW1-3 editors).
// Avalonia.ReactiveUI 11.x is not compatible with Avalonia 12 and crashed on open.
public class GoWAViewModel : ObservableObject
{
    private readonly GoWASave _save = new();

    // --- Navigation ---
    private string _folder = "";
    private bool _isLoaded;
    private bool _isModified;
    private int _selectedChannel = 1;
    private bool _showBrowse = true;
    private bool _showOpen = true;
    private bool _showData;

    // --- Character ---
    private string _health = "0";
    private string _healthRegen = "0";
    private string _magic = "0";
    private string _magicRegen = "0";
    private string _rage = "0";
    private string _rageRegen = "0";
    private string _redOrbs = "0";
    private string _xPos = "0";
    private string _yPos = "0";
    private string _zPos = "0";
    private bool _swimming;
    private bool _frozen;
    private string _gorgonEyes = "0";
    private string _phoenixFeathers = "0";
    private string _subwepAmmo = "0";
    private int _difficulty;
    private bool _newGamePlus;

    // --- Weapons ---
    private string _bladesLvl = "1";  private string _elecLvl = "1";
    private string _fireLvl = "1";    private string _iceLvl = "1";
    private string _soulLvl = "1";    private string _amuletLvl = "1";
    private string _oathLvl = "1";    private string _eyesLvl = "1";

    private string _bladesOrbs = "0"; private string _elecOrbs = "0";
    private string _fireOrbs = "0";   private string _iceOrbs = "0";
    private string _soulOrbs = "0";   private string _amuletOrbs = "0";
    private string _oathOrbs = "0";   private string _eyesOrbs = "0";

    private bool _bladesUnl; private bool _elecUnl;
    private bool _fireUnl;   private bool _iceUnl;
    private bool _soulUnl;   private bool _amuletUnl;
    private bool _oathUnl;   private bool _eyesUnl;

    private bool _bladesSel; private bool _elecSel;
    private bool _fireSel;   private bool _iceSel;
    private bool _soulSel;

    // --- Sub-weapon ---
    private int _subWeaponIndex;
    private bool _subwepGlitch;
    private bool _subwepGlitchEnabled;

    // --- Costume ---
    private int _torsoIndex;
    private int _pantsIndex;

    // --- Static data ---
   public string[] SubWeaponNames { get; } =   
        ["None", "Bare Fists", "Club", "Club - Level 2", "Club - Level 3",
         "Shield", "Shield - Level 2", "Shield - Level 3",
         "Sling", "Sling - Level 2", "Sling - Level 3",
         "Spear", "Spear - Level 2", "Spear - Level 3",
         "Sword", "Sword - Level 2", "Sword - Level 3"];

    public string[] TorsoNames { get; } =   
        ["None", "White", "Dirty", "Dirty (2)", "White (2)",
         "Ares", "Fury", "Hades", "Kraken", "Skorpius", "Zeus", "Olive-Skinned"];

    public string[] PantsNames { get; } =   
        ["None", "Pristine", "Tattered", "Tattered (2)", "Pristine (2)",
         "Ares", "Fury", "Hades", "Kraken", "Skorpius", "Zeus", "Olive-Skinned"];

    public static readonly string[] InventoryNames =
    [
        "00 - Explosive", "01 - SpartanBlades (causes glow)", "02 - SpartanRageBladesAres (crash?)",
        "03 - SpartanBlades_Gaunt (causes glow)", "04 - MagicElectric", "05 - MagicFire",
        "06 - MagicIce", "07 - MagicSoul", "08 - RageModeElectric", "09 - RageModeFireGameStart",
        "0A - RageModeFire", "0B - RageModeIce", "0C - RageModeSoul", "0D - RageModeMultiplayer",
        "0E - Unknown", "0F - RageModeFire_Active", "10 - RageModeIce_Active", "11 - RageModeElectric_Active",
        "12 - RageModeSoul_Active", "13 - RageModeFire_Threshold1", "14 - RageModeIce_Threshold1",
        "15 - RageModeElectric_Threshold1", "16 - RageModeSoul_Threshold1", "17 - RageModeFire_Threshold2",
        "18 - RageModeIce_Threshold2", "19 - RageModeElectric_Threshold2", "1A - RageModeSoul_Threshold2",
        "1B - Bare Fists", "1C - SubClub", "1D - SubClub10", "1E - SubClub20",
        "1F - SubClubBase", "20 - SubShield", "21 - SubShield10", "22 - SubShield20",
        "23 - SubShieldBase", "24 - SubSling", "25 - SubSling10", "26 - SubSling20",
        "27 - SubSlingBase", "28 - SubSpear", "29 - SubSpear10", "2A - SubSpear20",
        "2B - SubSpearBase", "2C - SubSword", "2D - SubSword10", "2E - SubSword20",
        "2F - SubSwordBase", "30 - SpartanMPVIP", "31 - SpartanMPFlag", "32 - SpartanMPFlagRed",
        "33 - SpartanMPFlagSpecialSquare", "34 - SpartanMPFlagSpecialTriangle", "35 - SpartanMPTitanKill",
        "36 - GodModePowerUp", "37 - ZeusMagic (Load Forever)", "38 - HermesBoots",
        "39 - HermesBootsPowerUp", "3A - HeliosHead", "3B - MedusaHead", "3C - KratosBodyWhite",
        "3D - KratosBodyDirty", "3E - KratosWaistPristine", "3F - KratosWaistTattered",
        "40 - KratosBodyDirty", "41 - KratosWaistTattered", "42 - KratosBodyWhite",
        "43 - KratosWaistWhite", "44 - KratosBodyAres", "45 - KratosWaistAres",
        "46 - KratosBodyFury", "47 - KratosWaistFury", "48 - KratosBodyHades",
        "49 - KratosWaistHades", "4A - KratosBodyKraken", "4B - KratosWaistKraken",
        "4C - KratosBodySkorpius", "4D - KratosWaistSkorpius", "4E - KratosBodyZeus",
        "4F - KratosWaistZeus", "50 - KratosBodyOlive", "51 - KratosWaistOlive",
        "52 - SpartanSwordQ", "53 - SpartanSwordR", "54 - SpartanSwordS", "55 - SpartanSwordT",
        "56 - SpartanSwordA", "57 - SpartanSwordC", "58 - SpartanSwordH", "59 - SpartanSwordD",
        "5A - SpartanSwordE", "5B - SpartanSwordF", "5C - SpartanSwordB", "5D - SpartanSwordG",
        "5E - SpartanSwordI", "5F - SpartanSwordJ", "60 - SpartanSwordK", "61 - SpartanSwordL",
        "62 - SpartanSwordM", "63 - SpartanSwordN", "64 - SpartanSwordO", "65 - SpartanSwordP",
        "66 - SpartanHammerQ", "67 - SpartanHammerR", "68 - SpartanHammerS", "69 - SpartanHammerB",
        "6A - SpartanHammerC", "6B - SpartanHammerD", "6C - SpartanHammerE", "6D - SpartanHammerA",
        "6E - SpartanHammerF", "6F - SpartanHammerG", "70 - SpartanHammerH", "71 - SpartanHammerO",
        "72 - SpartanHammerJ", "73 - SpartanHammerK", "74 - SpartanHammerL", "75 - SpartanHammerM",
        "76 - SpartanHammerN", "77 - SpartanHammerP", "78 - SpartanHammerI", "79 - SpartanSpearQ",
        "7A - SpartanSpearR", "7B - SpartanSpearA", "7C - SpartanSpearB", "7D - SpartanSpearC",
        "7E - SpartanSpearD", "7F - SpartanSpearE", "80 - SpartanSpearF", "81 - SpartanSpearG",
        "82 - SpartanSpearH", "83 - SpartanSpearI", "84 - SpartanSpearJ", "85 - SpartanSpearK",
        "86 - SpartanSpearL", "87 - SpartanSpearM", "88 - SpartanSpearP", "89 - SpartanSpearO",
        "8A - SpartanSpearN", "8B - SpartanGauntletA", "8C - HermesShoes", "8D - GoldenFleece",
        "8E - Poseidon's Trident", "8F - Health", "90 - Magic", "91 - Lifecycle",
        "92 - LifeCycleGlow", "93 - ShadowDouble", "94 - XeShadowDestroy", "95 - Eyes", "96 - ZeusBolt"
    ];

    // --- Inventory observable collection ---
    public ObservableCollection<InventoryItem> InventoryItems { get; }

    // --- Commands ---
    public IRelayCommand BrowseCommand { get; }
    public IRelayCommand OpenCommand { get; }
    public IRelayCommand SaveCommand { get; }
    public IRelayCommand<object?> ChannelCommand { get; }

    private string _statusText = "Browse to the folder containing DAT-1, then click Open.";
    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    // --- Observable properties ---
    public string Folder
    {
        get => _folder;
        set => SetProperty(ref _folder, value);
    }

    public bool IsLoaded
    {
        get => _isLoaded;
        private set => SetProperty(ref _isLoaded, value);
    }

    public bool IsModified
    {
        get => _isModified;
        private set => SetProperty(ref _isModified, value);
    }

    public int SelectedChannel
    {
        get => _selectedChannel;
        private set => SetProperty(ref _selectedChannel, value);
    }

    public bool ShowBrowse
    {
        get => _showBrowse;
        private set => SetProperty(ref _showBrowse, value);
    }

    public bool ShowOpen
    {
        get => _showOpen;
        private set => SetProperty(ref _showOpen, value);
    }

    public bool ShowData
    {
        get => _showData;
        private set => SetProperty(ref _showData, value);
    }

    // Character
    public string Health       { get => _health;       set { SetProperty(ref _health, value); IsModified = true; } }
    public string HealthRegen  { get => _healthRegen;  set { SetProperty(ref _healthRegen, value); IsModified = true; } }
    public string Magic         { get => _magic;        set { SetProperty(ref _magic, value); IsModified = true; } }
    public string MagicRegen   { get => _magicRegen;   set { SetProperty(ref _magicRegen, value); IsModified = true; } }
    public string Rage          { get => _rage;         set { SetProperty(ref _rage, value); IsModified = true; } }
    public string RageRegen    { get => _rageRegen;    set { SetProperty(ref _rageRegen, value); IsModified = true; } }
    public string RedOrbs      { get => _redOrbs;      set { SetProperty(ref _redOrbs, value); IsModified = true; } }
    public string XPos         { get => _xPos;         set { SetProperty(ref _xPos, value); IsModified = true; } }
    public string YPos         { get => _yPos;         set { SetProperty(ref _yPos, value); IsModified = true; } }
    public string ZPos         { get => _zPos;         set { SetProperty(ref _zPos, value); IsModified = true; } }
    public bool Swimming       { get => _swimming;     set { SetProperty(ref _swimming, value); IsModified = true; } }
    public bool Frozen         { get => _frozen;       set { SetProperty(ref _frozen, value); IsModified = true; } }
    public string GorgonEyes   { get => _gorgonEyes;   set { SetProperty(ref _gorgonEyes, value); IsModified = true; } }
    public string PhoenixFeathers { get => _phoenixFeathers; set { SetProperty(ref _phoenixFeathers, value); IsModified = true; } }
    public string SubwepAmmo   { get => _subwepAmmo;   set { SetProperty(ref _subwepAmmo, value); IsModified = true; } }
    public int Difficulty      { get => _difficulty;   set { SetProperty(ref _difficulty, value); IsModified = true; } }
    public bool NewGamePlus    { get => _newGamePlus;  set { SetProperty(ref _newGamePlus, value); IsModified = true; } }

    // Weapons
    public string BladesLvl   { get => _bladesLvl;   set { SetProperty(ref _bladesLvl, value); IsModified = true; } }
    public string ElecLvl     { get => _elecLvl;     set { SetProperty(ref _elecLvl, value); IsModified = true; } }
    public string FireLvl     { get => _fireLvl;     set { SetProperty(ref _fireLvl, value); IsModified = true; } }
    public string IceLvl      { get => _iceLvl;      set { SetProperty(ref _iceLvl, value); IsModified = true; } }
    public string SoulLvl     { get => _soulLvl;     set { SetProperty(ref _soulLvl, value); IsModified = true; } }
    public string AmuletLvl   { get => _amuletLvl;   set { SetProperty(ref _amuletLvl, value); IsModified = true; } }
    public string OathLvl     { get => _oathLvl;     set { SetProperty(ref _oathLvl, value); IsModified = true; } }
    public string EyesLvl     { get => _eyesLvl;     set { SetProperty(ref _eyesLvl, value); IsModified = true; } }

    public string BladesOrbs  { get => _bladesOrbs;  set { SetProperty(ref _bladesOrbs, value); IsModified = true; } }
    public string ElecOrbs    { get => _elecOrbs;    set { SetProperty(ref _elecOrbs, value); IsModified = true; } }
    public string FireOrbs    { get => _fireOrbs;    set { SetProperty(ref _fireOrbs, value); IsModified = true; } }
    public string IceOrbs     { get => _iceOrbs;     set { SetProperty(ref _iceOrbs, value); IsModified = true; } }
    public string SoulOrbs    { get => _soulOrbs;    set { SetProperty(ref _soulOrbs, value); IsModified = true; } }
    public string AmuletOrbs  { get => _amuletOrbs;  set { SetProperty(ref _amuletOrbs, value); IsModified = true; } }
    public string OathOrbs    { get => _oathOrbs;    set { SetProperty(ref _oathOrbs, value); IsModified = true; } }
    public string EyesOrbs    { get => _eyesOrbs;    set { SetProperty(ref _eyesOrbs, value); IsModified = true; } }

    public bool BladesUnl { get => _bladesUnl; set { if (SetProperty(ref _bladesUnl, value)) OnWeaponUnlockChanged(0, value); } }
    public bool ElecUnl   { get => _elecUnl;   set { if (SetProperty(ref _elecUnl, value))   OnWeaponUnlockChanged(1, value); } }
    public bool FireUnl   { get => _fireUnl;   set { if (SetProperty(ref _fireUnl, value))   OnWeaponUnlockChanged(2, value); } }
    public bool IceUnl    { get => _iceUnl;    set { if (SetProperty(ref _iceUnl, value))    OnWeaponUnlockChanged(3, value); } }
    public bool SoulUnl   { get => _soulUnl;   set { if (SetProperty(ref _soulUnl, value))   OnWeaponUnlockChanged(4, value); } }
    public bool AmuletUnl { get => _amuletUnl; set { if (SetProperty(ref _amuletUnl, value)) OnWeaponUnlockChanged(5, value); } }
    public bool OathUnl   { get => _oathUnl;   set { if (SetProperty(ref _oathUnl, value))   OnWeaponUnlockChanged(6, value); } }
    public bool EyesUnl   { get => _eyesUnl;   set { if (SetProperty(ref _eyesUnl, value))   OnWeaponUnlockChanged(7, value); } }

    // VB chkEleSel_Click: only the four elements are mutually exclusive; Blades is independent.
    public bool BladesSel { get => _bladesSel; set { SetProperty(ref _bladesSel, value); IsModified = true; } }
    public bool ElecSel   { get => _elecSel;   set { SetProperty(ref _elecSel, value); IsModified = true; if (value) { FireSel = false; IceSel = false; SoulSel = false; } } }
    public bool FireSel   { get => _fireSel;   set { SetProperty(ref _fireSel, value); IsModified = true; if (value) { ElecSel = false; IceSel = false; SoulSel = false; } } }
    public bool IceSel    { get => _iceSel;    set { SetProperty(ref _iceSel, value); IsModified = true; if (value) { ElecSel = false; FireSel = false; SoulSel = false; } } }
    public bool SoulSel   { get => _soulSel;   set { SetProperty(ref _soulSel, value); IsModified = true; if (value) { ElecSel = false; FireSel = false; IceSel = false; } } }

    // Sub-weapon
    public int SubWeaponIndex
    {
        get => _subWeaponIndex;
        set { SetProperty(ref _subWeaponIndex, value); IsModified = true; SyncSubWeaponToInv(); }
    }

    public bool SubwepGlitch
    {
        get => _subwepGlitch;
        set { SetProperty(ref _subwepGlitch, value); IsModified = true; SyncSubwepGlitchToInv(); }
    }

    public bool SubwepGlitchEnabled
    {
        get => _subwepGlitchEnabled;
        private set => SetProperty(ref _subwepGlitchEnabled, value);
    }

    // Costume
    public int TorsoIndex
    {
        get => _torsoIndex;
        set { SetProperty(ref _torsoIndex, value); IsModified = true; SyncCostumeToInv(); }
    }

    public int PantsIndex
    {
        get => _pantsIndex;
        set { SetProperty(ref _pantsIndex, value); IsModified = true; SyncCostumeToInv(); }
    }

    // --- Constructor ---
    public GoWAViewModel()
    {
        InventoryItems = new ObservableCollection<InventoryItem>(
            InventoryNames.Select(n => new InventoryItem(n)));

        BrowseCommand  = new RelayCommand(Browse);
        OpenCommand    = new RelayCommand(Open);
        SaveCommand    = new RelayCommand(Save);
        ChannelCommand = new RelayCommand<object?>(p =>
        {
            // CommandParameter arrives from XAML as a string
            if (int.TryParse(p?.ToString(), out int ch)) SelectChannel(ch);
        });
    }

    // --- Channel selection ---
    private void SelectChannel(int channel)
    {
        if (IsModified)
        {
            // In production, show a confirmation dialog here
            // For now, proceed
        }
        _save.FolderPath = Folder;
        _save.Channel = channel;
        string? err = _save.Load();
        if (err == null) SelectedChannel = channel;
        else _save.Channel = SelectedChannel;   // stay on the slot that is actually loaded
        if (err == null)
        {
            PopulateFromSave();
            IsLoaded = true;
            ShowData = true;
            ShowBrowse = false;
            ShowOpen = false;
            IsModified = false;
            StatusText = $"Loaded slot {channel}.";
        }
        else
        {
            StatusText = err;
        }
    }

    // --- Populate UI from save ---
    private void PopulateFromSave()
    {
        _populating = true;
        try { PopulateCore(); }
        finally { _populating = false; }
        IsModified = false;
    }

    private void PopulateCore()
    {
        Health      = _save.Health.ToString("F4", Culture);
        HealthRegen = _save.HealthRegen.ToString("F4", Culture);
        Magic       = _save.Magic.ToString("F4", Culture);
        MagicRegen  = _save.MagicRegen.ToString("F4", Culture);
        Rage        = _save.Rage.ToString("F4", Culture);
        RageRegen   = _save.RageRegen.ToString("F4", Culture);
        RedOrbs     = _save.RedOrbs.ToString("n0", Culture);
        XPos        = _save.XPos.ToString("F5", Culture);
        YPos        = _save.YPos.ToString("F5", Culture);
        ZPos        = _save.ZPos.ToString("F5", Culture);
        Swimming    = _save.Swimming;
        Frozen      = _save.Frozen;
        GorgonEyes  = _save.GorgonEyes.ToString();
        PhoenixFeathers = _save.PhoenixFeathers.ToString();
        SubwepAmmo  = _save.SubWeaponAmmo.ToString();
        Difficulty  = _save.Difficulty;
        NewGamePlus = _save.NewGamePlus;

        BladesLvl = _save.WeaponLevels[0].ToString();
        ElecLvl   = _save.WeaponLevels[1].ToString();
        FireLvl   = _save.WeaponLevels[2].ToString();
        IceLvl    = _save.WeaponLevels[3].ToString();
        SoulLvl   = _save.WeaponLevels[4].ToString();
        AmuletLvl = _save.WeaponLevels[5].ToString();
        OathLvl   = _save.WeaponLevels[6].ToString();
        EyesLvl   = _save.WeaponLevels[7].ToString();

        BladesOrbs = _save.WeaponOrbs[0].ToString("n0", Culture);
        ElecOrbs   = _save.WeaponOrbs[1].ToString("n0", Culture);
        FireOrbs   = _save.WeaponOrbs[2].ToString("n0", Culture);
        IceOrbs    = _save.WeaponOrbs[3].ToString("n0", Culture);
        SoulOrbs   = _save.WeaponOrbs[4].ToString("n0", Culture);
        AmuletOrbs = _save.WeaponOrbs[5].ToString("n0", Culture);
        OathOrbs   = _save.WeaponOrbs[6].ToString("n0", Culture);
        EyesOrbs   = _save.WeaponOrbs[7].ToString("n0", Culture);

        BladesUnl = _save.WeaponUnlocked[0];
        ElecUnl   = _save.WeaponUnlocked[1];
        FireUnl   = _save.WeaponUnlocked[2];
        IceUnl    = _save.WeaponUnlocked[3];
        SoulUnl   = _save.WeaponUnlocked[4];
        AmuletUnl = _save.WeaponUnlocked[5];
        OathUnl   = _save.WeaponUnlocked[6];
        EyesUnl   = _save.WeaponUnlocked[7];

        BladesSel = _save.WeaponSelected[0];
        ElecSel   = _save.WeaponSelected[1];
        FireSel   = _save.WeaponSelected[2];
        IceSel    = _save.WeaponSelected[3];
        SoulSel   = _save.WeaponSelected[4];

        SubWeaponIndex = _save.SubWeaponIndex;
        SubwepGlitch   = _save.SubWeaponGlitch;
        SubwepGlitchEnabled = _save.SubWeaponIndex > 1;

        TorsoIndex = _save.TorsoIndex;
        PantsIndex = _save.PantsIndex;

        for (int i = 0; i < InventoryItems.Count && i < _save.Inventory.Length; i++)
            InventoryItems[i].IsChecked = _save.Inventory[i];
    }

    // --- Push UI values to save model ---
    private static readonly CultureInfo Culture = CultureInfo.CurrentCulture;
    private static bool TryF(string s, out float v) => float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, Culture, out v);
    private static bool TryI(string s, out int v)   => int.TryParse(s, NumberStyles.Integer | NumberStyles.AllowThousands, Culture, out v);
    private static int IntOr(string s, int fallback) => TryI(s, out var v) ? v : fallback;

    private void PushToSave()
    {
        if (TryF(Health, out var h)) _save.Health = h;
        if (TryF(HealthRegen, out var hr)) _save.HealthRegen = hr;
        if (TryF(Magic, out var m)) _save.Magic = m;
        if (TryF(MagicRegen, out var mr)) _save.MagicRegen = mr;
        if (TryF(Rage, out var r)) _save.Rage = r;
        if (TryF(RageRegen, out var rr)) _save.RageRegen = rr;
        if (TryI(RedOrbs, out var ro)) _save.RedOrbs = ro;
        if (TryF(XPos, out var x)) _save.XPos = x;
        if (TryF(YPos, out var y)) _save.YPos = y;
        if (TryF(ZPos, out var z)) _save.ZPos = z;
        _save.Swimming = Swimming;
        _save.Frozen = Frozen;
        if (TryI(GorgonEyes, out var ge)) _save.GorgonEyes = ge;
        if (TryI(PhoenixFeathers, out var pf)) _save.PhoenixFeathers = pf;
        if (TryI(SubwepAmmo, out var sa)) _save.SubWeaponAmmo = sa;
        _save.Difficulty = Difficulty;
        _save.NewGamePlus = NewGamePlus;

        string[] lvls = [BladesLvl, ElecLvl, FireLvl, IceLvl, SoulLvl, AmuletLvl, OathLvl, EyesLvl];
        string[] orbs = [BladesOrbs, ElecOrbs, FireOrbs, IceOrbs, SoulOrbs, AmuletOrbs, OathOrbs, EyesOrbs];
        for (int i = 0; i < 8; i++)
        {
            _save.WeaponLevels[i] = IntOr(lvls[i], 1);
            _save.WeaponOrbs[i]   = IntOr(orbs[i], 0);
        }

        _save.WeaponUnlocked[0] = BladesUnl;
        _save.WeaponUnlocked[1] = ElecUnl;
        _save.WeaponUnlocked[2] = FireUnl;
        _save.WeaponUnlocked[3] = IceUnl;
        _save.WeaponUnlocked[4] = SoulUnl;
        _save.WeaponUnlocked[5] = AmuletUnl;
        _save.WeaponUnlocked[6] = OathUnl;
        _save.WeaponUnlocked[7] = EyesUnl;

        _save.WeaponSelected[0] = BladesSel;
        _save.WeaponSelected[1] = ElecSel;
        _save.WeaponSelected[2] = FireSel;
        _save.WeaponSelected[3] = IceSel;
        _save.WeaponSelected[4] = SoulSel;

        _save.SubWeaponIndex = SubWeaponIndex;
        _save.SubWeaponGlitch = SubwepGlitch;
        _save.TorsoIndex = TorsoIndex;
        _save.PantsIndex = PantsIndex;

        for (int i = 0; i < InventoryItems.Count && i < _save.Inventory.Length; i++)
            _save.Inventory[i] = InventoryItems[i].IsChecked;
    }

    // --- Inventory sync helpers ---
    private bool _populating;

    /// <summary>VB chkWepUnl_click + chkEleUnl_Click.</summary>
    private void OnWeaponUnlockChanged(int idx, bool unlocked)
    {
        IsModified = true;
        if (_populating) return;   // loading a slot: inventory is restored from the file afterwards

        SetInv(GoWASave.WeaponIds[idx], unlocked);

        if (idx < 5)
        {
            if (!unlocked)
            {
                switch (idx)
                {
                    case 0: BladesSel = false; break;
                    case 1: ElecSel = false; break;
                    case 2: FireSel = false; break;
                    case 3: IceSel = false; break;
                    case 4: SoulSel = false; break;
                }
            }
            // Element rage items: Elec 0x08, Fire 0x0A, Ice 0x0B, Soul 0x0C
            if (idx > 0)
                SetInv(idx == 1 ? 0x08 : idx + 8, unlocked);
        }

        // With no element unlocked, the starting (non-elemental) rage 0x09 is used
        if (idx is >= 1 and <= 4)
            SetInv(0x09, !(ElecUnl || FireUnl || IceUnl || SoulUnl));
    }

    private void SetInv(int id, bool value)
    {
        if (id >= 0 && id < InventoryItems.Count) InventoryItems[id].IsChecked = value;
    }

    /// <summary>VB cmbSubwep_Changed.</summary>
    private void SyncSubWeaponToInv()
    {
        if (_populating) return;

        for (int i = 0; i <= 20; i++) SetInv(0x1B + i, false);

        int idx = SubWeaponIndex;
        if (idx == 1) SetInv(0x1B, true);   // Bare Fists
        if (idx < 2)
        {
            SubwepGlitchEnabled = false;
            SubwepGlitch = false;
            return;
        }

        SubwepGlitchEnabled = true;
        SubwepGlitch = false;
        idx -= 1;
        int grp = (idx - 1) / 3;
        SetInv(0x1B + idx + grp, true);     // the sub-weapon level item
        SetInv(grp * 4 + 31, true);         // its "Base" item
    }

    private void SyncSubwepGlitchToInv()
    {
        if (_populating || !SubwepGlitchEnabled) return;
        int wepIdx = SubWeaponIndex - 2;
        int wepGrp = wepIdx / 3;
        wepIdx = wepIdx - wepGrp * 3 + wepGrp * 4 + 28;
        if (wepIdx >= 0 && wepIdx < InventoryItems.Count)
            InventoryItems[wepIdx].IsChecked = !SubwepGlitch;
    }

    private void SyncCostumeToInv()
    {
        if (_populating) return;
        for (int i = 0; i < GoWASave.TorsoIds.Length; i++)
        {
            int id = GoWASave.TorsoIds[i];
            if (id < InventoryItems.Count)
                InventoryItems[id].IsChecked = (i + 1 == TorsoIndex);
        }
        for (int i = 0; i < GoWASave.PantsIds.Length; i++)
        {
            int id = GoWASave.PantsIds[i];
            if (id < InventoryItems.Count)
                InventoryItems[id].IsChecked = (i + 1 == PantsIndex);
        }
    }

    // --- Commands ---
    public event Action? BrowseRequested;

    private void Browse()
    {
        BrowseRequested?.Invoke();
    }

    private void Open()
    {
        if (string.IsNullOrWhiteSpace(Folder) || !Directory.Exists(Folder))
        {
            StatusText = $"Folder not found: {Folder}";
            return;
        }

        _save.FolderPath = Folder;

        // The original editor shows the 31 slot buttons before checking DAT-1,
        // so an empty/invalid slot 1 never locks you out of the other slots.
        ShowData = true;
        ShowBrowse = false;
        ShowOpen = false;

        _save.Channel = SelectedChannel;
        string? firstError = _save.Load();
        if (firstError == null)
        {
            PopulateFromSave();
            IsLoaded = true;
            StatusText = $"Loaded slot {SelectedChannel}. Pick another slot above to switch.";
            return;
        }

        // Slot 1 didn't load: look for the first slot that does
        for (int ch = 1; ch <= 31; ch++)
        {
            if (ch == SelectedChannel) continue;
            _save.Channel = ch;
            if (_save.Load() == null)
            {
                SelectedChannel = ch;
                PopulateFromSave();
                IsLoaded = true;
                StatusText = $"{firstError}\nLoaded slot {ch} instead.";
                return;
            }
        }

        _save.Channel = SelectedChannel;
        IsLoaded = false;
        StatusText = firstError + "\nNo valid DAT-1 … DAT-31 file was found in this folder.";
    }

    private void Save()
    {
        if (!IsLoaded) { StatusText = "Open a valid slot first."; return; }
        PushToSave();
        _save.FolderPath = Folder;
        _save.Channel = SelectedChannel;
        string? err = _save.Save();
        if (err == null)
        {
            IsModified = false;
            StatusText = $"Saved slot {SelectedChannel}.";
        }
        else
        {
            StatusText = err;
        }
    }
}   







