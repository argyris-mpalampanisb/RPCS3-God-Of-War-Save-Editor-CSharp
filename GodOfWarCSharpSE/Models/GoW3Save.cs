namespace GodOfWarCSharpSE.Models;

public class GoW3Save
{
    private byte[] _data = [];
    private byte[] _sfo = [];
    public string SaveFolder { get; set; } = "";
    public bool IsUserData { get; set; }
    public bool Modified { get; set; }

    // --- UserData (offsets in SAVEDATA) ---
    public bool Ch5 { get => (_data[0x6] & 1) != 0; set { _data[0x6] = (byte)((_data[0x6] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool Ch6 { get => (_data[0x6] & 2) != 0; set { _data[0x6] = (byte)((_data[0x6] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool Ch7 { get => (_data[0x6] & 4) != 0; set { _data[0x6] = (byte)((_data[0x6] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool Ch1 { get => (_data[0x7] & 0x10) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }
    public bool Ch2 { get => (_data[0x7] & 0x20) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~0x20) | (value ? 0x20 : 0)); Modified = true; } }
    public bool Ch3 { get => (_data[0x7] & 0x40) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~0x40) | (value ? 0x40 : 0)); Modified = true; } }
    public bool Ch4 { get => (_data[0x7] & 0x80) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~0x80) | (value ? 0x80 : 0)); Modified = true; } }
    public bool BeatEasy { get => (_data[0x7] & 1) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool BeatNormal { get => (_data[0x7] & 2) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool BeatHard { get => (_data[0x7] & 4) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool BeatVeryHard { get => (_data[0x7] & 8) != 0; set { _data[0x7] = (byte)((_data[0x7] & ~8) | (value ? 8 : 0)); Modified = true; } }

    public byte MusVol { get => _data[0xE]; set { _data[0xE] = value; Modified = true; } }
    public byte SpeechVol { get => _data[0xF]; set { _data[0xF] = value; Modified = true; } }
    public byte SfxVol { get => _data[0x10]; set { _data[0x10] = value; Modified = true; } }
    public byte CineVol { get => _data[0x11]; set { _data[0x11] = value; Modified = true; } }
    public byte SoundMode { get => _data[0x12]; set { _data[0x12] = value; Modified = true; } }
    public byte Brightness { get => _data[0x13]; set { _data[0x13] = value; Modified = true; } }
    public byte ScreenScale { get => _data[0x14]; set { _data[0x14] = value; Modified = true; } }
    public byte ScreenLocX { get => _data[0x15]; set { _data[0x15] = value; Modified = true; } }
    public byte ScreenLocY { get => _data[0x16]; set { _data[0x16] = value; Modified = true; } }
    public byte TutDisplay { get => _data[0x17]; set { _data[0x17] = value; Modified = true; } }
    public bool ShowSubs { get => _data[0x18] == 1; set { _data[0x18] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool InvertFlight { get => _data[0x1A] == 1; set { _data[0x1A] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool InvertFreeCam { get => _data[0x1B] == 1; set { _data[0x1B] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte DefaultCostume { get => _data[0x1D]; set { _data[0x1D] = value; Modified = true; } }

    // --- SaveData (dynamic offsets via PlayerLoc) ---
    private int PlayerLoc => SaveFile.ReadInt16(_data, 0x46, true);

    public float XPos { get => SaveFile.ReadSingle(_data, PlayerLoc + 0x7C, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0x7C, value, true); Modified = true; } }
    public float YPos { get => SaveFile.ReadSingle(_data, PlayerLoc + 0x80, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0x80, value, true); Modified = true; } }
    public float ZPos { get => SaveFile.ReadSingle(_data, PlayerLoc + 0x84, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0x84, value, true); Modified = true; } }

    // Possessions (byte at PlayerLoc+0xA0)
    public bool AphroditesGarter { get => (_data[PlayerLoc + 0xA0] & 1) != 0; set { _data[PlayerLoc + 0xA0] = (byte)((_data[PlayerLoc + 0xA0] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool HephaestusRing { get => (_data[PlayerLoc + 0xA0] & 2) != 0; set { _data[PlayerLoc + 0xA0] = (byte)((_data[PlayerLoc + 0xA0] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool HerasChalice { get => (_data[PlayerLoc + 0xA0] & 4) != 0; set { _data[PlayerLoc + 0xA0] = (byte)((_data[PlayerLoc + 0xA0] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool ZeusEagle { get => (_data[PlayerLoc + 0xA0] & 8) != 0; set { _data[PlayerLoc + 0xA0] = (byte)((_data[PlayerLoc + 0xA0] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool DaedalusSchematics { get => (_data[PlayerLoc + 0xA0] & 0x10) != 0; set { _data[PlayerLoc + 0xA0] = (byte)((_data[PlayerLoc + 0xA0] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }

    // Weapon unlocks (byte at PlayerLoc+0xA1)
    public bool WhipUnlock { get => (_data[PlayerLoc + 0xA1] & 1) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool BladesUnlock { get => (_data[PlayerLoc + 0xA1] & 2) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool ClawsUnlock { get => (_data[PlayerLoc + 0xA1] & 4) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool HadesHelm { get => (_data[PlayerLoc + 0xA1] & 8) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool HeliosShield { get => (_data[PlayerLoc + 0xA1] & 0x10) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }
    public bool HermesCoin { get => (_data[PlayerLoc + 0xA1] & 0x20) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~0x20) | (value ? 0x20 : 0)); Modified = true; } }
    public bool HerculesShouldGuard { get => (_data[PlayerLoc + 0xA1] & 0x40) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~0x40) | (value ? 0x40 : 0)); Modified = true; } }
    public bool PoseidonsConchShell { get => (_data[PlayerLoc + 0xA1] & 0x80) != 0; set { _data[PlayerLoc + 0xA1] = (byte)((_data[PlayerLoc + 0xA1] & ~0x80) | (value ? 0x80 : 0)); Modified = true; } }

    // Equipment (byte at PlayerLoc+0xA2)
    public bool BladeOfOlympusRage { get => (_data[PlayerLoc + 0xA2] & 2) != 0; set { _data[PlayerLoc + 0xA2] = (byte)((_data[PlayerLoc + 0xA2] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool BootsUnlock { get => (_data[PlayerLoc + 0xA2] & 4) != 0; set { _data[PlayerLoc + 0xA2] = (byte)((_data[PlayerLoc + 0xA2] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool PoseidonsTrident { get => (_data[PlayerLoc + 0xA2] & 8) != 0; set { _data[PlayerLoc + 0xA2] = (byte)((_data[PlayerLoc + 0xA2] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool BladeOfOlympus { get => (_data[PlayerLoc + 0xA2] & 0x10) != 0; set { _data[PlayerLoc + 0xA2] = (byte)((_data[PlayerLoc + 0xA2] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }
    public bool CestusUnlock { get => (_data[PlayerLoc + 0xA2] & 0x80) != 0; set { _data[PlayerLoc + 0xA2] = (byte)((_data[PlayerLoc + 0xA2] & ~0x80) | (value ? 0x80 : 0)); Modified = true; } }

    // More equipment (byte at PlayerLoc+0xA3)
    public bool BowUnlock { get => (_data[PlayerLoc + 0xA3] & 1) != 0; set { _data[PlayerLoc + 0xA3] = (byte)((_data[PlayerLoc + 0xA3] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool HeadUnlock { get => (_data[PlayerLoc + 0xA3] & 4) != 0; set { _data[PlayerLoc + 0xA3] = (byte)((_data[PlayerLoc + 0xA3] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool BoreasIcestorm { get => (_data[PlayerLoc + 0xA3] & 8) != 0; set { _data[PlayerLoc + 0xA3] = (byte)((_data[PlayerLoc + 0xA3] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool GoldenFleece { get => (_data[PlayerLoc + 0xA3] & 0x10) != 0; set { _data[PlayerLoc + 0xA3] = (byte)((_data[PlayerLoc + 0xA3] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }
    public bool IcarusWings { get => (_data[PlayerLoc + 0xA3] & 0x20) != 0; set { _data[PlayerLoc + 0xA3] = (byte)((_data[PlayerLoc + 0xA3] & ~0x20) | (value ? 0x20 : 0)); Modified = true; } }

    // Stats
    public float CurrHealth { get => SaveFile.ReadSingle(_data, PlayerLoc + 0xA8, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0xA8, value, true); Modified = true; } }
    public float CurrMagic { get => SaveFile.ReadSingle(_data, PlayerLoc + 0xAC, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0xAC, value, true); Modified = true; } }
    public float CurrItem { get => SaveFile.ReadSingle(_data, PlayerLoc + 0xB0, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0xB0, value, true); Modified = true; } }
    public float RedOrbs { get => SaveFile.ReadSingle(_data, PlayerLoc + 0xC4, true); set { SaveFile.WriteSingle(_data, PlayerLoc + 0xC4, value, true); Modified = true; } }

    public byte HealthExt { get => _data[PlayerLoc + 0xDA]; set { _data[PlayerLoc + 0xDA] = value; Modified = true; } }
    public byte MagicExt { get => _data[PlayerLoc + 0xDB]; set { _data[PlayerLoc + 0xDB] = value; Modified = true; } }
    public byte ItemExt { get => _data[PlayerLoc + 0xDC]; set { _data[PlayerLoc + 0xDC] = value; Modified = true; } }

    public byte BowLevel { get => _data[PlayerLoc + 0xDD]; set { _data[PlayerLoc + 0xDD] = value; Modified = true; } }
    public byte BootsLevel { get => _data[PlayerLoc + 0xDE]; set { _data[PlayerLoc + 0xDE] = value; Modified = true; } }
    public byte HeadLevel { get => _data[PlayerLoc + 0xDF]; set { _data[PlayerLoc + 0xDF] = value; Modified = true; } }
    public byte CestusLevel { get => _data[PlayerLoc + 0xE0]; set { _data[PlayerLoc + 0xE0] = value; Modified = true; } }
    public byte WhipLevel { get => _data[PlayerLoc + 0xE1]; set { _data[PlayerLoc + 0xE1] = value; Modified = true; } }
    public byte BladesLevel { get => _data[PlayerLoc + 0xE2]; set { _data[PlayerLoc + 0xE2] = value; Modified = true; } }
    public byte ClawsLevel { get => _data[PlayerLoc + 0xE3]; set { _data[PlayerLoc + 0xE3] = value; Modified = true; } }

    public byte CurrDiff { get => _data[PlayerLoc + 0x48A]; set { _data[PlayerLoc + 0x48A] = value; Modified = true; } }
    public byte CurrCost { get => _data[PlayerLoc + 0x48B]; set { _data[PlayerLoc + 0x48B] = value; Modified = true; } }
    public bool HealthInfinite { get => _data[PlayerLoc + 0x48C] != 0; set { _data[PlayerLoc + 0x48C] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool MagicInfinite { get => _data[PlayerLoc + 0x493] != 0; set { _data[PlayerLoc + 0x493] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool ItemInfinite { get => _data[PlayerLoc + 0x496] != 0; set { _data[PlayerLoc + 0x496] = (byte)(value ? 1 : 0); Modified = true; } }

    // --- SFO ---
    public string SaveDesc { get; set; } = "";
    public string SaveArea { get; set; } = "";

    // --- Load ---
    public bool Load(string folder)
    {
        SaveFolder = folder;
        IsUserData = folder.ToLower().Contains("-userdata");
        Modified = false;

        string dataPath = Path.Combine(folder, "SAVEDATA");
        if (!File.Exists(dataPath)) return false;
        _data = File.ReadAllBytes(dataPath);

        if (!IsUserData)
        {
            string sfoPath = Path.Combine(folder, "PARAM.SFO");
            if (File.Exists(sfoPath))
            {
                _sfo = File.ReadAllBytes(sfoPath);
                ReadSfoStrings();
            }
        }
        return true;
    }

    private void ReadSfoStrings()
    {
        int txtOffset = SaveFile.ReadInt16(_sfo, 0xC, false);
        if (txtOffset == 0x140) txtOffset = 0x158;
        if (txtOffset == 0x190) txtOffset = 0x1B0;
        SaveDesc = SaveFile.ReadAscii(_sfo, txtOffset, 68).Replace("\n", "\r\n").TrimEnd('\0');

        int areaOffset;
        if (txtOffset == 0x158) areaOffset = 0x9B0;
        else if (txtOffset == 0x1B0) areaOffset = 0x9FC;
        else areaOffset = 0x9B0;
        SaveArea = SaveFile.ReadAscii(_sfo, areaOffset, 0x7F).Replace("\n", "\r\n").TrimEnd('\0');
    }

    // --- Save ---
    public bool Save()
    {
        if (!IsUserData)
        {
            WriteSfoStrings();
            File.WriteAllBytes(Path.Combine(SaveFolder, "PARAM.SFO"), _sfo);
        }
        File.WriteAllBytes(Path.Combine(SaveFolder, "SAVEDATA"), _data);
        Modified = false;
        return true;
    }

    private void WriteSfoStrings()
    {
        int txtOffset = SaveFile.ReadInt16(_sfo, 0xC, false);
        if (txtOffset == 0x140) txtOffset = 0x158;
        if (txtOffset == 0x190) txtOffset = 0x1B0;

        string desc = SaveDesc.Replace("\r\n", "\n");
        SaveFile.WriteAscii(_sfo, txtOffset, desc, 68);

        int areaOffset;
        if (txtOffset == 0x158) areaOffset = 0x9B0;
        else if (txtOffset == 0x1B0) areaOffset = 0x9FC;
        else areaOffset = 0x9B0;

        string area = SaveArea.Replace("\r\n", "\n");
        SaveFile.WriteAscii(_sfo, areaOffset, area, 0x7F);
    }
}