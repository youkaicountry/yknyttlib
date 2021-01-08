using System;

namespace YUtil.BinaryTools
{
    public struct UVarInt
    {
        public ulong Value { get; }
        public byte[] Bytes { get; }
        public int Length { get { return Bytes.Length; } }

        public UVarInt(ulong val)
        {
            Value = val;
            Bytes = ULongToBytes(val);
        }

        public UVarInt(byte[] data, ref int position) : this(data, position)
        {
            position += Length;
        }

        public UVarInt(byte[] data, int position)
        {
            int start = position;
            Value = BytesToULong(data, ref position);
            var length = position - start;
            var buffer = new byte[position - start];
            Buffer.BlockCopy(data, start, buffer, 0, length);
            Bytes = buffer;
        }

        public UVarInt(byte[] data) : this(data, 0) { }

        public static ulong BytesToULong(byte[] data, ref int position)
        {
            int sr = 0;
            ulong result = 0;

            while (position < data.Length) // TODO: Terminate after max length has been reached
            {
                ulong b = data[position++];
                ulong v = b & 0x7f;
                result |= v << sr;

                if ((b & 0x80) != 0x80)
                {
                    return result;
                }

                sr += 7;
            }

            throw new SystemException("Failure to decode varint.");
        }


        public static byte[] ULongToBytes(ulong value)
        {
            var buffer = new byte[10];
            var length = ULongToBytes(value, buffer);
            var result = new byte[length];
            Buffer.BlockCopy(buffer, 0, result, 0, length);

            return result;
        }

        // Fills the given byte buffer from start position, and returns length
        public static int ULongToBytes(ulong value, byte[] buffer, int start = 0)
        {
            int pos = start;

            do
            {
                var b = value & 0x7f;
                value >>= 7;
                if (value != 0) { b |= 0x80; }
                buffer[pos++] = (byte)b;
            }
            while (value != 0);

            return pos - start;
        }
    }
}
