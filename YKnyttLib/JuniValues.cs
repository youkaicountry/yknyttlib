﻿using System.Linq;
using System.Collections.Generic;

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
            PurpleKey = 11,
            Map = 12
        }

        public bool[] Powers { get; }
        public bool[] Flags { get; }
        public bool[] Collectables { get; private set; }
        public int CoinsSpent { get; set; }
        public HashSet<KnyttPoint> VisitedAreas { get; private set; }

        public class Flag
        {
            public bool power;
            public int number;

            public static Flag Parse(string str)
            {
                if (str == null) { return null; }
                Flag flag = new Flag();
                flag.power = str.StartsWith("Power");
                if (!int.TryParse(flag.power ? str.Substring(5) : str, out flag.number)) { return null; }
                return flag;
            }
        }

        public JuniValues()
        {
            Powers = new bool[13];
            Flags = new bool[10];
            Collectables = new bool[200];
            VisitedAreas = new HashSet<KnyttPoint>();
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

        public bool check(Flag flag)
        {
            return flag.power ? Powers[flag.number] : Flags[flag.number];
        }

        public void setCollectable(int index, bool val) { Collectables[index] = val; }
        public bool getCollectable(int index) { return Collectables[index]; }

        public int getCreaturesCount() { return Collectables.Skip(1).Take(50).Where(a => a).Count(); }
        public int getCoinCount() { return Collectables.Skip(51).Take(100).Where(a => a).Count() - CoinsSpent; }
        public int getArtifactsCount() { return Collectables.Skip(151).Take(49).Where(a => a).Count(); }

        public void writeToSave(KnyttSave save)
        {
            for (int i = 0; i < Powers.Length; i++) { save.setPower(i, Powers[i]); }
            for (int i = 0; i < Flags.Length; i++) { save.setFlag(i, Flags[i]); }
            save.setCollectables(Collectables, CoinsSpent);
            save.setVisitedAreas(VisitedAreas);
        }

        public void readFromSave(KnyttSave save)
        {
            for (int i = 0; i < Powers.Length; i++) { Powers[i] = save.getPower(i); }
            for (int i = 0; i < Flags.Length; i++) { Flags[i] = save.getFlag(i); }
            (Collectables, CoinsSpent) = save.getCollectables();
            VisitedAreas = save.getVisitedAreas();
        }
    }
}