using System;
using System.Collections.Generic;

namespace CJH_Frame.CharacterBase
{

    #region Calculator Interfaces
    static class DamageProcess
    {
        internal static int CalculateAttackPower(CharacterStatContainer attacker)
        {
            // 기본 공격력과 보너스 % (예: 100 + 20% = 120)
            int baseAttack = attacker.Stats.ContainsKey(StatAttribute.BaseAttack) ? attacker.Stats[StatAttribute.BaseAttack] : 0;
            int bonusPercent = attacker.Stats.ContainsKey(StatAttribute.AttackPowerBonus) ? attacker.Stats[StatAttribute.AttackPowerBonus] : 0;
            return baseAttack * (1 + bonusPercent / 100);
        }

        internal static int CalculateCriticalMultiplier(CharacterStatContainer attacker)
        {
            Random _random = new Random();

            // 크리티컬 발생 시 치명타 데미지 (예: 150% → 1.5배)
            int critRate = attacker.Stats.ContainsKey(StatAttribute.CriticalRate) ? attacker.Stats[StatAttribute.CriticalRate] : 0;
            int critDamage = attacker.Stats.ContainsKey(StatAttribute.CriticalDamage) ? attacker.Stats[StatAttribute.CriticalDamage] : 100;
            bool isCritical = _random.Next(100) < critRate;
            return isCritical ? (critDamage / 100) : 1;
        }

        internal static int CalculateEffectiveArmor(CharacterStatContainer attacker, CharacterStatContainer defender)
        {
            // 관통력 적용: 고정 관통력을 먼저 적용하고, 퍼센트 관통력으로 방어력 일부 무시
            int fixedPen = attacker.Stats.ContainsKey(StatAttribute.ArmorFixedPenetration) ? attacker.Stats[StatAttribute.ArmorFixedPenetration] : 0;
            int percentPen = attacker.Stats.ContainsKey(StatAttribute.ArmorPercentPenetration) ? attacker.Stats[StatAttribute.ArmorPercentPenetration] : 0;
            int defenderArmor = defender.Stats.ContainsKey(StatAttribute.Armor) ? defender.Stats[StatAttribute.Armor] : 0;
            int effectiveArmor = Math.Max(0, defenderArmor - fixedPen);
            effectiveArmor *= (1 - percentPen / 100);
            return effectiveArmor;
        }

        internal static int CalculateDefense(CharacterStatContainer defender)
        {
            // 추가 방어 수치 (필요시 사용)
            return defender.Stats.ContainsKey(StatAttribute.Defense) ? defender.Stats[StatAttribute.Defense] : 0;
        }

        internal static int ApplyDamageReduction(double damage, CharacterStatContainer defender)
        {
            // 고정 데미지 감소 적용 후, 퍼센트 데미지 감소 적용
            int fixedReduction = defender.Stats.ContainsKey(StatAttribute.FixedDamageReduction) ? defender.Stats[StatAttribute.FixedDamageReduction] : 0;
            int percentReduction = defender.Stats.ContainsKey(StatAttribute.PercentDamageReduction) ? defender.Stats[StatAttribute.PercentDamageReduction] : 0;
            int reducedDamage = (int)Math.Max(0, damage - fixedReduction);
            reducedDamage *= (1 - percentReduction / 100);
            return reducedDamage;
        }


        internal static int ApplyGuard(int damage, CharacterStatContainer defender)
        {
            Random _random = new Random();
            // 가드 발동 시 데미지 감소
            int guardRate = defender.Stats.ContainsKey(StatAttribute.GuardRate) ? defender.Stats[StatAttribute.GuardRate] : 0;
            int guardPercent = defender.Stats.ContainsKey(StatAttribute.GuardPercent) ? defender.Stats[StatAttribute.GuardPercent] : 0;
            if (_random.Next(100) < guardRate)
            {
                damage *= (1 - guardPercent / 100);
            }
            return damage;
        }

        internal static bool IsEvaded(CharacterStatContainer defender)
        {
            Random _random = new Random();
            int evasion = defender.Stats.ContainsKey(StatAttribute.EvasionPercent) ? defender.Stats[StatAttribute.EvasionPercent] : 0;
            return _random.Next(100) < evasion;
        }
     
    }
    #endregion


    public static class DamageCalculator
    {
        public static int CalculateDamage(CharacterStatContainer attacker, CharacterStatContainer defender)
        {
            // 1. 회피 체크: 회피하면 데미지 0
            if (DamageProcess.IsEvaded(defender))
                return 0;

            // 2. 공격력 및 치명타 적용
            int attackPower = DamageProcess.CalculateAttackPower(attacker);
            int critMultiplier = DamageProcess.CalculateCriticalMultiplier(attacker);
            int damage = attackPower * critMultiplier;

            // 3. 관통력을 통한 방어력 감소 적용
            int effectiveArmor = DamageProcess.CalculateEffectiveArmor(attacker, defender);
            damage = Math.Max(0, damage - effectiveArmor);

            // (추가로 필요하다면, 방어 스탯도 고려할 수 있음)
            int defense = DamageProcess.CalculateDefense(defender);
            damage = Math.Max(0, damage - defense);

            // 4. 피해량 증가(공격자)
            int dmgIncrease = attacker.Stats.ContainsKey(StatAttribute.DamageIncrease) ? attacker.Stats[StatAttribute.DamageIncrease] : 0;
            damage *= (1 + dmgIncrease / 100);

            // 5. 데미지 감소(방어자)
            damage = DamageProcess.ApplyDamageReduction(damage, defender);

            // 6. 가드 적용
            damage = DamageProcess.ApplyGuard(damage, defender);

            return damage;
        }
    }
}
