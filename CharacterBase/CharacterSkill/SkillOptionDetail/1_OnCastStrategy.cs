namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    // 1. 셀프 이벤트 (변신 쿨타임 마나 소모)
    public abstract class OnCastStrategy : ISkillOption
    {
        public string OptionName => typeof(OnCastStrategy).Name;
        public abstract void WorkToSelf(GameCharacter caster);
    }
}

namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail.OnCastStrategyDetail
{
    public class CooldownOption : OnCastStrategy
    {
        // 이 위치가 맞나
        float LastCast { get; set; }
        float CooldownTime { get; set; }

        public CooldownOption(float LastCast, float CooldownTime)
        {
            this.LastCast = LastCast;
            this.CooldownTime = CooldownTime;
        }

        public override void WorkToSelf(GameCharacter caster)
        {
            ApplyCastCooldown(0);
        }

        bool TryCastCooldown(float currTime) => LastCast + CooldownTime < currTime;
        void ApplyCastCooldown(float currTime) => LastCast = 1;

        public void OnWork(GameCharacter caster, GameCharacter target)
        {
            Console.WriteLine(OptionName + " <<");
        }
    }
}