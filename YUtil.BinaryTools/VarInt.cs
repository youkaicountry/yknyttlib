using System;

namespace YUtil.BinaryTools
{
    public struct VarInt
    {
        public long Value { get; }
        public byte[] Bytes { get; }
        public int Length { get { return Bytes.Length; } }

        public VarInt(long val)
        {
            Value = val;
            Bytes = LongToBytes(val);
        }

        public VarInt(byte[] data, ref int position) : this(data, position)
        {
            position += Length;
        }

        public VarInt(byte[] data, int position)
        {
            int start = position;
            Value = BytesToLong(data, ref position);
            var length = position - start;
            var buffer = new byte[position - start];
            Buffer.BlockCopy(data, start, buffer, 0, length);
            Bytes = buffer;
        }

        public VarInt(byte[] data) : this(data, 0) { }

        public static long BytesToLong(byte[] data, ref int position)
        {
            var zz = UVarInt.BytesToULong(data, ref position);
            return decodeZigZag(zz);
        }

        public static byte[] LongToBytes(long value)
        {
            var buffer = new byte[10];
            var length = LongToBytes(value, buffer);
            var result = new byte[length];
            Buffer.BlockCopy(buffer, 0, result, 0, length);

            return result;
        }

        // Fills the given byte buffer from start position, and returns length
        public static int LongToBytes(long value, byte[] buffer, int start = 0)
        {
            var zz = encodeZigZag(value, 64);
            return UVarInt.ULongToBytes((ulong)zz, buffer, start);
        }

        public static long encodeZigZag(long value, int bits)
        {
            return (value << 1) ^ (value >> (bits - 1));
        }

        public static long decodeZigZag(ulong value)
        {
            if ((value & 0x1) == 0x1)
            {
                return (-1 * ((long)(value >> 1) + 1));
            }

            return (long)(value >> 1);
        }
    }
}
