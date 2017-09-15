using System.Drawing;

namespace WeaponCeater
{
    public interface IWeaponItem
    {
        WeaponStatistics Stats { get; set; }
        Bitmap Picture { get; set; }
    }

    public interface IWeapon : IWeaponItem { }

    public interface IWeaponPart : IWeaponItem { }

    public interface ISword : IWeapon { }
}
