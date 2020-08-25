using IniParser.Model;
using IniParser.Parser;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace YKnyttLib
{
    public class KnyttWorld
    {
        public KnyttWorldInfo Info { get; private set; }

        public KnyttPoint MinBounds { get; protected set; }
        public KnyttPoint MaxBounds { get; protected set; }

        public KnyttPoint Size { get; protected set; }

        public List<KnyttArea> Areas { get; protected set; }
        public KnyttArea[] Map { get; protected set; }

        private IniData INIData { get; set; }

        public string WorldDirectory { get; private set; }
        public string WorldDirectoryName { get; private set; }

        public bool BinFile { get; protected set; }

        public KnyttSave CurrentSave { get; set; }

        public KnyttWorld()
        {
            this.MinBounds = new KnyttPoint(int.MaxValue, int.MaxValue);
            this.MaxBounds = new KnyttPoint(int.MinValue, int.MinValue);
        }

        public void loadWorldConfig(string world_ini)
        {
            var parser = new IniDataParser();
            this.INIData = parser.Parse(world_ini);
            this.Info = new KnyttWorldInfo(INIData["World"]);
        }

        public void loadWorldMap(Stream map)
        {
            GZipStream gz = new GZipStream(map, CompressionMode.Decompress);

            var areas = new List<KnyttArea>();

            // Area definition starts with an 'x' character
            while (gz.ReadByte() == 'x')
            {
                var area = new KnyttArea(gz, this);
                areas.Add(area);

                this.MinBounds = this.MinBounds.min(area.Position);
                this.MaxBounds = this.MaxBounds.max(area.Position);
            }

            // Set the map
            this.Size = new KnyttPoint((MaxBounds.x - MinBounds.x) + 1, (MaxBounds.y - MinBounds.y) + 1);
            this.Map = new KnyttArea[this.Size.Area];

            foreach (var area in areas)
            {
                this.Map[getMapIndex(area.Position)] = area;
            }
        }

        public byte[] getWorldFile(string filepath)
        {
            if (BinFile) { return null; } // TODO: This should handle getting the bin file

            var data = getExternalWorldFile(filepath);
            return data == null ? getSystemWorldFile(filepath) : data;
        }

        protected virtual byte[] getExternalWorldFile(string filepath)
        {
            return null;
        }

        protected virtual byte[] getSystemWorldFile(string filepath)
        {
            return null;
        }

        // TODO: This logic needs refactoring when things are fleshed out
        public KnyttArea getArea(KnyttPoint coords)
        {
            // If outside of bounds, return null
            if (coords.x < MinBounds.x || coords.x > MaxBounds.x || coords.y < MinBounds.y || coords.y > MaxBounds.y) { return null; }

            var i = getMapIndex(coords);

            // If there is no area stored at the location, create and return an empty area
            if (this.Map[i] == null) { return new KnyttArea(coords, this); }
            return this.Map[getMapIndex(coords)];
        }

        public int getMapIndex(KnyttPoint coords)
        {
            return (coords.y - MinBounds.y) * Size.x + (coords.x - MinBounds.x);
        }

        public void setDirectory(string full_dir, string dir_name)
        {
            this.WorldDirectory = full_dir;
            this.WorldDirectoryName = dir_name;
        }
    }
}
