namespace WeaponCeater
{
    public interface IWeaponGenerator
    {
        IWeapon Generate(int legendaryWeaponChance);
    }
}