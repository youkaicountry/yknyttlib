using IniParser.Model;
using IniParser.Parser;

namespace YKnyttLib
{
    public class KnyttSave : KnyttSave<string>
    {
        public KnyttSave(KnyttWorld world, string ini_data, int slot) : base(world, ini_data, slot) { }
    }

    public class KnyttSave<OT>
    {
        public KnyttWorld<OT> World { get; }

        private IniData data;
        public int Slot { get; }

        public string SaveFileName { get { return string.Format("{0} {1}.ini", World.WorldDirectoryName, Slot); } }

        public KnyttSave(KnyttWorld<OT> world, string ini_data, int slot)
        {
            this.World = world;
            this.Slot = slot;
            var parser = new IniDataParser();
            this.data = parser.Parse(ini_data);
        }

        public bool getPower(int power_id)
        {
            var presult = data["Powers"][string.Format("Power{0}", power_id)];
            return int.Parse(presult) == 0 ? false : true;
        }

        public void setPower(int power_id, bool value)
        {
            data["Powers"][string.Format("Power{0}", power_id)] = value ? "1" : "0";
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

        public override string ToString()
        {
            return data.ToString();
        }
    }
}
