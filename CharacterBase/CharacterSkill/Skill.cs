using CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail;
using CJH_Frame.CJH_FlagBaseTypeDictionary;
using CJH_Frame.CJH_ParamFactory;

namespace CJH_Frame.CharacterBase.CharacterSkill
{
    public interface ISkillOption
    {
        string OptionName { get; }
    }

    public class SkillBuilder
    {
        public string SkillName { get; set; }

        private TypeBase_MultiValueDictionary<ISkillOption> optionSet = new();
        private Builder paramSet = new();

        public SkillBuilder(string SkillName)
        {
            this.SkillName = SkillName; 
        }

        // 재료 빌딩
        public void AddOptionPrams(string pramName, object value) => paramSet.Add(pramName, value);
        public void AddOptionPrams<T>(object value) where T : ISkillOption => paramSet.Add<T>(value);

        // param to option.build
        public void BuildOption<Type>() where Type : ISkillOption => BuildOption(typeof(Type).Name);
        public void BuildOption(string optionName)
        {
            if (ParamFactory<ISkillOption>.TryCreateSkill(optionName, paramSet.Build(), out var temp) && temp != null)
                optionSet.TryAdd(temp.OptionName, temp);

            paramSet.Clear();
        }

        public Skill? Build()
        {
            var result = new Skill( SkillName , new SkillDataUnit(optionSet) );

            optionSet = new();
            paramSet.Clear();

            return result;
        }

        private bool InvokeEvent(Func<bool> eventHandler)
        {
            if (eventHandler == null) return true; // 이벤트가 없으면 성공으로 간주

            // 모든 핸들러의 결과를 종합
            bool result = true;
            foreach (Func<bool> handler in eventHandler.GetInvocationList())
            {
                result &= handler.Invoke(); // AND 연산으로 종합
            }

            //options.PrintAll();
            return result;
        }
    }


    // 시전방식 - 액티브 패시브
    // 제한사항 - 쿨타임 마나
    // 타겟 논타겟
    // 즉발 투사체
    // ISkillOption 딜, cc, 
    // 셀프 이벤트 (buff? 변신? 채널링? 기타등등)
    // 
    // 투사체 효과 보이기식 이펙트

    // 1. 스킬 효과 분리 고민
    // 2. 중복타격 방지 고민
    // 3. list? dict?


    public class Skill : SkillCastCommand
    {
        SkillDataUnit optionSet;
        public string SkillName { get; private set; }

        internal Skill(string SkillName, SkillDataUnit optionSet)
        {
            this.SkillName = SkillName;
            this.optionSet = optionSet;
        }

        public void OnEquip(GameCharacter character)
        {
            character.SkillSlots.Equip(1,this);
        }


        bool SkillCastCommand.IsCanSkillCast(GameCharacter caster)
        {
            return optionSet.SkillProcess_Check(caster);
        }

        void SkillCastCommand.OnSkillCast(GameCharacter gameCharacter)
        {
            optionSet.SkillProcess_OnCast(gameCharacter);
        }

        List<GameCharacter>? SkillCastCommand.GetTarget(GameCharacter caster)
        {
            return optionSet.SkillProcess_GetTarget(caster);
        }

        void SkillCastCommand.WorkToTarget(GameCharacter caster, GameCharacter target)
        {
            optionSet.SkillProcess_WorkToTarget(caster,target);
        }
    }

    // 독립적, read only
    internal class SkillDataUnit
    {
        private TypeBase_MultiValueDictionary<ISkillOption> optionSet;

        //object etcAffact;
        internal SkillDataUnit(TypeBase_MultiValueDictionary<ISkillOption> optionSet)
        {
            this.optionSet = optionSet;
        }

        public bool SkillProcess_Check(GameCharacter character)
        {
            optionSet.TryGetValue<RestrictStrategy>(out var _list);
            if (_list == null)
                return false;

            return _list.All(x => ((RestrictStrategy)x).Check(character));
        }


        public void SkillProcess_OnCast(GameCharacter caster)
        {
            optionSet.TryGetValue<OnCastStrategy>(out var _list);
            if (_list == null)
                return;

            _list.ForEach(x => ((OnCastStrategy) x).WorkToSelf(caster));
        }

        public List<GameCharacter>? SkillProcess_GetTarget(GameCharacter caster)
        {
            optionSet.TryGetValue<GetTargetStrategy>(out var _list);
            if (_list == null)
                return null;

            // Is it Linq?
            return ((GetTargetStrategy)_list[0]).GetTarget();
        }


        public void SkillProcess_WorkToTarget(GameCharacter caster, GameCharacter target)
        {
            optionSet.TryGetValue<TargetWorkStrategy>(out var _list);
            if (_list == null)
                return;

            _list.ForEach(x => ((TargetWorkStrategy)x).WorkToTarget(caster, target));
        }
    }
}
