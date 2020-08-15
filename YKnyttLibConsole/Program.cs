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

            var fs = File.Open("Nifflas - The Machine/Map.bin", FileMode.Open);
            KnyttWorld<bool> world = new KnyttWorld<bool>(fs, null);

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0} ms", stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine(world.getArea(new KnyttPoint(998, 1021)));

            Console.ReadKey();
        }
    }
}
