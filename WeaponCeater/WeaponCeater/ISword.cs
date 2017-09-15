using System.Drawing;

namespace WeaponCeater
{
    public interface ISword
    {
        WeaponStatistics Stats { get; set; }
        Bitmap Picture { get; set; }
    }
}