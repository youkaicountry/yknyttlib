using System;
using System.Diagnostics;
using System.IO;
using YKnyttLib;

namespace YKnyttLibConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            KnyttWorld world = new KnyttWorld();
            string world_ini = File.ReadAllText("Nifflas - The Machine/World.ini");
            world.loadWorldConfig(world_ini);
            var fs = File.Open("Nifflas - The Machine/Map.bin", FileMode.Open);
            world.loadWorldMap(fs);
            fs.Close();

            KnyttWorldManager<bool> manager = new KnyttWorldManager<bool>();
            manager.addWorld(world, false);


            var save = new KnyttSave(world, File.ReadAllText("Nifflas - The Machine/DefaultSavegame.ini"), 1);
            save.setPower(3, true);
            save.setArea(new KnyttPoint(123, 456));
            save.setAreaPosition(new KnyttPoint(1, 2));
            save.setPower(13, false);
            Console.WriteLine(save);
            

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0} ms", stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine(world.Info.Name);
            Console.WriteLine(world.Info.Description);
            Console.WriteLine(world.Info.Difficulties[0]);
            Console.WriteLine(world.Info.Difficulties[1]);
            Console.WriteLine(world.getArea(new KnyttPoint(998, 1021)));

            fs.Close();

            fs = File.Open("Fubaka - The Column.knytt.bin", FileMode.Open);
            KnyttBinWorldLoader bworld = new KnyttBinWorldLoader(fs);

            Console.WriteLine(bworld.RootDirectory);

            foreach (var fname in bworld.GetFileNames())
            {
                Console.WriteLine(fname);
            }

            //Console.WriteLine(bworld.GetFile("Ambiance/Ambi22.ogg"));

            Console.ReadKey();
        }
    }
}
