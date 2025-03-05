using CJH_Frame.CharacterBase;
using CJH_Frame.CharacterBase.CharacterSkill;

namespace CJH_Frame
{
    public class GameCharacter
    {
        public SkillEquipment SkillSlots;
        public CharacterStatContainer CharacterStatContainer;
        public CharacterBattleStateMachine CharacterBattleStateMachine;

        int Id { get; }
        public int Health { get; set; }

        public GameCharacter()
        {
            SkillSlots = new();
            CharacterStatContainer = new CharacterStatContainer();
            CharacterBattleStateMachine = new CharacterBattleStateMachine(this);
        }
    }

    public interface ICharacterState
    {
        float Health { get; set; }
        bool IsDead { get; }
        void UpdateState(string type, float value);
    }
}
