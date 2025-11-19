using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace TaskList.Backend;

public static class CombGuid
{
    /// <summary>
    /// Byte order SQL Server uses when comparing <see cref="Guid"/> values.
    /// </summary>
    private static readonly int[] SqlOrder = { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };

    private static readonly object Lock = new();

    // Per-process counter to break ties within the same millisecond
    private static long _lastMilliseconds;
    private static uint _sequence;

    /// <summary>
    /// Generates a COMB GUID (Combined GUID/Timestamp) that maintains chronological order across systems while
    /// remaining globally unique.
    /// </summary>
    /// <remarks>
    /// <para>
    /// THIS FUNCTION IS DUPLICATED IN TetraTech.CNU.Database.ClrFunctions
    /// </para>
    /// <para>
    /// This function creates a 128-bit <see cref="Guid"/> where the bytes that SQL Server compares first (based
    /// on its internal byte ordering) contain a UTC timestamp in milliseconds since the Unix epoch. The remaining
    /// bytes are random, ensuring global uniqueness.
    /// </para>
    /// <para>
    /// This method is designed for use as a SQL CLR scalar function and can safely be registered in SQL Server
    /// using <c>PERMISSION_SET = SAFE</c>. It may also be used directly from .NET applications.
    /// </para>
    /// <para>
    /// The resulting values are roughly time-ordered, which improves clustered index insert performance compared
    /// to purely random GUIDs. However, ordering across systems assumes reasonably synchronized clocks.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A <see cref="SqlGuid"/> representing a COMB GUID value suitable for use as a SQL Server
    /// <c>uniqueidentifier</c> primary key or default column value.
    /// </returns>
    public static Guid NewGuid()
    {
        // 1) Get current UTC ms and per-ms sequence (thread-safe)
        ulong milliseconds;
        uint localSeq;
        var nowMilliseconds = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        lock (Lock)
        {
            if ((ulong)_lastMilliseconds == nowMilliseconds)
                _sequence++;
            else
            {
                _lastMilliseconds = (long)nowMilliseconds;
                _sequence = 0;
            }
            milliseconds = nowMilliseconds;
            localSeq = _sequence;
        }

        // 2) Build 8-byte big-endian timestamp
        var timestamp = new byte[8];
        for (int i = 7; i >= 0; i--)
        {
            timestamp[i] = (byte)(milliseconds & 0xFF);
            milliseconds >>= 8;
        }

        // 3) Create 16 random bytes
        Span<byte> bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);

        // 4) Overlay timestamp bytes into SQL Server compare order
        for (int i = 0; i < 8; i++)
        {
            bytes[SqlOrder[i]] = timestamp[i];
        }

        // 5) Tie-breaker nibble in the last-compared byte (byte 15)
        bytes[15] = (byte)((byte)(bytes[15] & 0xF0) | (byte)(localSeq & 0x0F));

        return new Guid(bytes);
    }
}