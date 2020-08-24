using System.IO;
using System.Text;

namespace YKnyttLib
{
    public class KnyttArea : KnyttArea<string>
    {
        public KnyttArea(Stream map, KnyttWorld world) : base(map, world) { }
        public KnyttArea(KnyttPoint position, KnyttWorld world) : base(position, world) { }
    }

    public class KnyttArea<OT>
    {
        public const int AREA_WIDTH = 25;
        public const int AREA_HEIGHT = 10;
        public const int AREA_TILE_LAYERS = 4;
        public const int AREA_SPRITE_LAYERS = 4;

        public KnyttWorld<OT> World { get; }

        public KnyttPoint Position { get; protected set; }

        public int TilesetA { get; protected set; }
        public int TilesetB { get; protected set; }

        public int Song { get; protected set; }

        public int AtmosphereA { get; protected set; }
        public int AtmosphereB { get; protected set; }

        public int Background { get; protected set; }

        public bool Empty { get; private set; }

        public TileLayer[] TileLayers { get; protected set; }
        public SpriteLayer[] SpriteLayers { get; protected set; }

        public struct TileLayer
        {
            public byte[] tiles;

            public TileLayer(byte[] tiles)
            {
                this.tiles = tiles;
            }

            public byte getTile(int x, int y)
            {
                return this.tiles[y * AREA_WIDTH + x];
            }
        }

        public struct SpriteLayer
        {
            public byte[] hi;
            public byte[] lo;

            public SpriteLayer(byte[] hi, byte[] lo)
            {
                this.hi = hi;
                this.lo = lo;
            }

            public byte getSpriteHi(int x, int y)
            {
                return this.hi[y * AREA_WIDTH + x];
            }

            public byte getSpriteLo(int x, int y)
            {
                return this.lo[y * AREA_WIDTH + x];
            }
        }

        // Reads the next area in the map stream
        public KnyttArea(Stream map, KnyttWorld<OT> world)
        {
            this.World = world;
            this.loadFromStream(map);
        }

        public KnyttArea(KnyttPoint position, KnyttWorld<OT> world)
        {
            this.World = world;
            this.Empty = true;
            this.Position = position;
        }

        private void loadFromStream(Stream map)
        {
            this.Empty = false;

            this.parseSize(map);

            // Skip misc header stuff
            // TODO: Verify it
            skipBytes(map, 4);

            // Parse layer data
            this.parseTileLayers(map);
            this.parseSpriteLayers(map);

            // Parse tilesets
            this.TilesetB = map.ReadByte();
            this.TilesetA = map.ReadByte();

            // Parse atmosphere
            this.AtmosphereA = map.ReadByte();
            this.AtmosphereB = map.ReadByte();
            this.Song = map.ReadByte();
            this.Background = map.ReadByte();

            //Console.WriteLine(string.Format("Pos: ({0}, {1}), T: ({2}, {3})", this.Position.x, this.Position.y, this.TilesetB, this.TilesetA));
        }

        private void parseSize(Stream map)
        {
            this.Position = new KnyttPoint(parseNumber(map), parseNumber(map));
        }

        private void parseTileLayers(Stream map)
        {
            this.TileLayers = new TileLayer[AREA_TILE_LAYERS];

            for (int i = 0; i < AREA_TILE_LAYERS; i++)
            {
                byte[] tiles = parseByteArray(map, AREA_WIDTH * AREA_HEIGHT);
                this.TileLayers[i] = new TileLayer(tiles);
            }
        }

        private void parseSpriteLayers(Stream map)
        {
            this.SpriteLayers = new SpriteLayer[AREA_SPRITE_LAYERS];

            for (int i = 0; i < AREA_SPRITE_LAYERS; i++)
            {
                byte[] hi = parseByteArray(map, AREA_WIDTH * AREA_HEIGHT);
                byte[] lo = parseByteArray(map, AREA_WIDTH * AREA_HEIGHT);
                this.SpriteLayers[i] = new SpriteLayer(hi, lo);
            }
        }

        private static byte[] parseByteArray(Stream map, int size)
        {
            byte[] data = new byte[size];
            map.Read(data, 0, size);
            return data;
        }

        private static int parseNumber(Stream map)
        {
            var sb = new StringBuilder();
            char c = (char)map.ReadByte();
            while (c >= 48 && c < 58)
            {
                sb.Append(c);
                c = (char)map.ReadByte();
            }

            return int.Parse(sb.ToString());
        }

        private static void skipBytes(Stream map, int size)
        {
            for (int i = 0; i < size; i++)
            {
                map.ReadByte();
            }
        }

        public override string ToString()
        {
            return string.Format("[Area {0}]", this.Position);
        }
    }
}
