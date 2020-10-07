namespace YKnyttLib
{
    public class JuniValues
    {
        public enum PowerNames
        {
            Run = 0,
            Climb = 1,
            DoubleJump = 2,
            HighJump = 3,
            Eye = 4,
            EnemyDetector = 5,
            Umbrella = 6,
            Hologram = 7,
            RedKey = 8,
            YellowKey = 9,
            BlueKey = 10,
            PurpleKey = 11
        }

        public bool[] Powers { get; }
        public bool[] Flags { get; }

        public JuniValues()
        {
            Powers = new bool[12];
            Flags = new bool[10];
        }

        public JuniValues(KnyttSave save) : this()
        {
            readFromSave(save);
        }

        public void setPower(int id, bool val) { this.Powers[id] = val; }
        public void setPower(PowerNames name, bool val) { this.Powers[(int)name] = val; }

        public bool getPower(int id) { return this.Powers[id]; }
        public bool getPower(PowerNames name) { return this.Powers[(int)name]; }

        public void setFlag(int index, bool val) { Flags[index] = val; }
        public bool getFlag(int index) { return Flags[index]; }

        public void writeToSave(KnyttSave save)
        {
            for (int i = 0; i < Powers.Length; i++) { save.setPower(i, Powers[i]); }
            for (int i = 0; i < Flags.Length; i++) { save.setFlag(i, Flags[i]); }
        }

        public void readFromSave(KnyttSave save)
        {
            for (int i = 0; i < Powers.Length; i++) { Powers[i] = save.getPower(i); }
            for (int i = 0; i < Flags.Length; i++) { Flags[i] = save.getFlag(i); }
        }
    }
}
