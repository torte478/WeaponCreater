using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

namespace WeaponCeater
{
    class Program
    {
        static void Main()
        {
            // .txt files dataPath
            var basePath = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 9 - 13);
            var swordBladesPath = Path.Combine(basePath, "swordblade.txt");
            var swordHandlesPath = Path.Combine(basePath, "swordhandle.txt");
            var legendarySwordsPath = Path.Combine(basePath, "lgsw.txt");
            var picturesPath = Path.Combine(basePath, "images");

            // lists and variables initialization
            var myBags = new List<Bag>();
            var mySword = new Sword();
            var swordBlades = BaseWeapon.ReadData(() => new SwordBlade(), swordBladesPath, picturesPath);
            var swordHandles = BaseWeapon.ReadData(() => new SwordHandle(), swordHandlesPath, picturesPath);
            var legendarySwords = BaseWeapon.ReadData(() => new LegendarySword(), legendarySwordsPath, picturesPath);

            // get 8 swords 
            HowIGetWeapon.FindChest(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.KillEnemy(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.FindChest(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.KillEnemy(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.FindChest(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.KillEnemy(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.FindChest(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);
            HowIGetWeapon.KillEnemy(mySword, legendarySwords, swordBlades, swordHandles, myBags, basePath);

            // output of the final bag
            Console.WriteLine();
            Console.WriteLine("Your swords:");
            for (var i = 0; i < myBags.Count; i++)
            {
                Console.WriteLine(myBags[i].Name);
            }

            // calculate the total cost of the swords (.SellSword method)
            var totalCost = 0;
            for (var i = 0; i < myBags.Count; i++)
            {
                totalCost += Bag.SellSword(myBags, i);
            }
            Console.WriteLine();
            Console.WriteLine("If you sell all swords you will earn {0} coins.", totalCost);

            // clear CreatedSword directory
            Console.WriteLine("Do you want to delet new pictures?(Y/N)");
            if (Console.ReadLine() == "Y")
            {
                var dirInfo = new DirectoryInfo(basePath+@"CreatedSword");
                foreach (var file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                Console.WriteLine("Check the directory " +basePath+ @"CreatedSword");
            }

            Console.Read();
        }
    }

    // basic class, creates all the characteristics of a sword
    abstract class BaseWeapon
    {
        public Bitmap Picture { get; set; }

        public string Name { get; set; }
        public int Fightspeed { get; set; }
        public int Damage { get; set; }
        public int CriticalHitChance { get; set; }
        public int Value { get; set; }
        public string ImageName { get; set; }
        public string Creator { get; set; }
        public int Level { get; set; }

        public static List<T> ReadData<T>(Func<T> constructWeapon, string dataPath, string picturesPath)
            where T : BaseWeapon
        {
            var swordBlades = new List<T>();

            using (var stream = new StreamReader(dataPath))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    var wordlist = line.Split(' ');

                    var blade = constructWeapon();
                    blade.Name = wordlist[0];
                    blade.Fightspeed = Convert.ToInt32(wordlist[1]);
                    blade.Damage = Convert.ToInt32(wordlist[2]);
                    blade.CriticalHitChance = Convert.ToInt32(wordlist[3]);
                    blade.Value = Convert.ToInt32(wordlist[4]);
                    blade.Creator = wordlist[5];
                    blade.Level = Convert.ToInt32(wordlist[6]);
                    blade.ImageName = wordlist[7];
                    

                    var fileName = string.Format("{0}.bmp", blade.ImageName);
                    var filePath = Path.Combine(picturesPath, fileName);
                    blade.Picture = new Bitmap(filePath);

                    swordBlades.Add(blade);
                }
            }

            return swordBlades;
        }
    }

    class SwordBlade : BaseWeapon
    {
    }

    class SwordHandle : BaseWeapon
    {
    }

    class Sword : BaseWeapon
    {
        public Bitmap Swordpic { get; set; }

        private class CreatorBonus
        {
            public double Damage { get; set; }
            public double FightSpeed { get; set; }
            public int CriticalHitChance { get; set; }
            public double Value { get; set; }

            public CreatorBonus()
            {
                Damage = 1.0;
                FightSpeed = 1.0;
                CriticalHitChance = 0;
                Value = 1.0;
            }

            public void Apply(Sword sword)
            {
                sword.Damage = Convert.ToInt32(sword.Damage*Damage);
                sword.Fightspeed = Convert.ToInt32(sword.Fightspeed * FightSpeed);
                sword.Value = Convert.ToInt32(sword.Value * Value);
                sword.CriticalHitChance = sword.CriticalHitChance + CriticalHitChance;
            }
        }

        private static readonly Dictionary<string, CreatorBonus> BladeBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ Damage = 1.1 } },
            { "eld",   new CreatorBonus{ FightSpeed = 1.15 } },
            { "dwarf", new CreatorBonus{ CriticalHitChance = 5 } },
            { "orc",   new CreatorBonus{ Value = 0.85 } },
            { "daemon",new CreatorBonus{ Damage = 1.40, FightSpeed = 0.90, Value = 1.10 } }
        };

        private static readonly Dictionary<string, CreatorBonus> HandleBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ CriticalHitChance = 5} },
            { "eld",   new CreatorBonus{ Damage = 1.25 } },
            { "dwarf", new CreatorBonus{ Damage = 1.50, Value = 1.30 } },
            { "orc",   new CreatorBonus{ FightSpeed = 1.20} },
            { "daemon",new CreatorBonus{ CriticalHitChance = 10, Damage = 0.90 } }
        }; 

        public static void MakeSword(List<SwordBlade> swordBlades, List<SwordHandle> swordHandles, Sword mySword, string basePath)
        {
            var random = new Random();

            var bladeIndex = random.Next(0, swordBlades.Count - 1);
            var blade = swordBlades[bladeIndex];
            var handleIndex = random.Next(0, swordHandles.Count - 1);
            var handle = swordHandles[handleIndex];

            mySword.Level = (blade.Level + handle.Level)/2;
            mySword.Fightspeed = (blade.Fightspeed + handle.Fightspeed) / 2;
            mySword.Damage = (blade.Damage + handle.Damage) / 2;
            mySword.CriticalHitChance = (blade.CriticalHitChance + handle.CriticalHitChance) / 2;
            mySword.Value = (blade.Value + handle.Value) / 2;
            mySword.Creator = blade.Creator +@"/"+ handle.Creator;
            mySword.Name = blade.Name +" "+ handle.Name;

            mySword.Swordpic = new Bitmap(blade.Picture);
            for (var z = 0; z < handle.Picture.Width; z++)
            {
                for (var t = 400; t < handle.Picture.Height; t++)
                {
                    Color pixelColor = handle.Picture.GetPixel(z, t);
                    mySword.Swordpic.SetPixel(z, t, pixelColor);
                }
            }

            var fileName = string.Format("{0}.bmp", mySword.Name);
            var filePath = Path.Combine(basePath, "CreatedSword", fileName);
            blade.Picture.Save(filePath);

            if (BladeBonuses.ContainsKey(blade.Creator))
            {
                var bonus = BladeBonuses[blade.Creator];
                bonus.Apply(mySword);
            }

            if (HandleBonuses.ContainsKey(handle.Creator))
            {
                var bonus = HandleBonuses[handle.Creator];
                bonus.Apply(mySword);
            }
        }

        public static void AddToBag(Sword mySword, List<Bag> myBag)
        {
            if (myBag.Count < 6) // 6 is bag capacity (if you want to change it, change '6' and in .AddToBag method in LegendarySword class)
            {
                Bag bag = new Bag
                {
                    Creator = mySword.Creator,
                    CriticalHitChance = mySword.CriticalHitChance,
                    Damage = mySword.Damage,
                    Fightspeed = mySword.Fightspeed,
                    ImageName = mySword.ImageName,
                    Name = mySword.Name,
                    Value = mySword.Value,
                    Level = mySword.Level,
                    Swordpic = mySword.Swordpic
                };
                myBag.Add(bag);
            }
            else
            {
                Console.WriteLine("Your bag is full! Do you want to exchange weapon?(Y / N)");
                if (Console.ReadLine() == "Y")
                {
                   
                    Console.WriteLine("Enter a number of the weapon that you want to discard.(1..{0})", myBag.Count);
                    string op = Console.ReadLine();
                    while ((Convert.ToInt32(op) > myBag.Count)||(Convert.ToInt32(op)==0))
                    {
                        Console.WriteLine("Enter number in (1..{0})", myBag.Count);
                        op = Console.ReadLine();
                    }
                    myBag.Remove(myBag.ElementAt(Convert.ToInt16(op) -1));
                    Bag bag = new Bag
                    {
                        Creator = mySword.Creator,
                        CriticalHitChance = mySword.CriticalHitChance,
                        Damage = mySword.Damage,
                        Fightspeed = mySword.Fightspeed,
                        ImageName = mySword.ImageName,
                        Name = mySword.Name,
                        Value = mySword.Value,
                        Level = mySword.Level,
                        Swordpic = mySword.Swordpic
                    };
                    myBag.Add(bag);
                    Console.WriteLine("Your bag now:");
                    Console.WriteLine("-----------------");
                    foreach (var p in myBag)
                    {
                        Console.WriteLine(p.Name);
                    }
                    Console.WriteLine("-----------------");
                }
            }
        }
    }
    // .MakeSword (combines sword blade and handle) + .AddToBag methods

    class LegendarySword : BaseWeapon
    {
        public Bitmap Swordpic { get; set; }
        public static void AddToBag(LegendarySword mySword, List<Bag> myBag)
        {
            if (myBag.Count < 6) // 6 is bag capacity (if you want to change it, change '6' and in .AddToBag method in Sword class)
            {
                Bag bag = new Bag
                {
                    Creator = mySword.Creator,
                    CriticalHitChance = mySword.CriticalHitChance,
                    Damage = mySword.Damage,
                    Fightspeed = mySword.Fightspeed,
                    ImageName = mySword.ImageName,
                    Name = mySword.Name,
                    Value = mySword.Value,
                    Level = mySword.Level,
                    Swordpic = mySword.Swordpic
                };
                myBag.Add(bag);
            }
            else
            {
                Console.WriteLine("Your bag is full! Do you want to exchange weapon?(Y / N)");
                if (Console.ReadLine() == "Y")
                {
                    Console.WriteLine("Enter a number of the weapon that you want to discard.(1..{0})", myBag.Count);
                    string op = Console.ReadLine();
                    while ((Convert.ToInt32(op) > myBag.Count) || (Convert.ToInt32(op) == 0))
                    {
                        Console.WriteLine("Enter number in (1..{0})", myBag.Count);
                        op = Console.ReadLine();
                    }
                    myBag.Remove(myBag.ElementAt(Convert.ToInt16(op) - 1));
                    Bag bag = new Bag
                    {
                        Creator = mySword.Creator,
                        CriticalHitChance = mySword.CriticalHitChance,
                        Damage = mySword.Damage,
                        Fightspeed = mySword.Fightspeed,
                        ImageName = mySword.ImageName,
                        Name = mySword.Name,
                        Value = mySword.Value,
                        Level = mySword.Level,
                        Swordpic = mySword.Swordpic
                    };
                    myBag.Add(bag);
                    Console.WriteLine("Your bag now:");
                    Console.WriteLine("-----------------");
                    foreach (var p in myBag)
                    {
                        Console.WriteLine(p.Name);
                    }
                    Console.WriteLine("-----------------");
                }
            }
        }
    }

    class Bag :BaseWeapon
    {
        public Bitmap Swordpic { get; set; }
        public static int SellSword(List<Bag> myBag, int number)
        {
            int t= myBag.ElementAt(number).Value;
            myBag.Remove(myBag.ElementAt(number));
            return t; 
        }
    }
    // .SellSword method

    class HowIGetWeapon 
    {
        public Bitmap pic { get; set; }
        public static void FindChest(Sword mySword,List<LegendarySword> legendarySword,List<SwordBlade> Sblade, List<SwordHandle> Shandle,List<Bag> myBag,string alfa)
        {
            Random e = new Random();
            int u = 0;
            if ((u = e.Next(0, 5)) != 0) // (0, 5) - 1/5 (20%)  chance to get legendary sword
            {

                Sword.MakeSword(Sblade, Shandle, mySword,alfa);
                Console.WriteLine("Name: {0}, Creator: {1}, Level: {2}, Damage: {3}, Fightspeed: {4} hit per s, CriticalHitChance: {5}%, Value: {6}"
                   , mySword.Name
                   , mySword.Creator
                   , mySword.Level
                   , mySword.Damage
                   , mySword.Fightspeed
                   , mySword.CriticalHitChance
                   , mySword.Value);
                Sword.AddToBag(mySword, myBag);

            }
            else
            {

                int q = e.Next(0, legendarySword.Count);

                Console.WriteLine("Name: {0}, Creator: {1}, Level: {2}, Damage: {3}, Fightspeed: {4} hit per s, CriticalHitChance: {5}%, Value: {6}"
               , legendarySword.ElementAt(q).Name
               , legendarySword.ElementAt(q).Creator
               , legendarySword.ElementAt(q).Level
               , legendarySword.ElementAt(q).Damage
               , legendarySword.ElementAt(q).Fightspeed
               , legendarySword.ElementAt(q).CriticalHitChance
               , legendarySword.ElementAt(q).Value);
                legendarySword.ElementAt(q).Swordpic.Save(alfa + @"CreatedSword\" + legendarySword.ElementAt(q).Name + ".bmp");
                LegendarySword.AddToBag(legendarySword.ElementAt(q), myBag);
            }
        }
        public static void KillEnemy(Sword mySword, List<LegendarySword> legendarySword, List<SwordBlade> Sblade, List<SwordHandle> Shandle, List<Bag> myBag,string alfa)
        {
            Random e = new Random();
            int u = 0;
            if ((u = e.Next(0, 10)) != 0) // (0, 10) - 1/10 (10%)  chance to get legendary sword
            {

                Sword.MakeSword(Sblade, Shandle, mySword,alfa);
                Console.WriteLine("Name: {0}, Creator: {1}, Level: {2}, Damage: {3}, Fightspeed: {4} hit per s, CriticalHitChance: {5}%, Value: {6}"
                   , mySword.Name
                   , mySword.Creator
                   , mySword.Level
                   , mySword.Damage
                   , mySword.Fightspeed
                   , mySword.CriticalHitChance
                   , mySword.Value);
                Sword.AddToBag(mySword, myBag);

            }
            else
            {

                int q = e.Next(0, legendarySword.Count);

                Console.WriteLine("Name: {0}, Creator: {1}, Level: {2}, Damage: {3}, Fightspeed: {4} hit per s, CriticalHitChance: {5}%, Value: {6}"
               , legendarySword.ElementAt(q).Name
               , legendarySword.ElementAt(q).Creator
               , legendarySword.ElementAt(q).Level
               , legendarySword.ElementAt(q).Damage
               , legendarySword.ElementAt(q).Fightspeed
               , legendarySword.ElementAt(q).CriticalHitChance
               , legendarySword.ElementAt(q).Value);
                legendarySword.ElementAt(q).Swordpic.Save(alfa + @"CreatedSword\" + legendarySword.ElementAt(q).Name + ".bmp");
                LegendarySword.AddToBag(legendarySword.ElementAt(q), myBag);
            }
        }
    }
    // .FindChest + .KillEnemy  methods
}
