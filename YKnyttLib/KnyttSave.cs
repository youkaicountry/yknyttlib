using IniParser.Model;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using YUtil.BinaryTools;
using YUtil.BinaryTools.Base58;

namespace YKnyttLib
{
    public class KnyttSave
    {
        private const int HASH_BYTES = 4;
        private const int SLIM_HASH_BYTES = 2;

        public KnyttWorld World { get; }

        private IniData data;
        public int Slot { get; }

        public string SaveFileName { get { return string.Format("{0} {1}.ini", World.WorldDirectoryName, Slot); } }

        public KnyttSave(KnyttWorld world, string ini_data, int slot)
        {
            this.World = world;
            this.Slot = slot;
            var parser = new IniDataParser();
            this.data = parser.Parse(ini_data);
            this.setWorldDirectory(World.WorldDirectoryName);
        }

        public KnyttSave(KnyttWorld world, int slot = 0)
        {
            World = world;
            Slot = slot;
            data = new IniData();
            setWorldDirectory(World.WorldDirectoryName);
            setArea(new KnyttPoint(1000, 1000));
            setAreaPosition(new KnyttPoint(6, 6));
        }

        public bool getPower(int power_id)
        {
            var presult = getValue("Powers", string.Format("Power{0}", power_id));
            return presult == null ? false : (int.Parse(presult) == 0 ? false : true);
        }

        public void setPower(int power_id, bool value)
        {
            setValue("Powers", string.Format("Power{0}", power_id), value ? "1" : "0");
        }

        public void setFlag(int flag_id, bool value)
        {
            setValue("Flags", string.Format("Flag{0}", flag_id), value ? "1" : "0");
        }

        public bool getFlag(int flag_id)
        {
            var fresult = getValue("Flags", string.Format("Flag{0}", flag_id));
            return fresult == null ? false : (int.Parse(fresult) == 0 ? false : true);
        }

        public KnyttPoint getArea()
        {
            return new KnyttPoint(int.Parse(data["Positions"]["X Map"]), int.Parse(data["Positions"]["Y Map"]));
        }

        public void setArea(KnyttPoint location)
        {
            data["Positions"]["X Map"] = location.x.ToString();
            data["Positions"]["Y Map"] = location.y.ToString();
        }

        public KnyttPoint getAreaPosition()
        {
            return new KnyttPoint(int.Parse(data["Positions"]["X Pos"]), int.Parse(data["Positions"]["Y Pos"]));
        }

        public void setAreaPosition(KnyttPoint location)
        {
            data["Positions"]["X Pos"] = location.x.ToString();
            data["Positions"]["Y Pos"] = location.y.ToString();
        }

        public void setWorldDirectory(string dir)
        {
            setValue("World", "World Folder", dir);
        }

        public string getWorldDirectory()
        {
            return getValue("World", "World Folder");
        }

        private string getValue(string section, string key)
        {
            if (!data.Sections.ContainsSection(section)) { return null; }
            if (!data[section].ContainsKey(key)) { return null; }
            return data[section][key];
        }

        private void setValue(string section, string key, string value)
        {
            if (!data.Sections.ContainsSection(section)) { data.Sections.AddSection(section); }
            data[section][key] = value;
        }

        public override string ToString()
        {
            return data.ToString();
        }

        public byte[] ToBinary(bool slim = false)
        {
            List<byte> result = new List<byte>();
            if (!slim)
            {
                result.Add((byte)'Y');
                result.Add((byte)'K');
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(getWorldDirectory()));
                for (int i = 0; i < (slim ? SLIM_HASH_BYTES : HASH_BYTES); i++)
                {
                    result.Add(hash[i]);
                }
            }
            
            var area = getArea();
            result.AddRange(new VarInt(area.x-1000).Bytes);
            result.AddRange(new VarInt(area.y-1000).Bytes);

            int bitpack = 0;

            var pos = getAreaPosition();
            bitpack |= pos.x & 0x1F;
            bitpack |= (pos.y & 0xF) << 5;

            JuniValues values = new JuniValues(this);
            bitpack = BinaryTools.writeBoolArray(values.Powers, bitpack, 9);
            bitpack = BinaryTools.writeBoolArray(values.Flags, bitpack, 21);

            result.AddRange(new VarInt(bitpack).Bytes);

            return result.ToArray();
        }

        public static KnyttSave FromBinary(KnyttWorld world, byte[] data, bool slim = false)
        {
            var save = new KnyttSave(world);

            int bposition = 0;

            if (!slim)
            {
                var y = data[bposition++];
                var k = data[bposition++];
                if (y != 'Y' || k != 'K') { throw new SystemException("Magic number YK not present"); }
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(save.getWorldDirectory()));
                for (int i = 0; i < (slim ? SLIM_HASH_BYTES : HASH_BYTES); i++)
                {
                    if (!(hash[i] == data[bposition++])) { throw new SystemException("World name hash doesn't match."); }
                }
            }

            save.setArea(new KnyttPoint((int)new VarInt(data, ref bposition).Value + 1000,
                                        (int)new VarInt(data, ref bposition).Value + 1000));

            int bitpack = (int)new VarInt(data, ref bposition).Value;
            save.setAreaPosition(new KnyttPoint(bitpack & 0x1F, (bitpack & (0xF << 5)) >> 5));

            JuniValues values = new JuniValues();

            BinaryTools.readBoolArray(bitpack, values.Powers, 9);
            BinaryTools.readBoolArray(bitpack, values.Flags, 21);
            values.writeToSave(save);

            return save;
        }

        public string ToPassword()
        {
            var bytes = ToBinary(slim:true);
            return Base58.EncodePlain(bytes);
        }

        public static KnyttSave FromPassword(KnyttWorld world, string password)
        {
            var bytes = Base58.DecodePlain(password);
            return FromBinary(world, bytes, slim:true);
        }
    }
}
