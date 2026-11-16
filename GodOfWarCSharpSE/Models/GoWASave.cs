using System;
using System.IO;
using System.Buffers.Binary;

namespace GodOfWarCSharpSE.Models;

public class GoWASave
{
    // --- Item ID constants ---
    public static readonly byte[] WeaponIds = { 0x00, 0x04, 0x05, 0x06, 0x07, 0x91, 0x93, 0x95 };
    public static readonly byte[] SubWeaponIds = { 0x1B, 0x1C, 0x1D, 0x1E, 0x20, 0x21, 0x22, 0x24, 0x25, 0x26, 0x28, 0x29, 0x2A, 0x2C, 0x2D, 0x2E };
    public static readonly byte[] SubWeaponBaseIds = { 0x1F, 0x23, 0x27, 0x2B, 0x2F };
    public static readonly byte[] TorsoIds = { 0x3C, 0x3D, 0x40, 0x42, 0x44, 0x46, 0x48, 0x4A, 0x4C, 0x4E, 0x50 };
    public static readonly byte[] PantsIds = { 0x3E, 0x3F, 0x41, 0x43, 0x45, 0x47, 0x49, 0x4B, 0x4D, 0x4F, 0x51 };

    // --- KPTR-relative offsets ---
    private const int OffHealth      = 0x0C;
    private const int OffHealthRegen = 0x10;
    private const int OffMagic       = 0x14;
    private const int OffMagicRegen  = 0x18;
    private const int OffRage        = 0x24;
    private const int OffRageRegen   = 0x28;
    private const int OffRedOrbs     = 0x30;
    private const int OffXPos        = 0x38;
    private const int OffYPos        = 0x3C;
    private const int OffZPos        = 0x40;
    private const int OffSwim        = 0x5E;
    private const int OffInvStart    = 0xD7;
    private const int OffFrozen      = 0xCC;
    private const int OffInvCount    = 0x2D7;
    private const int OffGorgonEyes  = 0x303;
    private const int OffPhoenixFea  = 0x307;
    private const int OffSubwepAmmo  = 0x317;

    // --- KPTR2-relative offsets ---
    private const int Off2Difficulty = 0x17;
    private const int Off2NGPlus     = 0x1B;

    // --- Patterns ---
    private static readonly byte[] KptrPattern  = { 0x00, 0x00, 0x03, 0x24 };
    private static readonly byte[] Kptr2Pattern = { 0x04, 0xFF, 0x00, 0x00 };

    // --- State ---
    public byte[] Bytes { get; private set; } = [];
    public string? FolderPath { get; set; }
    public int Channel { get; set; } = 1;
    public bool Modified { get; set; }
    public bool IsLoaded { get; private set; }
    public bool IsBigEndian { get; } = true;

    public int KPtr { get; private set; }
    public int KPtr2 { get; private set; }

    // --- Character fields ---
    public float Health { get; set; }
    public float HealthRegen { get; set; }
    public float Magic { get; set; }
    public float MagicRegen { get; set; }
    public float Rage { get; set; }
    public float RageRegen { get; set; }
    public int RedOrbs { get; set; }
    public float XPos { get; set; }
    public float YPos { get; set; }
    public float ZPos { get; set; }
    public bool Swimming { get; set; }
    public bool Frozen { get; set; }
    public int GorgonEyes { get; set; }
    public int PhoenixFeathers { get; set; }
    public int SubWeaponAmmo { get; set; }
    public int Difficulty { get; set; }
    public bool NewGamePlus { get; set; }

    // --- Weapon fields ---
    public int[] WeaponLevels { get; set; } = [1, 1, 1, 1, 1, 1, 1, 1];
    public int[] WeaponOrbs { get; set; } = [0, 0, 0, 0, 0, 0, 0, 0];
    public bool[] WeaponUnlocked { get; set; } = [false, false, false, false, false, false, false, false];
    public bool[] WeaponSelected { get; set; } = [false, false, false, false, false, false, false, false];

    // --- Sub-weapon ---
    public int SubWeaponIndex { get; set; }
    public bool SubWeaponGlitch { get; set; }

    // --- Costume ---
    public int TorsoIndex { get; set; }
    public int PantsIndex { get; set; }

    // --- Inventory (153 items, index = item ID) ---
    public bool[] Inventory { get; set; } = new bool[0x97];

    public string FileName => $"DAT-{Channel}";

    // --- Byte pattern search ---
    public static int IndexOf(byte[] haystack, byte[] needle)
    {
        if (needle.Length > haystack.Length) return -1;
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < needle.Length; j++)
            {
                if (haystack[i + j] != needle[j]) { found = false; break; }
            }
            if (found) return i;
        }
        return -1;
    }

    // --- Load ---
    public string? Load()
    {
        if (string.IsNullOrEmpty(FolderPath))
            return "No folder selected.";

        string path = Path.Combine(FolderPath, FileName);
        if (!File.Exists(path))
            return $"Slot {Channel}: {FileName} not found in {FolderPath}";

        byte[] data = File.ReadAllBytes(path);

        // VB: "This file's opening bytes are not GoW-standard. Decryption has failed or save slot is empty."
        if (data.Length == 0 || data[0] != 0xC0)
        {
            if (File.Exists(Path.Combine(FolderPath, "PARAM.PFD")))
                return $"Slot {Channel}: {FileName} looks encrypted (first byte 0x{(data.Length > 0 ? data[0] : 0):X2}, " +
                       "expected 0xC0, and PARAM.PFD is present). Saves copied from a real PS3 must be " +
                       "decrypted first, e.g. with Apollo Save Tool.";
            return $"Slot {Channel}: {FileName} is empty or not an Ascension save " +
                   $"(first byte 0x{(data.Length > 0 ? data[0] : 0):X2}, expected 0xC0).";
        }

        int kptr = IndexOf(data, KptrPattern);
        if (kptr == -1)
            return $"Slot {Channel}: could not find the character block (kptr pattern 00 00 03 24).";
        int kptr2 = IndexOf(data, Kptr2Pattern) - 4;
        if (kptr2 < 0)
            return $"Slot {Channel}: could not find the settings block (kptr2 pattern 04 FF 00 00).";

        // Only replace the loaded state once the new file is known to be valid
        Bytes = data;
        ResetFields();

        KPtr = kptr;
        KPtr2 = kptr2;

        // Read character stats
        Health      = ReadSingle(KPtr + OffHealth);
        HealthRegen = ReadSingle(KPtr + OffHealthRegen);
        Magic       = ReadSingle(KPtr + OffMagic);
        MagicRegen  = ReadSingle(KPtr + OffMagicRegen);
        Rage        = ReadSingle(KPtr + OffRage);
        RageRegen   = ReadSingle(KPtr + OffRageRegen);
        RedOrbs     = (int)ReadSingle(KPtr + OffRedOrbs);

        Swimming = Bytes[KPtr + OffSwim] == 0x50;
        Frozen   = Bytes[KPtr + OffFrozen] == 1;

        XPos = (float)Math.Round(ReadSingle(KPtr + OffXPos), 5);
        YPos = (float)Math.Round(ReadSingle(KPtr + OffYPos), 5);
        ZPos = (float)Math.Round(ReadSingle(KPtr + OffZPos), 5);

        // Inventory
        int invCount = Bytes[KPtr + OffInvCount];
        Array.Clear(Inventory, 0, Inventory.Length);
        for (int i = 0; i < invCount && i < 0x97; i++)
        {
            int loc = KPtr + OffInvStart + 16 * i;
            if (loc >= Bytes.Length) break;
            byte itemId = Bytes[loc];
            Inventory[itemId] = true;

            int wIdx = Array.IndexOf(WeaponIds, itemId);
            if (wIdx >= 0)
            {
                WeaponLevels[wIdx] = Bytes[loc + 4] + 1;
                WeaponUnlocked[wIdx] = Bytes[loc + 5] != 0;
                if (wIdx < 5) WeaponSelected[wIdx] = Bytes[loc + 6] != 0;
                WeaponOrbs[wIdx] = ReadInt32(loc + 9);
            }

            int sIdx = Array.IndexOf(SubWeaponIds, itemId);
            if (sIdx >= 0)
                SubWeaponIndex = sIdx + 1;

            int tIdx = Array.IndexOf(TorsoIds, itemId);
            if (tIdx >= 0) TorsoIndex = tIdx + 1;

            int pIdx = Array.IndexOf(PantsIds, itemId);
            if (pIdx >= 0) PantsIndex = pIdx + 1;
        }

        if (SubWeaponIndex > 1)
            SubWeaponGlitch = !Inventory[(SubWeaponIndex - 2) / 3 * 4 + 31];

        GorgonEyes      = Bytes[KPtr + OffGorgonEyes];
        PhoenixFeathers = Bytes[KPtr + OffPhoenixFea];
        SubWeaponAmmo   = Bytes[KPtr + OffSubwepAmmo];

        Difficulty    = Bytes[KPtr2 + Off2Difficulty];
        NewGamePlus   = Bytes[KPtr2 + Off2NGPlus] != 0;

        Modified = false;
        IsLoaded = true;
        return null;
    }

    /// <summary>Mirror of the VB ClearValues(): wipe values left over from the previous slot.</summary>
    private void ResetFields()
    {
        WeaponLevels   = [1, 1, 1, 1, 1, 1, 1, 1];
        WeaponOrbs     = [0, 0, 0, 0, 0, 0, 0, 0];
        WeaponUnlocked = new bool[8];
        WeaponSelected = new bool[8];
        SubWeaponIndex = 0;
        SubWeaponGlitch = false;
        TorsoIndex = 0;
        PantsIndex = 0;
        Swimming = Frozen = NewGamePlus = false;
        Array.Clear(Inventory);
    }

    // --- Save ---
    public string? Save()
    {
        if (!IsLoaded) return "No save loaded.";
        if (string.IsNullOrEmpty(FolderPath)) return "No folder selected.";

        WriteSingle(KPtr + OffHealth, Health);
        WriteSingle(KPtr + OffHealthRegen, HealthRegen);
        WriteSingle(KPtr + OffMagic, Magic);
        WriteSingle(KPtr + OffMagicRegen, MagicRegen);
        WriteSingle(KPtr + OffRage, Rage);
        WriteSingle(KPtr + OffRageRegen, RageRegen);
        WriteSingle(KPtr + OffRedOrbs, RedOrbs);

        WriteSingle(KPtr + OffXPos, XPos);
        WriteSingle(KPtr + OffYPos, YPos);
        WriteSingle(KPtr + OffZPos, ZPos);

        Bytes[KPtr + OffSwim] = Swimming ? (byte)0x50 : (byte)0x08;
        Bytes[KPtr + OffFrozen] = Frozen ? (byte)1 : (byte)0;

        bool noEleSel = true;
        int invCount = 0;

        for (int id = 0; id < Inventory.Length; id++)
        {
            if (!Inventory[id]) continue;

            int loc = KPtr + OffInvStart + 16 * invCount;
            if (loc + 16 > Bytes.Length) break;

            Bytes[loc] = (byte)id;
            Bytes[loc + 5] = 1;
            Bytes[loc + 6] = 1;

            int wIdx = Array.IndexOf(WeaponIds, (byte)id);
            if (wIdx >= 0)
            {
                Bytes[loc + 4] = (byte)(WeaponLevels[wIdx] - 1);
                if (wIdx < 5)
                    Bytes[loc + 6] = WeaponSelected[wIdx] ? (byte)1 : (byte)0;
                else
                    Bytes[loc + 6] = 0;
                int orbs = WeaponOrbs[wIdx];
                Bytes[loc + 0x0B] = (byte)(orbs / 256);
                Bytes[loc + 0x0C] = (byte)(orbs % 256);
            }

            int rageIdx = Array.IndexOf(new byte[] { 0x08, 0x0A, 0x0B, 0x0C }, (byte)id);
            if (rageIdx >= 0)
            {
                if (!WeaponSelected[rageIdx + 1])
                    Bytes[loc + 6] = 0;
                else
                    noEleSel = false;
            }

            if (id == 9 && !noEleSel)
                Bytes[loc + 6] = 0;

            invCount++;
        }

        Bytes[KPtr + OffInvCount] = (byte)invCount;

        Bytes[KPtr + OffGorgonEyes]   = (byte)GorgonEyes;
        Bytes[KPtr + OffPhoenixFea]   = (byte)PhoenixFeathers;
        Bytes[KPtr + OffSubwepAmmo]   = (byte)SubWeaponAmmo;

        Bytes[KPtr2 + Off2Difficulty] = (byte)Difficulty;
        Bytes[KPtr2 + Off2NGPlus]     = NewGamePlus ? (byte)1 : (byte)0;

        string path = Path.Combine(FolderPath, FileName);
        File.WriteAllBytes(path, Bytes);

        Modified = false;
        return null;
    }

    // --- Big-endian helpers ---
        private float ReadSingle(int offset)
    {
        byte[] ba = new byte[4];
        Array.Copy(Bytes, offset, ba, 0, 4);
        if (IsBigEndian) Array.Reverse(ba);
        return BitConverter.ToSingle(ba, 0);
    }

        private void WriteSingle(int offset, float value)
    {
        byte[] ba = BitConverter.GetBytes(value);
        if (IsBigEndian) Array.Reverse(ba);
        ba.CopyTo(Bytes, offset);
    }

        private int ReadInt32(int offset)
    {
        byte[] ba = new byte[4];
        Array.Copy(Bytes, offset, ba, 0, 4);
        if (IsBigEndian) Array.Reverse(ba);
        return BitConverter.ToInt32(ba, 0);
    }
}

