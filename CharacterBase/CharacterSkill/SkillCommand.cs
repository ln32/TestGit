namespace CJH_Frame.CharacterBase.CharacterSkill
{
    public interface SkillManageCommand
    {
        SkillCastCommand GetSkill(GameCharacter character);
        void OnEquip(GameCharacter character);
        void OnUnequip(GameCharacter character);
    }

    public interface SkillCastCommand
    {
        // 시전?
        bool IsCanSkillCast(GameCharacter gameCharacter);
        // 시전!
        void OnSkillCast(GameCharacter gameCharacter);
        // 상대 받기
        List<GameCharacter>? GetTarget(GameCharacter caster);
        // 잡도리
        void WorkToTarget(GameCharacter caster, GameCharacter target);
    }
}
