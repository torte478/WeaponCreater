using System;

namespace WeaponCeater
{
    public class UserInterface
    {
        public void ShowInventory(Inventory inventory)
        {
            Console.WriteLine();
            Console.WriteLine("Your swords:");
            foreach (var weapon in inventory)
            {
                Console.WriteLine(weapon.Stats.Name);
            }
            Console.WriteLine();
        }

        public bool AskSwordReplace()
        {
            var question = "Your bag is full! Do you want to exchange mySword?(Y / N)";
            return AskYesNoQuestion(question);
        }

        private bool AskYesNoQuestion(string question)
        {
            Console.WriteLine(question);
            string answer;
            do
            {
                var line = Console.ReadLine();
                if (line == null) return false;

                answer = line.ToLower();

            } while (answer != "y" && answer != "n");

            return answer == "y";
        }

        public void WaitInput()
        {
            Console.ReadLine();
        }

        public int AskInventoryBagIndex(int minValue, int maxValue)
        {
            var message = string.Format("Enter number in ({0}..{1})", minValue, maxValue);
            Console.Write("Enter a number of the mySword that you want to discard. ");
            int index;
            bool correctNumber;
            do
            {
                Console.WriteLine(message);
                var line = Console.ReadLine();
                if (line == null) return -1;
                correctNumber = int.TryParse(line, out index);
            } while (!correctNumber);

            return index - 1;
        }

        public void ShowTotalCost(int totalCost)
        {
            var message = string.Format("If you sell all swords you will earn {0} coins.", totalCost);
            Console.WriteLine();
            Console.WriteLine(message);
        }

        public bool AskPictureDeleting()
        {
            var question = "Do you want delete new pictures?(Y/N)";
            return AskYesNoQuestion(question);
        }

        public void ShowCheckDirectoryMessage(string directory)
        {
            var message = string.Format("Check the directory: {0}", directory);
            Console.WriteLine(message);
        }

        public void ShowPicturesDeletedMessage()
        {
            var message = string.Format("All pictures was deleted.");
            Console.WriteLine(message);
        }

        public void ShowWeapon(IWeapon weapon)
        {
            Console.WriteLine(weapon.ToString());
        }
    }
}