namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    // 2. 시전방식 - 이벤트 접근
    public abstract class CastingStrategy : ISkillOption
    {
        public string OptionName => typeof(CastingStrategy).Name;
        public abstract void SetEvent(GameCharacter caster);
    }
}


namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail.CastingDetail
{
    public class ActiveSkill : CastingStrategy
    {
        public override void SetEvent(GameCharacter caster)
        {
            Console.WriteLine(" ActiveSkill ");
        }
    }

    public class PassiveSkill : CastingStrategy
    {
        public override void SetEvent(GameCharacter caster)
        {
            Console.WriteLine(" PassiveSkill ");
        }
    }
}