using System;
using System.Data;

namespace CJH_Frame.CharacterBase
{
    enum BattleStateType
    {
        None = -1,
        DamageOverTime = 0,
        StatBuff = 1,
        StatDebuff = 2,
    }

    public struct BattleStateData
    {
        public string Name;

        public float TimeGap;
        public int RepeatCount;
        public int StackCount;
    }

    public class CharacterBattleStateMachine
    {
        GameCharacter CharacterStatContainer;
        float time = 0;
        private List<BattleStateUnit> list = new();

        public CharacterBattleStateMachine(GameCharacter CharacterStatContainer)
        {
            this.CharacterStatContainer = CharacterStatContainer;
        }

        public void ApplyEffect(GameCharacter caster, BattleStateData battleState)
        {
            BattleStateUnit data = new BattleStateUnit()
            {
                fix = battleState,
                LastTime = time,
                CurrCount = 0,
                OnAction = () => Console.WriteLine($"{battleState} is work")
            };

            data.OnAction += () => Console.WriteLine($"{battleState} is work");
            list.Add(data);
        }

        public void TimeCheck()
        {
            time++;

            Console.WriteLine($" time : {time}");

            for (int i = 0; i < list.Count; i++)
            {
                BattleStateUnit target = list[i];
                BattleStateData temp = target.fix;

                if (time > temp.TimeGap + target.LastTime)
                {
                    target.CurrCount++;
                    target.LastTime = time;
                    target.OnAction?.Invoke();
                }

                Console.WriteLine($"{temp.Name} : {target.CurrCount} / {temp.RepeatCount}");

                if (temp.RepeatCount <= target.CurrCount)
                {
                    Console.WriteLine($"{temp.Name} : is Remove");

                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        class BattleStateUnit
        {
            public BattleStateData fix;

            public float LastTime;
            public int CurrCount;
            public Action? OnAction;
        }
    }
}
