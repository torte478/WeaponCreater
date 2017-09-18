using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace WeaponCeater
{
    public class SwordLoader
    {
        private readonly PathManager pathManager;

        public SwordLoader(PathManager pathManager)
        {
            this.pathManager = pathManager;
        }

        public List<LegendarySword> LoadLegendarySwords()
        {
            var swords = LoadWeaponItems<LegendarySword>(pathManager.LegendarySwordsDirectory);
            return swords;
        }

        public List<SwordBlade> LoadSwordBlades()
        {
            return LoadWeaponItems<SwordBlade>(pathManager.SwordBladesDirectory);
        }

        public List<SwordHandle> LoadSwordHandles()
        {
            return LoadWeaponItems<SwordHandle>(pathManager.SwordHandlesDirectory);
        }

        private List<T> LoadWeaponItems<T>(string path)
            where T : IWeaponItem, new()
        {
            var stats = LoadStatistics(path);

            return stats
                    .Select(stat => new T { Stats = stat, Picture = LoadPicture(stat.ImageName) })
                    .ToList();
        }

        private List<WeaponStatistics> LoadStatistics(string dataPath)
        {
            return File.ReadAllLines(dataPath)
                        .Select(line => line.Split(' '))
                        .Select(ConvertToStatistics)
                        .Where(stat => stat != null)
                        .ToList();
        }

        private static WeaponStatistics ConvertToStatistics(string[] tokens)
        {
            try
            {
                var stat = new WeaponStatistics
                {
                    Name = tokens[0],
                    Fightspeed = Convert.ToInt32(tokens[1]),
                    Damage = Convert.ToInt32(tokens[2]),
                    CriticalHitChance = Convert.ToInt32(tokens[3]),
                    Value = Convert.ToInt32(tokens[4]),
                    Creator = tokens[5],
                    Level = Convert.ToInt32(tokens[6]),
                    ImageName = tokens[7],
                };
                return stat;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Bitmap LoadPicture(string imageName)
        {
            try
            {
                var fileName = string.Format("{0}.bmp", imageName);
                var filePath = Path.Combine(pathManager.PicturesPath, fileName);
                return new Bitmap(filePath);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}