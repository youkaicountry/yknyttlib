using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using YKnyttLib;
using YKnyttLib.Parser;
using YUtil.BinaryTools.Base58;

namespace YKnyttLibConsole
{
    /*class TestKnyttWorld : KnyttWorld
    {
        protected override object bytesToSound(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected override object bytesToTexture(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected override bool externalFileExists(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override object getExternalSound(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override object getExternalTexture(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override byte[] getExternalWorldData(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override object getSystemSound(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override object getSystemTexture(string filepath)
        {
            throw new NotImplementedException();
        }

        protected override byte[] getSystemWorldData(string filepath)
        {
            throw new NotImplementedException();
        }
    }*/

    class Program
    {
        public static readonly byte[] KEY = new byte[] { 5, 127, 216, 221, 151, 80, 239, 69, 153, 45, 117, 118, 209, 205, 224, 104 };
        public static readonly byte[] IV = new byte[] { 155, 92, 91, 252, 250, 25, 67, 146, 189, 240, 118, 253, 87, 191, 227, 161 };

        static void Main(string[] args)
        {
            /*KnyttWorld world = new KnyttWorld();
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
            
            Console.WriteLine(world.Info.Name);
            Console.WriteLine(world.Info.Description);
            Console.WriteLine(world.Info.Difficulties[0]);
            Console.WriteLine(world.Info.Difficulties[1]);
            Console.WriteLine(world.getArea(new KnyttPoint(998, 1021)));

            fs.Close();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            fs = File.Open("Fubaka - The Column.knytt.bin", FileMode.Open);
            KnyttBinWorldLoader bworld = new KnyttBinWorldLoader(fs);

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0} ms", stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine(bworld.RootDirectory);

            foreach (var fname in bworld.GetFileNames())
            {
                Console.WriteLine(fname);
            }*/

            /*KnyttWorld world = new TestKnyttWorld();
            string world_ini = File.ReadAllText("Nifflas - The Machine/World.ini");
            world.loadWorldConfig(world_ini);
            world.setDirectory("...", "Nifflas - Tutocdd2342342131235rial");

            KnyttSave save = new KnyttSave(world);
            save.setArea(new KnyttPoint(1007, 990));
            save.setAreaPosition(new KnyttPoint(12, 7));
            save.setPower(1, true);
            save.setPower(2, true);
            save.setPower(3, true);
            save.setPower(11, true);
            save.setFlag(3, true);

            Console.WriteLine(save);

            var password = save.ToPassword();
            Console.WriteLine(password);

            var save2 = KnyttSave.FromPassword(world, password);
            Console.WriteLine(save2);

            Console.ReadKey();*/

            CommandSet cs = new CommandSet();
            cs.AddCommand(new CommandDeclaration("test", "test desc", false, null, new CommandArg("boolarg", CommandArg.Type.BoolArg), new CommandArg("stringarg", CommandArg.Type.StringArg)));

            var p = new CommandParser(cs);
            var r = p.Parse("test true \"booppppppppp\"");
            Console.ReadKey();
        }

        private static byte[] EncryptBytes(IEnumerable<byte> bytes)
        {
            //The ICryptoTransform is created for each call to this method as the MSDN documentation indicates that the public methods may not be thread-safe and so we cannot hold a static reference to an instance
            using (var r = Rijndael.Create())
            {
                using (var encryptor = r.CreateEncryptor(KEY, IV))
                {
                    return Transform(bytes, encryptor);
                }
            }
        }

        private static byte[] DecryptBytes(IEnumerable<byte> bytes)
        {
            //The ICryptoTransform is created for each call to this method as the MSDN documentation indicates that the public methods may not be thread-safe and so we cannot hold a static reference to an instance
            using (var r = Rijndael.Create())
            {
                using (var decryptor = r.CreateDecryptor(KEY, IV))
                {
                    return Transform(bytes, decryptor);
                }
            }
        }

        private static byte[] Transform(IEnumerable<byte> bytes, ICryptoTransform transform)
        {
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    foreach (var b in bytes)
                        cryptoStream.WriteByte(b);
                }

                return stream.ToArray();
            }
        }
    }
}
