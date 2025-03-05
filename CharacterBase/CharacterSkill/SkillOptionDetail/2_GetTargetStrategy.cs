namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    // 3. 즉발 투사체  타겟 논타겟
    public abstract class GetTargetStrategy : ISkillOption
    {
        public string OptionName => typeof(GetTargetStrategy).Name;
        public abstract List<GameCharacter> GetTarget();
    }
}


namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail.CastingType
{
    // stage manager 관련접근
    public class HitScan : GetTargetStrategy
    {
        public override List<GameCharacter> GetTarget() { return null; }
    }
}
