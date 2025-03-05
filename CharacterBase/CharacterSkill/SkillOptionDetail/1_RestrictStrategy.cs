namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    public abstract class RestrictStrategy : ISkillOption
    {
        public string OptionName => typeof(RestrictStrategy).Name;

        public abstract bool Check(GameCharacter caster);
    }

    public class CharacterCondition : RestrictStrategy
    {
        public override bool Check(GameCharacter caster) => caster.Health > 0;
    }
}