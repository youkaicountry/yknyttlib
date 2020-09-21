using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YKnyttLib
{
    public class KnyttWorldManager<T>
    {
        public struct WorldEntry
        {
            public KnyttWorld world;
            public T extra_data;

            public WorldEntry(KnyttWorld world, T extra_data)
            {
                this.world = world;
                this.extra_data = extra_data;
            }
        }

        public List<WorldEntry> Filtered 
        { 
            get
            {
                var result = new List<WorldEntry>();

                foreach (var entry in Entries)
                {
                    if (!checkEntry(entry)) { continue; }
                    result.Add(entry);
                }

                return result;
            }
        }

        private string category;
        private string difficulty;
        private string size;

        List<WorldEntry> Entries { get; }

        public KnyttWorldManager()
        {
            this.Entries = new List<WorldEntry>();
        }

        // TODO: Add indexing by categories

        public bool addWorld(KnyttWorld world, T extra_data)
        {
            return this.addWorld(new WorldEntry(world, extra_data));
        }

        public bool addWorld(WorldEntry entry)
        {
            if (entry.world.Info == null) { throw new SystemException("Must load world config first."); }

            this.Entries.Add(entry);
            return checkEntry(entry);
        }

        public void setFilter(string category, string difficulty, string size)
        {
            this.category = category;
            this.difficulty = difficulty;
            this.size = size;
        }

        public bool checkEntry(WorldEntry entry)
        {
            var info = entry.world.Info;
            if (category != null && !info.Categories.Contains(category)) { return false; }
            if (difficulty != null && !info.Difficulties.Contains(difficulty)) { return false; }
            if (size != null && !info.Size.Equals(size)) { return false; }
            return true;
        }
    }
}
