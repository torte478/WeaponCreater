namespace WeaponCeater
{
    class Program
    {
        static void Main()
        {
            var pathManager = new PathManager();
            var swordLoader = new SwordLoader(pathManager);
            var weaponGenerator = new SwordGenerator(swordLoader, pathManager);
            
            var game = new Game(weaponGenerator, pathManager.CreatedSwordsDirectory);
            var gui = new UserInterface();

            var controller = new Controller(game, gui);

            controller.Run();
        }
    }
}
