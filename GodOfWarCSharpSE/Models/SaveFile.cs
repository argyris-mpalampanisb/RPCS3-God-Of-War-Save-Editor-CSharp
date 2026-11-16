using System.Buffers.Binary;
using System;

namespace GodOfWarCSharpSE.Models;

/// <summary>
/// Endian-aware byte read/write helpers (replaces GoWFuncs).
/// </summary>
public static class SaveFile
{
    public static short ReadInt16(ReadOnlySpan<byte> data, int offset, bool bigEndian)
    {
        var span = data.Slice(offset, 2);
        return bigEndian
            ? BinaryPrimitives.ReadInt16BigEndian(span)
            : BinaryPrimitives.ReadInt16LittleEndian(span);
    }

    public static int ReadInt32(ReadOnlySpan<byte> data, int offset, bool bigEndian)
    {
        var span = data.Slice(offset, 4);
        return bigEndian
            ? BinaryPrimitives.ReadInt32BigEndian(span)
            : BinaryPrimitives.ReadInt32LittleEndian(span);
    }

    public static uint ReadUInt32(ReadOnlySpan<byte> data, int offset, bool bigEndian)
    {
        var span = data.Slice(offset, 4);
        return bigEndian
            ? BinaryPrimitives.ReadUInt32BigEndian(span)
            : BinaryPrimitives.ReadUInt32LittleEndian(span);
    }

    public static ushort ReadUInt16(ReadOnlySpan<byte> data, int offset, bool bigEndian)
    {
        var span = data.Slice(offset, 2);
        return bigEndian
            ? BinaryPrimitives.ReadUInt16BigEndian(span)
            : BinaryPrimitives.ReadUInt16LittleEndian(span);
    }

    public static float ReadSingle(ReadOnlySpan<byte> data, int offset, bool bigEndian)
    {
        var span = data.Slice(offset, 4);
        uint bits = bigEndian
            ? BinaryPrimitives.ReadUInt32BigEndian(span)
            : BinaryPrimitives.ReadUInt32LittleEndian(span);
        return BitConverter.Int32BitsToSingle((int)bits);
    }

    public static string ReadAscii(ReadOnlySpan<byte> data, int offset, int maxLen = 0x100)
    {
        int len = Math.Min(maxLen, data.Length - offset);
        int nullIdx = data.Slice(offset, len).IndexOf((byte)0);
        if (nullIdx >= 0) len = nullIdx;
        return System.Text.Encoding.ASCII.GetString(data.Slice(offset, len));
    }

    public static void WriteInt16(Span<byte> data, int offset, short value, bool bigEndian)
    {
        if (bigEndian) BinaryPrimitives.WriteInt16BigEndian(data.Slice(offset), value);
        else BinaryPrimitives.WriteInt16LittleEndian(data.Slice(offset), value);
    }

    public static void WriteInt32(Span<byte> data, int offset, int value, bool bigEndian)
    {
        if (bigEndian) BinaryPrimitives.WriteInt32BigEndian(data.Slice(offset), value);
        else BinaryPrimitives.WriteInt32LittleEndian(data.Slice(offset), value);
    }

    public static void WriteUInt32(Span<byte> data, int offset, uint value, bool bigEndian)
    {
        if (bigEndian) BinaryPrimitives.WriteUInt32BigEndian(data.Slice(offset), value);
        else BinaryPrimitives.WriteUInt32LittleEndian(data.Slice(offset), value);
    }

    public static void WriteUInt16(Span<byte> data, int offset, ushort value, bool bigEndian)
    {
        if (bigEndian) BinaryPrimitives.WriteUInt16BigEndian(data.Slice(offset), value);
        else BinaryPrimitives.WriteUInt16LittleEndian(data.Slice(offset), value);
    }

    public static void WriteSingle(Span<byte> data, int offset, float value, bool bigEndian)
    {
        uint bits = (uint)BitConverter.SingleToInt32Bits(value);
        if (bigEndian) BinaryPrimitives.WriteUInt32BigEndian(data.Slice(offset), bits);
        else BinaryPrimitives.WriteUInt32LittleEndian(data.Slice(offset), bits);
    }

    public static void WriteAscii(Span<byte> data, int offset, string text, int maxLen)
    {
        var bytes = System.Text.Encoding.ASCII.GetBytes(text);
        int len = Math.Min(bytes.Length, maxLen);
        data.Slice(offset, maxLen).Clear();
        bytes.AsSpan(0, len).CopyTo(data.Slice(offset, len));
    }

    /// <summary>
    /// GoW1 checksum: sum of each byte weighted by 16^(2*(3 - i%4)).
    /// Equivalent to summing the file as consecutive big-endian uint32s.
    /// </summary>
    public static uint ComputeChecksum(ReadOnlySpan<byte> data)
    {
        ulong checksum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            ulong power = 16UL << (4 * (3 - (i % 4)));
            checksum += (ulong)data[i] * power;
        }
        return (uint)(checksum & 0xFFFFFFFFUL);
    }

    /// <summary>
    /// GoW1 movie-name checksum (uppercase, initial value 0).
    /// </summary>
    public static uint ComputeMovieChecksum(string name)
    {
        ulong checksum = 0;
        foreach (char c in name)
        {
            uint ascii = (uint)c;
            if (ascii >= 97 && ascii <= 122) ascii -= 32;
            checksum = checksum * 127 + ascii;
        }
        return (uint)(checksum & 0xFFFFFFFFUL);
    }
}