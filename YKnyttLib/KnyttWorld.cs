using IniParser.Model;
using IniParser.Parser;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace YKnyttLib
{
    public class KnyttWorld<OT>
    {
        public KnyttWorldInfo Info { get; private set; }

        public KnyttPoint MinBounds { get; protected set; }
        public KnyttPoint MaxBounds { get; protected set; }

        public KnyttPoint Size { get; protected set; }

        public List<KnyttArea<OT>> Areas { get; protected set; }
        public KnyttArea<OT>[] Map { get; protected set; }
        public KnyttPoint StartCoord { get; set; }

        private IniData INIData { get; set; }

        public const int ASSET_LIMIT = 256;
        public OT[] TilesetsOverride { get; }
        public OT[] AmbianceOverride { get; }
        public OT[] GradientsOverride { get; }
        public OT[] MusicOverride { get; }
        public OT[] ObjectsOverride { get; }

        public KnyttWorld()
        {
            this.TilesetsOverride = new OT[ASSET_LIMIT];
            this.AmbianceOverride = new OT[ASSET_LIMIT];
            this.GradientsOverride = new OT[ASSET_LIMIT];
            this.MusicOverride = new OT[ASSET_LIMIT];
            this.ObjectsOverride = new OT[ASSET_LIMIT];

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

            var areas = new List<KnyttArea<OT>>();

            // Area definition starts with an 'x' character
            while (gz.ReadByte() == 'x')
            {
                var area = new KnyttArea<OT>(gz, this);
                areas.Add(area);

                this.MinBounds = this.MinBounds.min(area.Position);
                this.MaxBounds = this.MaxBounds.max(area.Position);
            }

            // Set the map
            this.Size = new KnyttPoint((MaxBounds.x - MinBounds.x) + 1, (MaxBounds.y - MinBounds.y) + 1);
            this.StartCoord = areas[0].Position; // TODO: Fix this to find actual world start
            this.Map = new KnyttArea<OT>[this.Size.Area];

            foreach (var area in areas)
            {
                this.Map[getMapIndex(area.Position)] = area;
            }
        }

        // TODO: This logic needs refactoring when things are fleshed out
        public KnyttArea<OT> getArea(KnyttPoint coords)
        {
            // If outside of bounds, return null
            if (coords.x < MinBounds.x || coords.x > MaxBounds.x || coords.y < MinBounds.y || coords.y > MaxBounds.y) { return null; }

            var i = getMapIndex(coords);

            // If there is no area stored at the location, create and return an empty area
            if (this.Map[i] == null) { return new KnyttArea<OT>(coords, this); }
            return this.Map[getMapIndex(coords)];
        }

        public int getMapIndex(KnyttPoint coords)
        {
            return (coords.y - MinBounds.y) * Size.x + (coords.x - MinBounds.x);
        }
    }
}
