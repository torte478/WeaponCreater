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
            throw new NotImplementedException();
        }

        public List<SwordBlade> LoadSwordBlades()
        {
            throw new System.NotImplementedException();
        }

        public List<SwordHandle> LoadSwordHandles()
        {
            throw new System.NotImplementedException();
        }

        private List<Tuple<WeaponStatistics, Bitmap>> Load(string dataPath)
        {
            return LoadStatistics(dataPath)
                        .Select(stat => Tuple.Create(stat, LoadPicture(stat.ImageName)))
                        .ToList();
        }

        private List<WeaponStatistics> LoadStatistics(string dataPath)
        {
            return System.IO.File.ReadAllLines(dataPath)
                        .Select(line => line.Split(' '))
                        .Select(ConvertToStatistics)
                        .Where(stat => stat != null)
                        .ToList();
        }

        private static WeaponStatistics ConvertToStatistics(string[] tokens)
        {
            try
            {
                return new WeaponStatistics
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