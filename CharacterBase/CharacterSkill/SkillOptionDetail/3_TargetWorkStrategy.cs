using System.Numerics;

namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    // 4. TargetWorkStrategy 딜, cc, 타겟 논타겟
    public abstract class TargetWorkStrategy : ISkillOption
    {
        public string OptionName => typeof(TargetWorkStrategy).Name;

        public abstract void WorkToTarget(GameCharacter caster, GameCharacter target);
    }
}


namespace CJH_Frame.CharacterBase.CharacterSkill.SkillOptionDetail
{
    // 투사체 or 즉발 or 설치기
    public class ProjectileOption : TargetWorkStrategy
    {

        Vector3 CastV3 { get; set; }
        Vector3 TargetV3 { get; set; }

        public ProjectileOption(Vector3 CastV3, Vector3 TargetV3)
        {
            this.CastV3 = CastV3;
            this.TargetV3 = TargetV3;

            Console.WriteLine(CastV3 + " / " + TargetV3);
        }

        public override void WorkToTarget(GameCharacter caster, GameCharacter target)
        {
            Console.WriteLine(OptionName + " <<");
        }
    }

    public class BattleStateOption : TargetWorkStrategy
    {
        private string Name { get; set; }
        private int RepeatCount { get; set; }
        private int StackCount { get; set; } // StackCounr < 0 => false;
        private float TimeGap { get; set; }

        public BattleStateOption(string Name, int RepeatCount, int StackCount, float TimeGap)
        {
            this.Name = Name;
            this.RepeatCount = RepeatCount;
            this.TimeGap = TimeGap;
            this.StackCount = StackCount;

            Console.WriteLine($"{Name} state : {RepeatCount} * {TimeGap} ");
        }

        public BattleStateData Data => new BattleStateData
        {
            Name = Name,
            RepeatCount = RepeatCount,
            TimeGap = TimeGap,
            StackCount = StackCount,
        };

        public override void WorkToTarget(GameCharacter caster, GameCharacter target)
        {
            target.CharacterBattleStateMachine.ApplyEffect(caster, Data);
        }
    }
}