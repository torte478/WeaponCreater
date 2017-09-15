using System.Collections.Generic;
using System.Linq;

namespace WeaponCeater
{
    public class Inventory
    {
        private const int MaxWeaponCount = 6;

        private readonly List<ISword> weapons;

        public Inventory()
        {
            weapons = new List<ISword>();
        }

        public bool IsFull()
        {
            return weapons.Count() >= MaxWeaponCount;
        }

        public void Add(ISword weapon)
        {
            weapons.Add(weapon);
        }

        public void RemoveAt(int replacedWeaponIndex)
        {
            weapons.RemoveAt(replacedWeaponIndex);
        }

        public int GetTotalCost()
        {
            return weapons.Sum(weapon => weapon.Stats.Value);
        }

        public IEnumerable<ISword> GetWeapons()
        {
            return weapons.ToList();
        }

        public int Count()
        {
            return weapons.Count();
        }
    }
}