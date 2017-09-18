using System;
using System.IO;

namespace WeaponCeater
{
    public class Game
    {
        private const int ChestLegendaryWeaponChance = 5;
        private const int EnemyLegendaryWeaponChance = 10;

        private readonly IWeaponGenerator weaponGenerator;
        private readonly string createdSwordsDirectory;

        public Inventory Inventory { get; private set; }

        public event Action AllActionsCompleted;
        public event Action<IWeapon> WeaponFounded;
        public event Action<IWeapon> NeedExchangeWeapon;

        public Game(IWeaponGenerator weaponGenerator, string createdSwordsDirectory)
        {
            this.weaponGenerator = weaponGenerator;
            this.createdSwordsDirectory = createdSwordsDirectory;

            Inventory = new Inventory();
        }

        public int GetTotalWeaponCost()
        {
            return Inventory.GetTotalCost();
        }

        public void ExchangeWeapon(IWeapon weapon, int index)
        {
            Inventory.RemoveAt(index);
            Inventory.Add(weapon);
        }

        public void TryAddWeaponToInventory(IWeapon weapon)
        {
            if (Inventory.IsFull())
            {
                NeedExchangeWeapon.Raise(weapon);
            }
            else
            {
                Inventory.Add(weapon);
            }
        }

        public string GetCreatedWeaponDirectory()
        {
            return createdSwordsDirectory;
        }

        public void RemoveCreatedPictures()
        {
            var files = new DirectoryInfo(createdSwordsDirectory).GetFiles();
            foreach (var file in files)
            {
                file.Delete();
            }
        }

        public void DoActions()
        {
            FindWeapons();
            AllActionsCompleted.Raise();
        }

        private void FindWeapons()
        {
            FindChest();
            KillEnemy();
            FindChest();
            KillEnemy();
            FindChest();
            KillEnemy();
            FindChest();
            KillEnemy();
        }

        private void FindChest()
        {
            FindWeapon(ChestLegendaryWeaponChance);
        }

        private void KillEnemy()
        {
            FindWeapon(EnemyLegendaryWeaponChance);
        }

        private void FindWeapon(int legendaryWeaponChance)
        {
            var weapon = weaponGenerator.Generate(legendaryWeaponChance);
            WeaponFounded.Raise(weapon);
        }
    }

    public static class EventExtensions
    {
        public static void Raise(this Action eventHandler)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler();
            }
        }

        public static void Raise<T>(this Action<T> eventHandler, T arg)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(arg);
            }
        }
    }
}