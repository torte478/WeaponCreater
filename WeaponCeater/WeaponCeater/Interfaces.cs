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

    public abstract class BaseSwordPart : IWeaponPart
    {
        public WeaponStatistics Stats { get; set; }
        public Bitmap Picture { get; set; }
    }

    public class SwordBlade : BaseSwordPart { }

    public class SwordHandle : BaseSwordPart { }

    public interface ISword : IWeapon { }

    public abstract class BaseSword : ISword
    {
        public WeaponStatistics Stats { get; set; }
        public Bitmap Picture { get; set; }

        public override string ToString()
        {
            return Stats.ToString();
        }
    }

    public class Sword : BaseSword { }

    public class LegendarySword : BaseSword { }
}
