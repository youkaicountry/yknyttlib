using System;
using System.Collections.Generic;

namespace YKnyttLib
{
    public class KnyttWorldManager<T> : KnyttWorldManager<string, T>
    {
        public KnyttWorldManager() : base() { }
    }

    public class KnyttWorldManager<OT, T>
    {
        public struct WorldEntry
        {
            public KnyttWorld<OT> world;
            public T extra_data;

            public WorldEntry(KnyttWorld<OT> world, T extra_data)
            {
                this.world = world;
                this.extra_data = extra_data;
            }
        }

        List<WorldEntry> Entries { get; }

        public KnyttWorldManager()
        {
            this.Entries = new List<WorldEntry>();
        }

        // TODO: Add indexing by categories

        public void addWorld(KnyttWorld<OT> world, T extra_data)
        {
            if (world.Info == null) { throw new SystemException("Must load world config first."); }

            this.Entries.Add(new WorldEntry(world, extra_data));
        }

        // TODO: O(n), we can do better by keeping indices as worlds are added
        public List<WorldEntry> filter(string category, string difficulty, string size)
        {
            var result = new List<WorldEntry>();

            foreach(var entry in Entries)
            {
                var info = entry.world.Info;
                if (category != null && !info.Categories.Contains(category)) { continue; }
                if (difficulty != null && !info.Difficulties.Contains(difficulty)) { continue; }
                if (size != null && !info.Size.Equals(size)) { continue; }
                result.Add(entry);
            }

            return result;
        }
    }
}
