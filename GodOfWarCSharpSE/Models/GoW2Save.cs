namespace GodOfWarCSharpSE.Models;

public class GoW2Save
{
    private byte[] _data = [];
    private byte[] _master = [];
    public string SaveFolder { get; set; } = "";
    public string Filename { get; set; } = "";
    public int SlotNum { get; set; } = -1;
    public bool Modified { get; set; }
    public bool IsMaster => SlotNum == -1;

    // --- Master flags ---
    public bool CyclopsEyesMaster { get => (_master[0xA2] & 1) == 1; set { _master[0xA2] = (byte)((_master[0xA2] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool BeatEMaster { get => (_master[0xA3] & 1) == 1; set { _master[0xA3] = (byte)((_master[0xA3] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool BeatNMaster { get => (_master[0xA3] & 2) == 2; set { _master[0xA3] = (byte)((_master[0xA3] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool BeatHMaster { get => (_master[0xA3] & 4) == 4; set { _master[0xA3] = (byte)((_master[0xA3] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool BeatVHMaster { get => (_master[0xA3] & 8) == 8; set { _master[0xA3] = (byte)((_master[0xA3] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool MortalRankMaster { get => (_master[0xA3] & 16) == 16; set { _master[0xA3] = (byte)((_master[0xA3] & ~16) | (value ? 16 : 0)); Modified = true; } }
    public bool SpartanRankMaster { get => (_master[0xA3] & 32) == 32; set { _master[0xA3] = (byte)((_master[0xA3] & ~32) | (value ? 32 : 0)); Modified = true; } }
    public bool GodRankMaster { get => (_master[0xA3] & 64) == 64; set { _master[0xA3] = (byte)((_master[0xA3] & ~64) | (value ? 64 : 0)); Modified = true; } }
    public bool TitanRankMaster { get => (_master[0xA3] & 128) == 128; set { _master[0xA3] = (byte)((_master[0xA3] & ~128) | (value ? 128 : 0)); Modified = true; } }

    // --- Slot data ---
    public byte Costume { get => _data[7]; set { _data[7] = value; _data[230] = value; Modified = true; } }

    public string Wad1 { get => SaveFile.ReadAscii(_data, 8, 16); set { SaveFile.WriteAscii(_data, 8, value, 16); Modified = true; } }
    public bool Wad1Active { get => _data[28] != 0; set { _data[28] = (byte)(value ? 1 : 0); Modified = true; } }
    public string Wad2 { get => SaveFile.ReadAscii(_data, 29, 16); set { SaveFile.WriteAscii(_data, 29, value, 16); Modified = true; } }
    public bool Wad2Active { get => _data[49] != 0; set { _data[49] = (byte)(value ? 1 : 0); Modified = true; } }

    public float XPos { get => SaveFile.ReadSingle(_data, 0x76, true); set { SaveFile.WriteSingle(_data, 0x76, value, true); Modified = true; } }
    public float Height { get => SaveFile.ReadSingle(_data, 0x7A, true); set { SaveFile.WriteSingle(_data, 0x7A, value, true); Modified = true; } }
    public float YPos { get => SaveFile.ReadSingle(_data, 0x7E, true); set { SaveFile.WriteSingle(_data, 0x7E, value, true); Modified = true; } }

    public bool Swimming { get => _data[144] > 0 && _data[144] < 4; set { _data[144] = (byte)(value ? 2 : 0); Modified = true; } }

    // Unlocks
    public bool BoOUnl { get => (_data[155] & 1) != 0; set { _data[155] = (byte)((_data[155] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool UnkUnl1_02 { get => (_data[155] & 2) != 0; set { _data[155] = (byte)((_data[155] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool UrnGaiaAcq { get => (_data[155] & 4) != 0; set { _data[155] = (byte)((_data[155] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool UrnGorgonAcq { get => (_data[155] & 8) != 0; set { _data[155] = (byte)((_data[155] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool UrnOlympusAcq { get => (_data[155] & 16) != 0; set { _data[155] = (byte)((_data[155] & ~16) | (value ? 16 : 0)); Modified = true; } }
    public bool UrnPrometheusAcq { get => (_data[155] & 32) != 0; set { _data[155] = (byte)((_data[155] & ~32) | (value ? 32 : 0)); Modified = true; } }
    public bool UrnFatesAcq { get => (_data[155] & 64) != 0; set { _data[155] = (byte)((_data[155] & ~64) | (value ? 64 : 0)); Modified = true; } }
    public bool UrnPoseidonAcq { get => (_data[155] & 128) != 0; set { _data[155] = (byte)((_data[155] & ~128) | (value ? 128 : 0)); Modified = true; } }

    public bool UnkUnl2_01 { get => (_data[156] & 1) != 0; set { _data[156] = (byte)((_data[156] & ~1) | (value ? 1 : 0)); Modified = true; } }
    public bool RageAcq { get => (_data[156] & 2) != 0; set { _data[156] = (byte)((_data[156] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool AmuletAcq { get => (_data[156] & 4) != 0; set { _data[156] = (byte)((_data[156] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool UnkUnl2_08 { get => (_data[156] & 8) != 0; set { _data[156] = (byte)((_data[156] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool UnkUnl2_10 { get => (_data[156] & 16) != 0; set { _data[156] = (byte)((_data[156] & ~16) | (value ? 16 : 0)); Modified = true; } }
    public bool SoDUnl { get => (_data[156] & 32) != 0; set { _data[156] = (byte)((_data[156] & ~32) | (value ? 32 : 0)); Modified = true; } }
    public bool BHUnl { get => (_data[156] & 64) != 0; set { _data[156] = (byte)((_data[156] & ~64) | (value ? 64 : 0)); Modified = true; } }
    public bool EHAcq { get => (_data[156] & 128) != 0; set { _data[156] = (byte)((_data[156] & ~128) | (value ? 128 : 0)); Modified = true; } }

    public bool UnkUnl3_80 { get => (_data[157] & 128) != 0; set { _data[157] = (byte)((_data[157] & ~128) | (value ? 128 : 0)); Modified = true; } }
    public bool WingsAcq { get => (_data[157] & 64) != 0; set { _data[157] = (byte)((_data[157] & ~64) | (value ? 64 : 0)); Modified = true; } }
    public bool AQAcq { get => (_data[157] & 32) != 0; set { _data[157] = (byte)((_data[157] & ~32) | (value ? 32 : 0)); Modified = true; } }
    public bool FleeceAcq { get => (_data[157] & 16) != 0; set { _data[157] = (byte)((_data[157] & ~16) | (value ? 16 : 0)); Modified = true; } }
    public bool UnkUnl3_08 { get => (_data[157] & 8) != 0; set { _data[157] = (byte)((_data[157] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool TBAcq { get => (_data[157] & 4) != 0; set { _data[157] = (byte)((_data[157] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool CRAcq { get => (_data[157] & 2) != 0; set { _data[157] = (byte)((_data[157] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool PRAcq { get => (_data[157] & 1) != 0; set { _data[157] = (byte)((_data[157] & ~1) | (value ? 1 : 0)); Modified = true; } }

    // Stats
    public float Health { get => SaveFile.ReadSingle(_data, 0xA2, true); set { SaveFile.WriteSingle(_data, 0xA2, value, true); Modified = true; } }
    public float Magic { get => SaveFile.ReadSingle(_data, 0xA6, true); set { SaveFile.WriteSingle(_data, 0xA6, value, true); Modified = true; } }
    public float Item { get => SaveFile.ReadSingle(_data, 0xAA, true); set { SaveFile.WriteSingle(_data, 0xAA, value, true); Modified = true; } }
    public float Rage { get => SaveFile.ReadSingle(_data, 0xAE, true); set { SaveFile.WriteSingle(_data, 0xAE, value, true); Modified = true; } }
    public float MagicRegen { get => SaveFile.ReadSingle(_data, 0xB2, true); set { SaveFile.WriteSingle(_data, 0xB2, value, true); Modified = true; } }
    public int RedOrbs { get => SaveFile.ReadInt32(_data, 0xB6, true); set { SaveFile.WriteInt32(_data, 0xB6, value, true); Modified = true; } }

    public byte HealthExt { get => _data[204]; set { _data[204] = value; Modified = true; } }
    public byte MagicExt { get => _data[205]; set { _data[205] = value; Modified = true; } }
    public byte ItemExt { get => _data[206]; set { _data[206] = value; Modified = true; } }

    public byte BoALvl { get => (byte)(_data[207] + 1); set { _data[207] = (byte)(value - 1); Modified = true; } }
    public byte AQLvl { get => (byte)(_data[209] + 1); set { _data[209] = (byte)(value - 1); Modified = true; } }
    public byte CRLvl { get => (byte)(_data[210] + 1); set { _data[210] = (byte)(value - 1); Modified = true; } }
    public byte PRLvl { get => (byte)(_data[211] + 1); set { _data[211] = (byte)(value - 1); Modified = true; } }
    public byte TBLvl { get => (byte)(_data[212] + 1); set { _data[212] = (byte)(value - 1); Modified = true; } }
    public byte SoDLvl { get => (byte)(_data[213] + 1); set { _data[213] = (byte)(value - 1); Modified = true; } }
    public byte BHLvl { get => (byte)(_data[214] + 1); set { _data[214] = (byte)(value - 1); Modified = true; } }
    public byte EHLvl { get => (byte)(_data[215] + 1); set { _data[215] = (byte)(value - 1); Modified = true; } }
    public byte BoOLvl { get => (byte)(_data[216] + 1); set { _data[216] = (byte)(value - 1); Modified = true; } }

    // Selected weapon: 1=PR, 2=CR, 3=TB, 6=AQ, 16=EH, 14=SoD, 15=BH, 17=BoO
    public byte SelectedWeapon
    {
        get
        {
            byte v = _data[217];
            if (v != 0) return v;
            return _data[218];
        }
        set
        {
            _data[217] = 0;
            _data[218] = 0;
            if (value is 1 or 2 or 3 or 6 or 16) _data[217] = value;
            else if (value is 14 or 15 or 17) _data[218] = value;
            Modified = true;
        }
    }

    public bool ForcedSubweapon { get => _data[219] != 0; set { _data[219] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte GorgonEyes { get => _data[220]; set { _data[220] = value; Modified = true; } }
    public byte PhoenixFeathers { get => _data[221]; set { _data[221] = value; Modified = true; } }
    public byte CyclopsEyes { get => _data[226]; set { _data[226] = value; Modified = true; } }

    public string Camera { get => SaveFile.ReadAscii(_data, 246, 17); set { SaveFile.WriteAscii(_data, 246, value, 17); Modified = true; } }
    public string CamWad { get => SaveFile.ReadAscii(_data, 0x10E, 17); set { SaveFile.WriteAscii(_data, 0x10E, value, 17); Modified = true; } }

    public short MoviesSeen { get => SaveFile.ReadInt16(_data, 0x614, true); set { SaveFile.WriteInt16(_data, 0x614, value, true); Modified = true; } }
    public int SecondsPlayed { get => SaveFile.ReadInt32(_data, 0x83E, true); set { SaveFile.WriteInt32(_data, 0x83E, value, true); Modified = true; } }

    public bool Frozen { get => _data[2114] != 0; set { _data[2114] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte Difficulty { get => _data[2115]; set { _data[2115] = value; Modified = true; } }
    public bool InfiniteHealth { get => _data[2116] != 0; set { _data[2116] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnOlympusActive { get => _data[2117] != 0; set { _data[2117] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnPrometheusActive { get => _data[2118] != 0; set { _data[2118] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnGorgonsActive { get => _data[2119] != 0; set { _data[2119] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnPoseidonActive { get => _data[2120] != 0; set { _data[2120] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnGaiaActive { get => _data[2121] != 0; set { _data[2121] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool UrnFatesActive { get => _data[2122] != 0; set { _data[2122] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool Bonus { get => _data[2124] != 0; set { _data[2124] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte TimesSaved { get => _data[2197]; set { _data[2197] = value; Modified = true; } }

    public string Checksum => SaveFile.ComputeChecksum(_data[..^4]).ToString("X8");

    // --- Load ---
    public bool Load(string folder, int slot = -1)
    {
        SaveFolder = folder;
        SlotNum = slot;
        Modified = false;

        string masterPath = Path.Combine(folder, "MASTER.BIN");
        if (!File.Exists(masterPath)) return false;
        _master = File.ReadAllBytes(masterPath);

        if (slot == -1)
        {
            Filename = "MASTER.BIN";
            _data = _master;
            return true;
        }
        else
        {
            Filename = $"DATA0{slot}.BIN";
            string dataPath = Path.Combine(folder, Filename);
            if (!File.Exists(dataPath)) return false;
            _data = File.ReadAllBytes(dataPath);
            return true;
        }
    }

    // --- Save ---
    public bool Save()
    {
        if (IsMaster)
        {
            File.WriteAllBytes(Path.Combine(SaveFolder, "MASTER.BIN"), _data);
            Modified = false;
            return true;
        }

        uint csum = SaveFile.ComputeChecksum(_data[..^4]);
        SaveFile.WriteUInt32(_data, _data.Length - 4, csum, true);
        File.WriteAllBytes(Path.Combine(SaveFolder, Filename), _data);

        int slotOffset = 4 + 16 * SlotNum;
        _master[slotOffset + 0] = 0xCA;
        _master[slotOffset + 1] = 0xFE;
        _master[slotOffset + 2] = 0xBA;
        _master[slotOffset + 3] = 0xD4;

        Array.Copy(_data, 0x83E, _master, slotOffset + 4, 4);
        if (_master[slotOffset + 0xD] == 0) _master[slotOffset + 0xD] = 1;
        _master[slotOffset + 0xE] = _data[2115];
        _master[slotOffset + 0xF] = _data[2124];

        File.WriteAllBytes(Path.Combine(SaveFolder, "MASTER.BIN"), _master);
        Modified = false;
        return true;
    }

    // --- New Game ---
    public bool NewGame(byte[] blankSave)
    {
        if (IsMaster) return false;
        SaveFile.WriteUInt32(blankSave, blankSave.Length - 4, 0, true);
        File.WriteAllBytes(Path.Combine(SaveFolder, Filename), blankSave);

        int slotOffset = 4 + 16 * SlotNum;
        SaveFile.WriteUInt32(_master, slotOffset, 0xCAFEBAD4, true);
        _master[slotOffset + 4] = 0;
        _master[slotOffset + 5] = 0;
        _master[slotOffset + 6] = 0;
        _master[slotOffset + 7] = 0;
        _master[slotOffset + 0xD] = 1;
        File.WriteAllBytes(Path.Combine(SaveFolder, "MASTER.BIN"), _master);

        return Load(SaveFolder, SlotNum);
    }

    // --- Make all movies skippable ---
    public void MakeMoviesSkippable(string[] movieNames)
    {
        MoviesSeen = (short)movieNames.Length;
        int offset = 0x616;
        foreach (string name in movieNames)
        {
            if (string.IsNullOrWhiteSpace(name)) continue;
            uint csum = SaveFile.ComputeMovieChecksum(name.Trim());
            SaveFile.WriteUInt32(_data, offset, csum, true);
            offset += 4;
        }
        Modified = true;
    }
}