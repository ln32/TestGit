using CJH_Frame.CharacterBase;
using CJH_Frame.CharacterBase.CharacterSkill;
using CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail;
using CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail.CastingDetail;
using CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail.OnCastStrategyDetail;
using CJH_Frame.CJH_ParamFactory;
using System.Numerics;
using UnityHFSM;

namespace CJH_Frame
{
    public class TestCode
    {
        public static void Main()
        {
            ParamFactory<ISkillOption>.Init();

            var builder = new SkillBuilder("TestSkill");
            //builder.BuildOption(typeof(IActionable).Name);
            if (true)
            {
                //new ProjectileOption();
                builder.BuildOption(typeof(ActiveSkill).Name);
            }

            if (true)
            {
                builder.BuildOption<CharacterCondition>();
            }

            if (true)
            {
                builder.AddOptionPrams("LastCast", 1f);
                builder.AddOptionPrams("CooldownTime", 2f);
                builder.BuildOption(typeof(CooldownOption).Name);
            }

            if (true)
            {
                //new ProjectileOption();
                builder.AddOptionPrams("CastV3", new Vector3(2, 2, 2));
                builder.AddOptionPrams("TargetV3", new Vector3(0,1,2));
                builder.BuildOption(typeof(ProjectileOption).Name);
            }

            if (true)
            {
                builder.AddOptionPrams("LastCast", 1f);
                builder.AddOptionPrams("CooldownTime", 2f);
                builder.BuildOption(typeof(CooldownOption).Name);
            }

            if (true)
            {
                //(string AffactType, int Count, float TimeGap)
                builder.AddOptionPrams("Name", "MyDamage");
                builder.AddOptionPrams("RepeatCount", 3);
                builder.AddOptionPrams("StackCount", 1);
                builder.AddOptionPrams("TimeGap", 1.9f);
                builder.BuildOption(typeof(BattleStateOption).Name);
            }

              GameCharacter gameCharacter = new GameCharacter();
           
            int sad = DamageCalculator.CalculateDamage(gameCharacter.CharacterStatContainer, gameCharacter.CharacterStatContainer);

            SkillCastCommand ssad = builder.Build();
            gameCharacter.Health = 10;
            Console.WriteLine(ssad?.IsCanSkillCast(gameCharacter) + " <<");
            ssad.OnSkillCast(gameCharacter);
            /*
              ssad?.OnSkillCast(gameCharacter);

             while (true)
             {
                 gameCharacter.CharacterBattleStateMachine.TimeCheck();
                 string input = Console.ReadLine();
             }*/
        }
    }
}
// stage
// 애니메이션 연동
// 스킬 등록
// 재화
// 장비 등록 - 장착제한 계산
// 버프 등록 - 딜공식 재배치
// 스테이지 등록
