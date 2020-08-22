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

            KnyttWorld<bool> world = new KnyttWorld<bool>();
            string world_ini = File.ReadAllText("Nifflas - The Machine/World.ini");
            world.loadWorldConfig(world_ini);
            var fs = File.Open("Nifflas - The Machine/Map.bin", FileMode.Open);
            world.loadWorldMap(fs);
            fs.Close();
            

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0} ms", stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine(world.Info.Name);
            Console.WriteLine(world.Info.Description);
            Console.WriteLine(world.Info.Difficulties[0]);
            Console.WriteLine(world.Info.Difficulties[1]);
            Console.WriteLine(world.getArea(new KnyttPoint(998, 1021)));

            Console.ReadKey();
        }
    }
}
