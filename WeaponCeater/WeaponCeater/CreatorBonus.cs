using System;

namespace WeaponCeater
{
    public class CreatorBonus
    {
        public double Damage { get; set; }
        public double FightSpeed { get; set; }
        public int CriticalHitChance { get; set; }
        public double Value { get; set; }

        public CreatorBonus()
        {
            Damage = 1.0;
            FightSpeed = 1.0;
            CriticalHitChance = 0;
            Value = 1.0;
        }

        public void Apply(IWeapon weapon)
        {
            weapon.Stats.Damage = Convert.ToInt32(weapon.Stats.Damage * Damage);
            weapon.Stats.Fightspeed = Convert.ToInt32(weapon.Stats.Fightspeed * FightSpeed);
            weapon.Stats.Value = Convert.ToInt32(weapon.Stats.Value * Value);
            weapon.Stats.CriticalHitChance = weapon.Stats.CriticalHitChance + CriticalHitChance;
        }
    }
}