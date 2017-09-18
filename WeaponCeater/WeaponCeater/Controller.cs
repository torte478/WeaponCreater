namespace WeaponCeater
{
    public class Controller
    {
        private readonly Game model;
        private readonly UserInterface view;

        public Controller(Game model, UserInterface view)
        {
            this.model = model;
            this.view = view;

            model.AllActionsCompleted += Model_AllActionsCompleted;
            model.WeaponFounded += Model_WeaponFounded;
            model.NeedExchangeWeapon += Model_NeedExchangeWeapon;
        }

        public void Run()
        {
            model.DoActions();
        }

        private void Model_NeedExchangeWeapon(IWeapon weapon)
        {
            var needExchange = view.AskSwordReplace();
            if (needExchange)
            {
                var index = view.AskInventoryBagIndex(1, Inventory.MaxWeaponCount);
                model.ExchangeWeapon(weapon, index);
            }
            view.ShowInventory(model.Inventory);
        }

        private void Model_WeaponFounded(IWeapon weapon)
        {
            view.ShowWeapon(weapon);

            model.TryAddWeaponToInventory(weapon);
        }

        private void Model_AllActionsCompleted()
        {
            ShowTotalWeaponCost();
            CheckPicturesDeleting();
            WaitInput();
        }

        private void WaitInput()
        {
            view.WaitInput();
        }

        private void ShowTotalWeaponCost()
        {
            var cost = model.GetTotalWeaponCost();
            view.ShowTotalCost(cost);
        }

        private void CheckPicturesDeleting()
        {
            var needRemovePictures = view.AskPictureDeleting();
            if (needRemovePictures)
            {
                RemovePictures();
            }
            else
            {
                ShowPicutesSavedMessage();
            }
        }

        private void ShowPicutesSavedMessage()
        {
            var directory = model.GetCreatedWeaponDirectory();
            view.ShowCheckDirectoryMessage(directory);
        }

        private void RemovePictures()
        {
            model.RemoveCreatedPictures();
            view.ShowPicturesDeletedMessage();
        }
    }
}