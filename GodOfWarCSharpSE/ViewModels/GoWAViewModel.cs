using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Platform.Storage;
using GodOfWarCSharpSE.Models;
using ReactiveUI;
using System.Reactive;

namespace GodOfWarCSharpSE.ViewModels;

public class GoWAViewModel : ReactiveObject
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
    public ReactiveCommand<Unit, Unit> BrowseCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<int, Unit> ChannelCommand { get; }

    // --- Observable properties ---
    public string Folder
    {
        get => _folder;
        set => this.RaiseAndSetIfChanged(ref _folder, value);
    }

    public bool IsLoaded
    {
        get => _isLoaded;
        private set => this.RaiseAndSetIfChanged(ref _isLoaded, value);
    }

    public bool IsModified
    {
        get => _isModified;
        private set => this.RaiseAndSetIfChanged(ref _isModified, value);
    }

    public int SelectedChannel
    {
        get => _selectedChannel;
        private set => this.RaiseAndSetIfChanged(ref _selectedChannel, value);
    }

    public bool ShowBrowse
    {
        get => _showBrowse;
        private set => this.RaiseAndSetIfChanged(ref _showBrowse, value);
    }

    public bool ShowOpen
    {
        get => _showOpen;
        private set => this.RaiseAndSetIfChanged(ref _showOpen, value);
    }

    public bool ShowData
    {
        get => _showData;
        private set => this.RaiseAndSetIfChanged(ref _showData, value);
    }

    // Character
    public string Health       { get => _health;       set { this.RaiseAndSetIfChanged(ref _health, value); IsModified = true; } }
    public string HealthRegen  { get => _healthRegen;  set { this.RaiseAndSetIfChanged(ref _healthRegen, value); IsModified = true; } }
    public string Magic         { get => _magic;        set { this.RaiseAndSetIfChanged(ref _magic, value); IsModified = true; } }
    public string MagicRegen   { get => _magicRegen;   set { this.RaiseAndSetIfChanged(ref _magicRegen, value); IsModified = true; } }
    public string Rage          { get => _rage;         set { this.RaiseAndSetIfChanged(ref _rage, value); IsModified = true; } }
    public string RageRegen    { get => _rageRegen;    set { this.RaiseAndSetIfChanged(ref _rageRegen, value); IsModified = true; } }
    public string RedOrbs      { get => _redOrbs;      set { this.RaiseAndSetIfChanged(ref _redOrbs, value); IsModified = true; } }
    public string XPos         { get => _xPos;         set { this.RaiseAndSetIfChanged(ref _xPos, value); IsModified = true; } }
    public string YPos         { get => _yPos;         set { this.RaiseAndSetIfChanged(ref _yPos, value); IsModified = true; } }
    public string ZPos         { get => _zPos;         set { this.RaiseAndSetIfChanged(ref _zPos, value); IsModified = true; } }
    public bool Swimming       { get => _swimming;     set { this.RaiseAndSetIfChanged(ref _swimming, value); IsModified = true; } }
    public bool Frozen         { get => _frozen;       set { this.RaiseAndSetIfChanged(ref _frozen, value); IsModified = true; } }
    public string GorgonEyes   { get => _gorgonEyes;   set { this.RaiseAndSetIfChanged(ref _gorgonEyes, value); IsModified = true; } }
    public string PhoenixFeathers { get => _phoenixFeathers; set { this.RaiseAndSetIfChanged(ref _phoenixFeathers, value); IsModified = true; } }
    public string SubwepAmmo   { get => _subwepAmmo;   set { this.RaiseAndSetIfChanged(ref _subwepAmmo, value); IsModified = true; } }
    public int Difficulty      { get => _difficulty;   set { this.RaiseAndSetIfChanged(ref _difficulty, value); IsModified = true; } }
    public bool NewGamePlus    { get => _newGamePlus;  set { this.RaiseAndSetIfChanged(ref _newGamePlus, value); IsModified = true; } }

    // Weapons
    public string BladesLvl   { get => _bladesLvl;   set { this.RaiseAndSetIfChanged(ref _bladesLvl, value); IsModified = true; } }
    public string ElecLvl     { get => _elecLvl;     set { this.RaiseAndSetIfChanged(ref _elecLvl, value); IsModified = true; } }
    public string FireLvl     { get => _fireLvl;     set { this.RaiseAndSetIfChanged(ref _fireLvl, value); IsModified = true; } }
    public string IceLvl      { get => _iceLvl;      set { this.RaiseAndSetIfChanged(ref _iceLvl, value); IsModified = true; } }
    public string SoulLvl     { get => _soulLvl;     set { this.RaiseAndSetIfChanged(ref _soulLvl, value); IsModified = true; } }
    public string AmuletLvl   { get => _amuletLvl;   set { this.RaiseAndSetIfChanged(ref _amuletLvl, value); IsModified = true; } }
    public string OathLvl     { get => _oathLvl;     set { this.RaiseAndSetIfChanged(ref _oathLvl, value); IsModified = true; } }
    public string EyesLvl     { get => _eyesLvl;     set { this.RaiseAndSetIfChanged(ref _eyesLvl, value); IsModified = true; } }

    public string BladesOrbs  { get => _bladesOrbs;  set { this.RaiseAndSetIfChanged(ref _bladesOrbs, value); IsModified = true; } }
    public string ElecOrbs    { get => _elecOrbs;    set { this.RaiseAndSetIfChanged(ref _elecOrbs, value); IsModified = true; } }
    public string FireOrbs    { get => _fireOrbs;    set { this.RaiseAndSetIfChanged(ref _fireOrbs, value); IsModified = true; } }
    public string IceOrbs     { get => _iceOrbs;     set { this.RaiseAndSetIfChanged(ref _iceOrbs, value); IsModified = true; } }
    public string SoulOrbs    { get => _soulOrbs;    set { this.RaiseAndSetIfChanged(ref _soulOrbs, value); IsModified = true; } }
    public string AmuletOrbs  { get => _amuletOrbs;  set { this.RaiseAndSetIfChanged(ref _amuletOrbs, value); IsModified = true; } }
    public string OathOrbs    { get => _oathOrbs;    set { this.RaiseAndSetIfChanged(ref _oathOrbs, value); IsModified = true; } }
    public string EyesOrbs    { get => _eyesOrbs;    set { this.RaiseAndSetIfChanged(ref _eyesOrbs, value); IsModified = true; } }

    public bool BladesUnl { get => _bladesUnl; set { this.RaiseAndSetIfChanged(ref _bladesUnl, value); IsModified = true; SyncWeaponToInv(0, value); } }
    public bool ElecUnl   { get => _elecUnl;   set { this.RaiseAndSetIfChanged(ref _elecUnl, value); IsModified = true; SyncWeaponToInv(1, value); } }
    public bool FireUnl   { get => _fireUnl;   set { this.RaiseAndSetIfChanged(ref _fireUnl, value); IsModified = true; SyncWeaponToInv(2, value); } }
    public bool IceUnl    { get => _iceUnl;    set { this.RaiseAndSetIfChanged(ref _iceUnl, value); IsModified = true; SyncWeaponToInv(3, value); } }
    public bool SoulUnl   { get => _soulUnl;   set { this.RaiseAndSetIfChanged(ref _soulUnl, value); IsModified = true; SyncWeaponToInv(4, value); } }
    public bool AmuletUnl { get => _amuletUnl; set { this.RaiseAndSetIfChanged(ref _amuletUnl, value); IsModified = true; } }
    public bool OathUnl   { get => _oathUnl;   set { this.RaiseAndSetIfChanged(ref _oathUnl, value); IsModified = true; } }
    public bool EyesUnl   { get => _eyesUnl;   set { this.RaiseAndSetIfChanged(ref _eyesUnl, value); IsModified = true; } }

    public bool BladesSel { get => _bladesSel; set { this.RaiseAndSetIfChanged(ref _bladesSel, value); IsModified = true; if (value) { ElecSel = false; FireSel = false; IceSel = false; SoulSel = false; } } }
    public bool ElecSel   { get => _elecSel;   set { this.RaiseAndSetIfChanged(ref _elecSel, value); IsModified = true; if (value) { BladesSel = false; FireSel = false; IceSel = false; SoulSel = false; } } }
    public bool FireSel   { get => _fireSel;   set { this.RaiseAndSetIfChanged(ref _fireSel, value); IsModified = true; if (value) { BladesSel = false; ElecSel = false; IceSel = false; SoulSel = false; } } }
    public bool IceSel    { get => _iceSel;    set { this.RaiseAndSetIfChanged(ref _iceSel, value); IsModified = true; if (value) { BladesSel = false; ElecSel = false; FireSel = false; SoulSel = false; } } }
    public bool SoulSel   { get => _soulSel;   set { this.RaiseAndSetIfChanged(ref _soulSel, value); IsModified = true; if (value) { BladesSel = false; ElecSel = false; FireSel = false; IceSel = false; } } }

    // Sub-weapon
    public int SubWeaponIndex
    {
        get => _subWeaponIndex;
        set { this.RaiseAndSetIfChanged(ref _subWeaponIndex, value); IsModified = true; SyncSubWeaponToInv(); }
    }

    public bool SubwepGlitch
    {
        get => _subwepGlitch;
        set { this.RaiseAndSetIfChanged(ref _subwepGlitch, value); IsModified = true; SyncSubwepGlitchToInv(); }
    }

    public bool SubwepGlitchEnabled
    {
        get => _subwepGlitchEnabled;
        private set => this.RaiseAndSetIfChanged(ref _subwepGlitchEnabled, value);
    }

    // Costume
    public int TorsoIndex
    {
        get => _torsoIndex;
        set { this.RaiseAndSetIfChanged(ref _torsoIndex, value); IsModified = true; SyncCostumeToInv(); }
    }

    public int PantsIndex
    {
        get => _pantsIndex;
        set { this.RaiseAndSetIfChanged(ref _pantsIndex, value); IsModified = true; SyncCostumeToInv(); }
    }

    // --- Constructor ---
    public GoWAViewModel()
    {
        InventoryItems = new ObservableCollection<InventoryItem>(
            InventoryNames.Select(n => new InventoryItem(n)));

        BrowseCommand  = ReactiveCommand.Create(Browse);
        OpenCommand    = ReactiveCommand.Create(Open);
        SaveCommand    = ReactiveCommand.Create(Save);
        ChannelCommand = ReactiveCommand.Create<int>(SelectChannel);
    }

    // --- Channel selection ---
    private void SelectChannel(int channel)
    {
        if (IsModified)
        {
            // In production, show a confirmation dialog here
            // For now, proceed
        }
        SelectedChannel = channel;
        _save.Channel = channel;
        string? err = _save.Load();
        if (err == null)
        {
            PopulateFromSave();
            IsLoaded = true;
            ShowData = true;
            ShowBrowse = false;
            ShowOpen = false;
        }
    }

    // --- Populate UI from save ---
    private void PopulateFromSave()
    {
        Health      = _save.Health.ToString("F4");
        HealthRegen = _save.HealthRegen.ToString("F4");
        Magic       = _save.Magic.ToString("F4");
        MagicRegen  = _save.MagicRegen.ToString("F4");
        Rage        = _save.Rage.ToString("F4");
        RageRegen   = _save.RageRegen.ToString("F4");
        RedOrbs     = _save.RedOrbs.ToString("n0");
        XPos        = _save.XPos.ToString("F5");
        YPos        = _save.YPos.ToString("F5");
        ZPos        = _save.ZPos.ToString("F5");
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

        BladesOrbs = _save.WeaponOrbs[0].ToString("n0");
        ElecOrbs   = _save.WeaponOrbs[1].ToString("n0");
        FireOrbs   = _save.WeaponOrbs[2].ToString("n0");
        IceOrbs    = _save.WeaponOrbs[3].ToString("n0");
        SoulOrbs   = _save.WeaponOrbs[4].ToString("n0");
        AmuletOrbs = _save.WeaponOrbs[5].ToString("n0");
        OathOrbs   = _save.WeaponOrbs[6].ToString("n0");
        EyesOrbs   = _save.WeaponOrbs[7].ToString("n0");

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
    private void PushToSave()
    {
        if (float.TryParse(Health, out var h)) _save.Health = h;
        if (float.TryParse(HealthRegen, out var hr)) _save.HealthRegen = hr;
        if (float.TryParse(Magic, out var m)) _save.Magic = m;
        if (float.TryParse(MagicRegen, out var mr)) _save.MagicRegen = mr;
        if (float.TryParse(Rage, out var r)) _save.Rage = r;
        if (float.TryParse(RageRegen, out var rr)) _save.RageRegen = rr;
        if (int.TryParse(RedOrbs.Replace(",", ""), out var ro)) _save.RedOrbs = ro;
        if (float.TryParse(XPos, out var x)) _save.XPos = x;
        if (float.TryParse(YPos, out var y)) _save.YPos = y;
        if (float.TryParse(ZPos, out var z)) _save.ZPos = z;
        _save.Swimming = Swimming;
        _save.Frozen = Frozen;
        if (int.TryParse(GorgonEyes, out var ge)) _save.GorgonEyes = ge;
        if (int.TryParse(PhoenixFeathers, out var pf)) _save.PhoenixFeathers = pf;
        if (int.TryParse(SubwepAmmo, out var sa)) _save.SubWeaponAmmo = sa;
        _save.Difficulty = Difficulty;
        _save.NewGamePlus = NewGamePlus;

        _save.WeaponLevels[0] = int.TryParse(BladesLvl, out var bl) ? bl : 1;
        _save.WeaponLevels[1] = int.TryParse(ElecLvl, out var el) ? el : 1;
        _save.WeaponLevels[2] = int.TryParse(FireLvl, out var fl) ? fl : 1;
        _save.WeaponLevels[3] = int.TryParse(IceLvl, out var il) ? il : 1;
        _save.WeaponLevels[4] = int.TryParse(SoulLvl, out var sl) ? sl : 1;
        _save.WeaponLevels[6] = int.TryParse(OathLvl, out var ol) ? ol : 1;
        _save.WeaponLevels[7] = int.TryParse(EyesLvl, out var yl) ? yl : 1;

        _save.WeaponOrbs[0] = int.TryParse(BladesOrbs.Replace(",", ""), out var bo) ? bo : 0;
        _save.WeaponOrbs[1] = int.TryParse(ElecOrbs.Replace(",", ""), out var eo) ? eo : 0;
        _save.WeaponOrbs[2] = int.TryParse(FireOrbs.Replace(",", ""), out var fo) ? fo : 0;
        _save.WeaponOrbs[3] = int.TryParse(IceOrbs.Replace(",", ""), out var io) ? io : 0;
        _save.WeaponOrbs[4] = int.TryParse(SoulOrbs.Replace(",", ""), out var so) ? so : 0;
        _save.WeaponOrbs[6] = int.TryParse(OathOrbs.Replace(",", ""), out var oo) ? oo : 0;
        _save.WeaponOrbs[7] = int.TryParse(EyesOrbs.Replace(",", ""), out var yo) ? yo : 0;

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
    private void SyncWeaponToInv(int weaponIdx, bool unlocked)
    {
        if (weaponIdx < 1 || weaponIdx > 4) return;
        int rageId = weaponIdx switch
        {
            1 => 0x08, // RageModeElectric
            2 => 0x0A, // RageModeFire
            3 => 0x0B, // RageModeIce
            4 => 0x0C, // RageModeSoul
            _ => -1
        };
        if (rageId >= 0 && rageId < InventoryItems.Count)
            InventoryItems[rageId].IsChecked = unlocked;
    }

    private void SyncSubWeaponToInv()
    {
        // Clear all sub-weapon items (0x1B through 0x2F)
        for (int i = 0; i < 21; i++)
        {
            int invIdx = 0x1B + i;
            if (invIdx < InventoryItems.Count)
            InventoryItems[invIdx].IsChecked = false;
        }

        if (SubWeaponIndex == 0) return;

        if (SubWeaponIndex == 1)
        {
            if (0x1B < InventoryItems.Count)
                InventoryItems[0x1B].IsChecked = true; // Bare Fists
            SubwepGlitchEnabled = false;
            SubwepGlitch = false;
            return;
        }

        SubwepGlitchEnabled = true;
        int idx = SubWeaponIndex - 1;
        int subIdx = 0x1B + idx + (idx / 3);
        int baseIdx = (idx / 3) * 4 + 31;
        if (subIdx < InventoryItems.Count)
            InventoryItems[subIdx].IsChecked = true;
        if (baseIdx < InventoryItems.Count)
            InventoryItems[baseIdx].IsChecked = true;
    }

    private void SyncSubwepGlitchToInv()
    {
        if (!SubwepGlitchEnabled) return;
        int wepIdx = SubWeaponIndex - 2;
        int wepGrp = wepIdx / 3;
        wepIdx = wepIdx - wepGrp * 3 + wepGrp * 4 + 28;
        if (wepIdx >= 0 && wepIdx < InventoryItems.Count)
            InventoryItems[wepIdx].IsChecked = !SubwepGlitch;
    }

    private void SyncCostumeToInv()
    {
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
        _save.FolderPath = Folder;
        _save.Channel = SelectedChannel;
        string? err = _save.Load();
        if (err != null) return;
        PopulateFromSave();
        IsLoaded = true;
        ShowData = true;
        ShowBrowse = false;
        ShowOpen = false;
    }

    private void Save()
    {
        PushToSave();
        _save.FolderPath = Folder;
        _save.Channel = SelectedChannel;
        string? err = _save.Save();
        if (err == null)
            IsModified = false;
    }
}   







