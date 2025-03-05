namespace CJH_Frame.CharacterBase
{
    public enum StatAttribute
    {
        // 공격 관련 보너스
        AttackPowerBonus,           // 공격력 보너스 (%)
        CriticalRate,               // 크리티컬 확률 (%)
        CriticalDamage,             // 치명타 데미지 (%, 예: 150이면 1.5배)
        DamageIncrease,             // 피해량 증가 (%)
        ArmorFixedPenetration,      // 방어구 고정 관통력
        ArmorPercentPenetration,    // 방어구 퍼센트 관통력 (%)

        // 기본 스탯
        BaseAttack,                 // 기본 공격력
        Defense,                    // 수비 (추가 방어력)
        MaxHP,                      // 최대 체력
        Armor,                      // 방어력
        FixedDamageReduction,       // 고정 데미지 감소
        PercentDamageReduction,     // 퍼센트 데미지 감소 (%)
        GuardRate,                  // 가드 발동 확률 (%)
        GuardPercent,               // 가드 시 데미지 감소 (%)
        EvasionPercent              // 회피 확률 (%)
    }

    public class CharacterStatContainer
    {
        public Dictionary<StatAttribute, int> Stats { get; private set; } = new Dictionary<StatAttribute, int>();

        public CharacterStatContainer()
        {
            Stats[StatAttribute.BaseAttack] = 100;
            Stats[StatAttribute.AttackPowerBonus] = 20;           // +20%
            Stats[StatAttribute.CriticalRate] = 30;                // 30% 크리티컬 확률
            Stats[StatAttribute.CriticalDamage] = 150;             // 치명타시 150% 데미지
            Stats[StatAttribute.DamageIncrease] = 10;              // 10% 피해 증가
            Stats[StatAttribute.ArmorFixedPenetration] = 5;
            Stats[StatAttribute.ArmorPercentPenetration] = 20;       // 20% 방어구 무시

            Stats[StatAttribute.Armor] = 30;
            Stats[StatAttribute.FixedDamageReduction] = 5;
            Stats[StatAttribute.PercentDamageReduction] = 10;        // 10% 데미지 감소
            Stats[StatAttribute.GuardRate] = 20;                     // 20% 가드 발동 확률
            Stats[StatAttribute.GuardPercent] = 50;                  // 가드 시 50% 데미지 감소
            Stats[StatAttribute.EvasionPercent] = 10;                // 10% 회피 확률
        }

        public int GetStat(StatAttribute statAttribute)
        {
            return Stats[statAttribute];
        }
    }
}
