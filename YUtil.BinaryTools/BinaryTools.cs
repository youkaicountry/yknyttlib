using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace YUtil.BinaryTools
{
    public static class BinaryTools
    {
        public static string readString(byte[] buffer, ref int index)
        {
            ushort string_size = readBEU16(buffer, ref index);
            string s = Encoding.UTF8.GetString(buffer, index, string_size);

            index += (int)string_size;

            return s;
        }

        public static string readLongString(byte[] buffer, ref int index)
        {
            uint string_size = readBEU32(buffer, ref index);
            string s = Encoding.UTF8.GetString(buffer, index, (int)string_size);

            index += (int)string_size;

            return s;
        }

        public static byte read8(byte[] array, ref int index)
        {
            byte val = array[index];
            index += 1;
            return val;
        }

        public static ushort readBEU16(byte[] array, ref int index)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(array, index);
            }

            ushort val = BitConverter.ToUInt16(new byte[] { array[index + 1], array[index] }, 0);
            index += 2;
            return val;
        }

        public static uint readBEU32(byte[] array, ref int index)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt32(array, index);
            }

            uint val = BitConverter.ToUInt32(new byte[] { array[index + 3], array[index + 2], array[index + 1], array[index] }, 0);
            index += 4;
            return val;
        }

        public static byte[] readBlob(byte[] array, int length, ref int index)
        {
            byte[] o = array.SubArray(index, length);
            index += length;
            return o;
        }



        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static void write8(Stream stream, byte b)
        {
            stream.WriteByte(b);

            return;
        }

        public static void writeBE16(Stream stream, ushort v)
        {
            byte[] b = BitConverter.GetBytes(v);
            if (!BitConverter.IsLittleEndian)
            {
                stream.Write(b, 0, 2);
            }
            else
            {
                stream.WriteByte(b[1]);
                stream.WriteByte(b[0]);
            }

            return;
        }

        public static void writeBE32(Stream stream, uint v)
        {
            byte[] b = BitConverter.GetBytes(v);
            if (!BitConverter.IsLittleEndian)
            {
                stream.Write(b, 0, 4);
            }
            else
            {
                stream.WriteByte(b[3]);
                stream.WriteByte(b[2]);
                stream.WriteByte(b[1]);
                stream.WriteByte(b[0]);
            }

            return;
        }

        public static void writeBlob(Stream stream, byte[] blob)
        {
            stream.Write(blob, 0, blob.Length);

            return;
        }

        public static void writeString(Stream stream, string s)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);
            writeBE16(stream, (ushort)b.Length);
            stream.Write(b, 0, b.Length);

            return;
        }

        public static void writeLongString(Stream stream, string s)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);
            writeBE32(stream, (uint)b.Length);
            stream.Write(b, 0, b.Length);

            return;
        }

        // Write the given bool array as single bits to the given int
        // Returns the new int
        public static int writeBoolArray(bool[] data, int target, int start_bit)
        {
            for (int i = 0; i < data.Length; i++)
            {
                target |= (data[i] ? 1 : 0) << start_bit++;
            }

            return target;
        }

        public static void readBoolArray(int data, bool[] target, int start_bit)
        {
            for (int i = 0; i < target.Length; i++)
            {
                target[i] = ((data & (1 << start_bit)) >> start_bit++) == 1;
            }
        }

        public static ulong[] packBytesToUInt64(byte[] data)
        {
            List<ulong> result = new List<ulong>();

            for (int i = 0; i < (data.Length / 8) + 1; i++)
            {
                var loc = i * 8;
                var length = (data.Length - loc) < 8 ? data.Length - loc : 8;
                if (length == 0) { break; }
                if (length == 8) 
                {
                    result.Add(BitConverter.ToUInt64(data, loc));
                }
                else
                {
                    byte[] tbuf = new byte[8];
                    Buffer.BlockCopy(data, loc, tbuf, 0, length);
                    result.Add(BitConverter.ToUInt64(tbuf, 0));
                }
            }

            return result.ToArray();
        }
    }
}

