using System.IO;

namespace WeaponCeater
{
    public class Game
    {
        private const int ChestLegendarySwordChance = 5;
        private const int EnemyLegendarySwordChance = 10;

        private readonly WeaponGenerator weaponGenerator;
        private readonly UserInterface gui;
        private readonly PathManager pathManager;

        private readonly Inventory inventory;

        public Game(WeaponGenerator weaponGenerator, UserInterface gui, PathManager pathManager)
        {
            this.weaponGenerator = weaponGenerator;
            this.gui = gui;
            this.pathManager = pathManager;

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
            FindWeapon(ChestLegendarySwordChance);
        }

        private void FindWeapon(int legendarySwordChance)
        {
            var weapon = weaponGenerator.Generate(legendarySwordChance);
            gui.ShowWeapon(weapon);
            TryPutWeaponToInventory(weapon);
        }

        private void TryPutWeaponToInventory(ISword weapon)
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

        private bool TryExchangeWeapon(ISword weapon)
        {
            var needExchange = gui.AskSwordReplace();
            if (needExchange)
            {
                ExcangedWeapon(weapon);
            }
            return needExchange;
        }

        private void ExcangedWeapon(ISword weapon)
        {
            var removedWeaponIndex = gui.AskInventoryBagIndex();
            inventory.RemoveAt(removedWeaponIndex);
            inventory.Add(weapon);
        }

        private void KillEnemy()
        {
            FindWeapon(EnemyLegendarySwordChance);
        }

        private void ShowTotalWeaponCost()
        {
            var totalCost = inventory.GetTotalCost();
            gui.ShowTotalCost(totalCost);
        }

        private void CheckPicturesDeleting()
        {
            var needDeletePictures = gui.AskPicturedDeleting();
            if (needDeletePictures)
            {
                ClearCreatedSwordsDirectory();
            }
            else
            {
                gui.ShowCheckDirectoryMessage();
            }
        }

        private void ClearCreatedSwordsDirectory()
        {
            var directory = pathManager.CreatedSwordsDirectory;

            var files = new DirectoryInfo(directory).GetFiles();
            foreach (var file in files)
            {
                file.Delete();
            }

            gui.ShowPicturesDeletedMessage();
        }
    }
}