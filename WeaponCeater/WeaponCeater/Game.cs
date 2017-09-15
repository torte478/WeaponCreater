using System.IO;
using System.Linq;

namespace WeaponCeater
{
    public class Game
    {
        private const int ChestLegendaryWeaponChance = 5;
        private const int EnemyLegendaryWeaponChance = 10;

        private readonly IWeaponGenerator weaponGenerator;
        private readonly UserInterface gui;
        private readonly string createdSwordsDirectory;

        private readonly Inventory inventory;

        public Game(IWeaponGenerator weaponGenerator, UserInterface gui, string createdSwordsDirectory)
        {
            this.weaponGenerator = weaponGenerator;
            this.gui = gui;
            this.createdSwordsDirectory = createdSwordsDirectory;

            inventory = new Inventory();
        }

        public void Play()
        {
            DoActions();
            ShowTotalWeaponCost();
            CheckPicturesDeleting();
        }

        private void DoActions()
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

        private void FindWeapon(int legendaryWeaponChance)
        {
            var weapon = weaponGenerator.Generate(legendaryWeaponChance);
            gui.ShowWeapon(weapon);
            TryPutWeaponToInventory(weapon);
        }

        private void TryPutWeaponToInventory(IWeapon weapon)
        {
            if (inventory.IsFull())
            {
                var wasExcanged = TryExchangeWeapon(weapon);
                if (wasExcanged)
                    gui.ShowInventory(inventory);
            }
            else
            {
                inventory.Add(weapon);
            }
        }

        private bool TryExchangeWeapon(IWeapon weapon)
        {
            var needExchange = gui.AskSwordReplace();
            if (needExchange)
            {
                ExcangedWeapon(weapon);
            }
            return needExchange;
        }

        private void ExcangedWeapon(IWeapon weapon)
        {
            var removedWeaponIndex = gui.AskInventoryBagIndex(1, inventory.Count());
            inventory.RemoveAt(removedWeaponIndex);
            inventory.Add(weapon);
        }

        private void KillEnemy()
        {
            FindWeapon(EnemyLegendaryWeaponChance);
        }

        private void ShowTotalWeaponCost()
        {
            var totalCost = inventory.GetTotalCost();
            gui.ShowTotalCost(totalCost);
        }

        private void CheckPicturesDeleting()
        {
            var needDeletePictures = gui.AskPictureDeleting();
            if (needDeletePictures)
            {
                ClearCreatedSwordsDirectory();
            }
            else
            {
                gui.ShowCheckDirectoryMessage(createdSwordsDirectory);
            }
        }

        private void ClearCreatedSwordsDirectory()
        {
            var files = new DirectoryInfo(createdSwordsDirectory).GetFiles();
            foreach (var file in files)
            {
                file.Delete();
            }

            gui.ShowPicturesDeletedMessage();
        }
    }
}