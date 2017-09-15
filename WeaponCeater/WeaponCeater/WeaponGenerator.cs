using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace WeaponCeater
{
    public class WeaponGenerator
    {
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
                sword.Damage = Convert.ToInt32(sword.Damage * Damage);
                sword.Fightspeed = Convert.ToInt32(sword.Fightspeed * FightSpeed);
                sword.Value = Convert.ToInt32(sword.Value * Value);
                sword.CriticalHitChance = sword.CriticalHitChance + CriticalHitChance;
            }
        }


        private static readonly Dictionary<string, CreatorBonus> BladeBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ Damage = 1.1 } },
            { "elf",   new CreatorBonus{ FightSpeed = 1.15 } },
            { "dwarf", new CreatorBonus{ CriticalHitChance = 5 } },
            { "orc",   new CreatorBonus{ Value = 0.85 } },
            { "daemon",new CreatorBonus{ Damage = 1.40, FightSpeed = 0.90, Value = 1.10 } }
        };

        private static readonly Dictionary<string, CreatorBonus> HandleBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ CriticalHitChance = 5} },
            { "elf",   new CreatorBonus{ Damage = 1.25 } },
            { "dwarf", new CreatorBonus{ Damage = 1.50, Value = 1.30 } },
            { "orc",   new CreatorBonus{ FightSpeed = 1.20} },
            { "daemon",new CreatorBonus{ CriticalHitChance = 10, Damage = 0.90 } }
        };

        private readonly List<LegendarySword> legendarySwords;
        private readonly List<SwordBlade> swordBlades;
        private readonly List<SwordHandle> swordHandles;

        private readonly PathManager pathManager;

        private readonly Random random = new Random();

        public WeaponGenerator(WeaponLoader loader, PathManager pathManager)
        {
            legendarySwords = loader.LoadLegendarySwords();
            swordBlades = loader.LoadSwordBlades();
            swordHandles = loader.LoadSwordHandles();

            this.pathManager = pathManager;
        }

        public ISword Generate(int legendarySwordChance)
        {
            return IsLegendarySword(legendarySwordChance)
                        ? GenereateLegendarySword()
                        : GenerateSword();
        }

        private ISword GenereateLegendarySword()
        {
            var sword = GetRandomItem(legendarySwords);
            SaveToFile(sword);
            return sword;
        }

        private T GetRandomItem<T>(IReadOnlyList<T> items)
        {
            var index = random.Next(items.Count());
            return items[index];
        }

        private Sword GenerateSword()
        {
            var blade = GetRandomItem(swordBlades);
            var handle = GetRandomItem(swordHandles);

            var sword = GenerateSword(blade, handle);
            SaveToFile(sword);

            return sword;
        }

        private Sword GenerateSword(SwordBlade blade, SwordHandle handle)
        {
            var swordStats = blade.Stats.Combine(handle.Stats);
            var picture = CombinePictures(blade.Picture, handle.Picture);

            var sword = new Sword
            {
                Stats = swordStats,
                Picture = picture,
            };
            ApplyBonus(sword, blade, BladeBonuses);
            ApplyBonus(sword, handle, HandleBonuses);

            return sword;
        }

        private static void ApplyBonus(Sword sword, IWeaponPart blade, Dictionary<string, CreatorBonus> bonuses)
        {
            var creator = blade.Stats.Creator;
            if (bonuses.ContainsKey(creator))
            {
                var bonus = bonuses[creator];
                bonus.Apply(sword);
            }
        }

        private static Bitmap CombinePictures(Bitmap bladeBitamp, Bitmap handleBitmap)
        {
            var result = new Bitmap(bladeBitamp);
            for (var x = 0; x < handleBitmap.Width; ++x)
            {
                for (var y = 400; y < handleBitmap.Height; ++y)
                {
                    var color = handleBitmap.GetPixel(x, y);
                    result.SetPixel(x, y, color);
                }
            }
            return result;
        }

        private void SaveToFile(ISword sword)
        {
            var fileName = string.Format("{0}.bmp", sword.Stats.Name);
            var filePath = Path.Combine(pathManager.CreatedSwordsDirectory, fileName);
            sword.Picture.Save(filePath);
        }

        private bool IsLegendarySword(int legendarySwordChance)
        {
            return random.Next(legendarySwordChance) == 0;
        }
    }
}