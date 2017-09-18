using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeaponCeater
{
    public class Inventory : IEnumerable<IWeapon>
    {
        public const int MaxWeaponCount = 6;

        private readonly List<IWeapon> weapons;

        public Inventory()
        {
            weapons = new List<IWeapon>();
        }

        public bool IsFull()
        {
            return weapons.Count() >= MaxWeaponCount;
        }

        public void Add(IWeapon weapon)
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

        public IEnumerator<IWeapon> GetEnumerator()
        {
            return weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}