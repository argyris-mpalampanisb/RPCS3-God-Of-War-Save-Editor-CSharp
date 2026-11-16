using GodOfWarCSharpSE.Models;
using System;
using System.IO;
namespace GodOfWarCSharpSE.Models;

public class GoW1Save
{
    // --- State ---
    private byte[] _data = [];
    private byte[] _master = [];
    public string SaveFolder { get; set; } = "";
    public string Filename { get; set; } = "";
    public int SlotNum { get; set; } = -1;  // -1 = master, 0-3 = slots
    public bool Modified { get; set; }
    public bool IsMaster => SlotNum == -1;

    // --- Master flags (offset 0x43) ---
    public bool BeatGame    { get => (_master[0x43] & 2) == 2;    set { _master[0x43] = (byte)((_master[0x43] & ~2) | (value ? 2 : 0)); Modified = true; } }
    public bool BeatVH      { get => (_master[0x43] & 4) == 4;    set { _master[0x43] = (byte)((_master[0x43] & ~4) | (value ? 4 : 0)); Modified = true; } }
    public bool BeatCotG    { get => (_master[0x43] & 8) == 8;    set { _master[0x43] = (byte)((_master[0x43] & ~8) | (value ? 8 : 0)); Modified = true; } }
    public bool Statues     { get => (_master[0x43] & 16) == 16;  set { _master[0x43] = (byte)((_master[0x43] & ~16) | (value ? 16 : 0)); Modified = true; } }

    // --- Slot data fields ---
    public string Wad1 { get => SaveFile.ReadAscii(_data, 8, 8); set { SaveFile.WriteAscii(_data, 8, value, 8); Modified = true; } }
    public bool Wad1Active { get => _data[0x12] != 0; set { _data[0x12] = (byte)(value ? 1 : 0); Modified = true; } }
    public string Wad2 { get => SaveFile.ReadAscii(_data, 19, 8); set { SaveFile.WriteAscii(_data, 19, value, 8); Modified = true; } }
    public bool Wad2Active { get => _data[0x1D] != 0; set { _data[0x1D] = (byte)(value ? 1 : 0); Modified = true; } }

    public float XPos { get => SaveFile.ReadSingle(_data, 0x4E, true); set { SaveFile.WriteSingle(_data, 0x4E, value, true); Modified = true; } }
    public float YPos { get => SaveFile.ReadSingle(_data, 0x52, true); set { SaveFile.WriteSingle(_data, 0x52, value, true); Modified = true; } }
    public float ZPos { get => SaveFile.ReadSingle(_data, 0x56, true); set { SaveFile.WriteSingle(_data, 0x56, value, true); Modified = true; } }

    public bool Swimming { get => _data[0x60] > 0 && _data[0x60] < 4; set { _data[0x60] = (byte)(value ? 2 : 0); Modified = true; } }
    public float SecondsPlayed { get => SaveFile.ReadSingle(_data, 0x6A, true); set { SaveFile.WriteSingle(_data, 0x6A, value, true); Modified = true; } }

    public float Health { get => SaveFile.ReadSingle(_data, 0x76, true); set { SaveFile.WriteSingle(_data, 0x76, value, true); Modified = true; } }
    public float Magic { get => SaveFile.ReadSingle(_data, 0x7A, true); set { SaveFile.WriteSingle(_data, 0x7A, value, true); Modified = true; } }
    public float Rage { get => SaveFile.ReadSingle(_data, 0x7E, true); set { SaveFile.WriteSingle(_data, 0x7E, value, true); Modified = true; } }
    public float MagicRegen { get => SaveFile.ReadSingle(_data, 0x82, true); set { SaveFile.WriteSingle(_data, 0x82, value, true); Modified = true; } }
    public ushort RedOrbs { get => SaveFile.ReadUInt16(_data, 0x86, true); set { SaveFile.WriteUInt16(_data, 0x86, value, true); Modified = true; } }

    public byte HealthExt { get => _data[0x94]; set { _data[0x94] = value; Modified = true; } }
    public byte MagicExt { get => _data[0x95]; set { _data[0x95] = value; Modified = true; } }

    public byte PRLevel { get => (byte)(_data[0x96] + 1); set { _data[0x96] = (byte)(value - 1); Modified = true; } }
    public byte MGLevel { get => (byte)(_data[0x97] + 1); set { _data[0x97] = (byte)(value - 1); Modified = true; } }
    public byte ZFLevel { get => (byte)(_data[0x98] + 1); set { _data[0x98] = (byte)(value - 1); Modified = true; } }
    public byte AoHLevel { get => (byte)(_data[153] + 1); set { _data[153] = (byte)(value - 1); Modified = true; } }
    public byte BoALevel { get => (byte)(_data[154] + 1); set { _data[154] = (byte)(value - 1); Modified = true; } }
    public byte BoCLevel { get => (byte)(_data[0x9B] + 1); set { _data[0x9B] = (byte)(value - 1); Modified = true; } }

    public byte SelectedWeapon { get => _data[0x9C]; set { _data[0x9C] = value; Modified = true; } }
    public bool PRSelected { get => SelectedWeapon == 3; set { if (value) SelectedWeapon = 3; } }
    public bool MGSelected { get => SelectedWeapon == 4; set { if (value) SelectedWeapon = 4; } }
    public bool AoHSelected { get => SelectedWeapon == 5; set { if (value) SelectedWeapon = 5; } }
    public bool ZFSelected { get => SelectedWeapon == 6; set { if (value) SelectedWeapon = 6; } }

    public byte GorgonEyes { get => _data[0x9D]; set { _data[0x9D] = value; Modified = true; } }
    public byte PhoenixFeathers { get => _data[0x9E]; set { _data[0x9E] = value; Modified = true; } }
    public byte MuseKeys { get => _data[0x9F]; set { _data[0x9F] = value; Modified = true; } }

    public bool PoseidonsTrident { get => (_data[0xA0] & 1) == 1; set { _data[0xA0] = (byte)((_data[0xA0] & ~1) | (value ? 1 : 0)); Modified = true; } }

    public bool PRUnlocked { get => (_data[0xA1] & 0x04) != 0; set { _data[0xA1] = (byte)((_data[0xA1] & ~0x04) | (value ? 0x04 : 0)); Modified = true; } }
    public bool MGUnlocked { get => (_data[0xA1] & 0x08) != 0; set { _data[0xA1] = (byte)((_data[0xA1] & ~0x08) | (value ? 0x08 : 0)); Modified = true; } }
    public bool ZFUnlocked { get => (_data[0xA1] & 0x10) != 0; set { _data[0xA1] = (byte)((_data[0xA1] & ~0x10) | (value ? 0x10 : 0)); Modified = true; } }
    public bool AoHUnlocked { get => (_data[0xA1] & 0x20) != 0; set { _data[0xA1] = (byte)((_data[0xA1] & ~0x20) | (value ? 0x20 : 0)); Modified = true; } }
    public bool BoAUnlocked { get => (_data[0xA1] & 0x40) != 0; set { _data[0xA1] = (byte)((_data[0xA1] & ~0x40) | (value ? 0x40 : 0)); Modified = true; } }

    public string Camera { get => SaveFile.ReadAscii(_data, 0xAE, 24); set { SaveFile.WriteAscii(_data, 0xAE, value, 24); Modified = true; } }
    public string CamWad { get => SaveFile.ReadAscii(_data, 0xC6, 24); set { SaveFile.WriteAscii(_data, 0xC6, value, 24); Modified = true; } }

    public short MoviesSeen { get => SaveFile.ReadInt16(_data, 0x28A, true); set { SaveFile.WriteInt16(_data, 0x28A, value, true); Modified = true; } }

    public bool WorldIdle { get => _data[0x41E] != 0; set { _data[0x41E] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte MusicVol { get => _data[0x41F]; set { _data[0x41F] = value; Modified = true; } }
    public byte SoundVol { get => _data[0x420]; set { _data[0x420] = value; Modified = true; } }
    public bool Vibration { get => _data[0x421] != 0; set { _data[0x421] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool Widescreen { get => _data[0x422] != 0; set { _data[0x422] = (byte)(value ? 1 : 0); Modified = true; } }
    public bool DeFlicker { get => _data[0x423] != 0; set { _data[0x423] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte SoundMode { get => _data[0x424]; set { _data[0x424] = value; Modified = true; } }
    public bool PUFT { get => _data[0x425] != 0; set { _data[0x425] = (byte)(value ? 1 : 0); Modified = true; } }
    public byte Costume { get => _data[0x426]; set { _data[0x426] = value; _data[7] = value; Modified = true; } }
    public byte Difficulty { get => _data[0x427]; set { _data[0x427] = value; Modified = true; } }
    public bool EndGame { get => _data[0x428] != 0; set { _data[0x428] = (byte)(value ? 1 : 0); Modified = true; } }

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

        // Recompute & write checksum
        uint csum = SaveFile.ComputeChecksum(_data[..^4]);
        SaveFile.WriteUInt32(_data, _data.Length - 4, csum, true);

        File.WriteAllBytes(Path.Combine(SaveFolder, Filename), _data);

        // Update MASTER.BIN slot entry
        int slotOffset = 4 + 16 * SlotNum;
        _master[slotOffset + 0] = 0xCA;
        _master[slotOffset + 1] = 0xFE;
        _master[slotOffset + 2] = 0xBA;
        _master[slotOffset + 3] = 0xD1;

        // Copy playtime (4 bytes from 0x6A)
        Array.Copy(_data, 0x6A, _master, slotOffset + 4, 4);

        // Set "has save" flag
        if (_master[slotOffset + 0xD] == 0) _master[slotOffset + 0xD] = 1;
        _master[slotOffset + 0xE] = _data[0x427]; // difficulty

        File.WriteAllBytes(Path.Combine(SaveFolder, "MASTER.BIN"), _master);
        Modified = false;
        return true;
    }

    // --- New Game (reset slot) ---
    public bool NewGame(byte[] blankSave)
    {
        if (IsMaster) return false;
        File.WriteAllBytes(Path.Combine(SaveFolder, Filename), blankSave);

        // Reset MASTER.BIN slot entry
        int slotOffset = 4 + 16 * SlotNum;
        _master[slotOffset + 4] = 0;
        _master[slotOffset + 5] = 0;
        _master[slotOffset + 6] = 0;
        _master[slotOffset + 7] = 0;
        _master[slotOffset + 0xD] = 1;
        File.WriteAllBytes(Path.Combine(SaveFolder, "MASTER.BIN"), _master);

        // Reload
        return Load(SaveFolder, SlotNum);
    }

    // --- Make all movies skippable ---
    public void MakeMoviesSkippable(string[] movieNames)
    {
        MoviesSeen = (short)movieNames.Length;
        int offset = 0x28E;
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