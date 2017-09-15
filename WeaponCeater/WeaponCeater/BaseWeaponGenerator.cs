using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WeaponCeater
{
    public abstract class BaseWeaponGenerator
    {
        private readonly Random random = new Random();
        private readonly PathManager pathManager;

        protected static readonly Dictionary<string, CreatorBonus> BladeBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ Damage = 1.1 } },
            { "elf",   new CreatorBonus{ FightSpeed = 1.15 } },
            { "dwarf", new CreatorBonus{ CriticalHitChance = 5 } },
            { "orc",   new CreatorBonus{ Value = 0.85 } },
            { "daemon",new CreatorBonus{ Damage = 1.40, FightSpeed = 0.90, Value = 1.10 } }
        };

        protected static readonly Dictionary<string, CreatorBonus> HandleBonuses = new Dictionary<string, CreatorBonus>
        {
            { "human", new CreatorBonus{ CriticalHitChance = 5} },
            { "elf",   new CreatorBonus{ Damage = 1.25 } },
            { "dwarf", new CreatorBonus{ Damage = 1.50, Value = 1.30 } },
            { "orc",   new CreatorBonus{ FightSpeed = 1.20} },
            { "daemon",new CreatorBonus{ CriticalHitChance = 10, Damage = 0.90 } }
        };

        protected BaseWeaponGenerator(PathManager pathManager)
        {
            this.pathManager = pathManager;
        }

        protected void SaveToFile(IWeapon weapon)
        {
            var fileName = string.Format("{0}.bmp", weapon.Stats.Name);
            var filePath = Path.Combine(pathManager.CreatedSwordsDirectory, fileName);
            weapon.Picture.Save(filePath);
        }

        protected bool IsLegendaryWeapon(int legendaryWeaponChance)
        {
            return random.Next(legendaryWeaponChance) == 0;
        }

        protected static void ApplyBonus(IWeapon weapon, IWeaponPart weaponPart, Dictionary<string, CreatorBonus> bonuses)
        {
            var creator = weaponPart.Stats.Creator;
            if (bonuses.ContainsKey(creator))
            {
                var bonus = bonuses[creator];
                bonus.Apply(weapon);
            }
        }

        protected T GetRandomItem<T>(IReadOnlyList<T> items)
        {
            var index = random.Next(items.Count());
            return items[index];
        }
    }
}