namespace WeaponCeater
{
    public class WeaponStatistics
    {
        public const string UndefinedImageName = "undefined";

        public string Name { get; set; }
        public int Fightspeed { get; set; }
        public int Damage { get; set; }
        public int CriticalHitChance { get; set; }
        public int Value { get; set; }
        public string ImageName { get; set; }
        public string Creator { get; set; }
        public int Level { get; set; }

        public WeaponStatistics Combine(WeaponStatistics other)
        {
            return new WeaponStatistics
            {
                Name = CombineNames(Name, other.Name),
                Fightspeed = CombineIntValues(Fightspeed, other.Fightspeed),
                Damage = CombineIntValues(Damage, other.Damage),
                CriticalHitChance = CombineIntValues(CriticalHitChance, other.CriticalHitChance),
                Value = CombineIntValues(Value, other.Value),
                ImageName = UndefinedImageName,
                Creator = CombineCreators(Creator, other.Creator),
                Level = CombineIntValues(Level, other.Level),
            };
        }

        private static int CombineIntValues(int first, int second)
        {
            return (first + second) / 2;
        }

        private static string CombineCreators(string first, string second)
        {
            return string.Format("{0}/{1}", first, second);
        }

        private static string CombineNames(string first, string second)
        {
            return string.Format("{0} {1}", first, second);
        }

        public override string ToString()
        {
            return string.Format(
                    "Name: {0}, Creator: {1}, Level: {2}, Damage: {3}, Fightspeed: {4} hit per s, CriticalHitChance: {5}%, Value: {6}",
                    Name,
                    Creator,
                    Level,
                    Damage,
                    Fightspeed,
                    CriticalHitChance,
                    Value);
        }
    }
}
