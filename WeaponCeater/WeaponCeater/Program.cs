namespace WeaponCeater
{
    class Program
    {
        static void Main()
        {
            var pathManager = new PathManager();
            var swordLoader = new SwordLoader(pathManager);
            var weaponGenerator = new SwordGenerator(swordLoader, pathManager);
            var gui = new UserInterface();
            var game = new Game(weaponGenerator, gui, pathManager.CreatedSwordsDirectory);
            game.Play();
        }
    }
}
