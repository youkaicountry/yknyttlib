namespace YKnyttLib
{
    public class JuniPowers
    {
        public enum PowerNames
        {
            Run = 0,
            Climb = 1,
            DoubleJump = 2,
            HighJump = 3,
            Eye = 4,
            EnemyDetecor = 5,
            Umbrella = 6,
            Hologram = 7,
            RedKey = 8,
            YellowKey = 9,
            BlueKey = 10,
            PurpleKey = 11
        }

        public bool[] Powers { get; }

        public JuniPowers()
        {
            this.Powers = new bool[12];
        }

        public void setPower(int id, bool val) { this.Powers[id] = val; }
        public void setPower(PowerNames name, bool val) { this.Powers[(int)name] = val; }

        public bool getPower(int id) { return this.Powers[id]; }
        public bool getPower(PowerNames name) { return this.Powers[(int)name]; }

        public void writeToSave(KnyttSave save)
        {
            for (int i = 0; i < this.Powers.Length; i++) { save.setPower(i, Powers[i]); }
        }

        public void readFromSave(KnyttSave save)
        {
            for (int i = 0; i < this.Powers.Length; i++) { Powers[i] = save.getPower(i); }
        }
    }
}
