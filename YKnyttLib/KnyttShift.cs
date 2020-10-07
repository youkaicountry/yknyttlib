using IniParser.Model;
using System.Dynamic;

namespace YKnyttLib
{
    public class KnyttShift
    {
        public enum ShiftID
        {
            A = 0,
            B = 1,
            C = 2
        }

        public enum ShiftShape
        {
            SPOT = 0,
            FLOOR = 1,
            CIRCLE = 2,
            SQUARE = 3
        }

        public KnyttPoint AreaPos { get; }
        public KnyttPoint ShiftPos { get; }
        public ShiftID ID { get; }
        public ShiftShape Shape { get; private set; }
        public bool Visible { get; private set; }
        public bool Save { get; private set; }
        public bool Effect { get; private set; }
        public bool OnTouch { get; private set; }
        public bool DenyHologram { get; private set; }
        public bool Quantize { get; private set; }
        public string Sound { get; private set; }
        public string Cutscene { get; private set; }
        public bool StopMusic { get; private set; }
        public int FlagOn { get; private set; }
        public int FlagOff { get; private set; }

        public bool AbsoluteTarget { get; set; }

        public KnyttPoint AbsoluteArea { get; set; }

        public KnyttPoint RelativeArea
        {
            get { return AbsoluteArea - AreaPos; }
            set { AbsoluteArea = AreaPos + value; }
        }

        public KnyttPoint FormattedArea
        {
            get { return AbsoluteTarget ? AbsoluteArea : RelativeArea; }
            set { if (AbsoluteTarget) { AbsoluteArea = value; } else { RelativeArea = value; }  }
        }

        public KnyttPoint AbsolutePosition { get; set; }

        public KnyttPoint RelativePosition
        {
            get { return AbsolutePosition - ShiftPos; }
            set { AbsolutePosition = ShiftPos + value; }
        }

        public KnyttPoint FormattedPosition
        {
            get { return AbsoluteTarget ? AbsolutePosition : RelativePosition; }
            set { if (AbsoluteTarget) { AbsolutePosition = value; } else { RelativePosition = value; } }
        }

        public KnyttShift(KnyttPoint area_pos, KnyttPoint shift_pos, ShiftID id)
        {
            AreaPos = area_pos;
            ShiftPos = shift_pos;
            ID = id;
            AbsoluteTarget = false; // Relative by default
        }

        public KnyttShift(KnyttPoint area_pos, KnyttPoint shift_pos, ShiftID id, KeyDataCollection data) : this(area_pos, shift_pos, id)
        {
            loadFromINI(data);
        }

        public KnyttShift(KnyttArea area, KnyttPoint shift_pos, ShiftID id) : this(area.Position, shift_pos, id, area.ExtraData) { }

        private void loadFromINI(KeyDataCollection data)
        {
            AbsoluteTarget = getBoolINIValue(data, "AbsoluteTarget");
            FormattedArea = new KnyttPoint(getIntINIValue(data, "XMap"), getIntINIValue(data, "YMap"));
            FormattedPosition = new KnyttPoint(getIntINIValue(data, "XPos"), getIntINIValue(data, "YPos"));
            Save = getBoolINIValue(data, "Save", false);
            Effect = getBoolINIValue(data, "Effect", true);
            StopMusic = getBoolINIValue(data, "StopMusic", true);
            Visible = getBoolINIValue(data, "Visible", true);
            OnTouch = getBoolINIValue(data, "Touch", false);
            DenyHologram = getBoolINIValue(data, "DenyHologram", false);
            Quantize = getBoolINIValue(data, "Quantize", true);
            Shape = (ShiftShape)getIntINIValue(data, "Type");
            StopMusic = getBoolINIValue(data, "StopMusic", false);
            Sound = getStringINIValue(data, "Sound");
            Cutscene = getStringINIValue(data, "Cutscene");
            FlagOn = getIntINIValue(data, "FlagOn", -1);
            FlagOff = getIntINIValue(data, "FlagOff", -1);
        }

        private bool getBoolINIValue(KeyDataCollection data, string name, bool @default = false)
        {
            string value = getStringINIValue(data, name);
            if (value == null) { return @default; }
            return value.Equals("True") ? true : false;
        }

        private int getIntINIValue(KeyDataCollection data, string name, int @default = 0)
        {
            string value = getStringINIValue(data, name);
            if (value == null) { return @default; }
            return int.Parse(value);
        }

        private string getStringINIValue(KeyDataCollection data, string name)
        {
            char letter = "ABC"[(int)ID];
            string key = string.Format("Shift{0}({1})", name, letter);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }
    }
}
