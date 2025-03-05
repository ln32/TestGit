using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJH_Frame.CharacterBase.CharacterSkill
{
    public class SkillEquipment
    {
        private readonly Dictionary<int, Skill> _equippedSkills = new();

        public Skill? GetSkill(int index) =>
            _equippedSkills.TryGetValue(index, out var item) ? item : null;

        public bool Equip(int index, Skill item)
        {
            if (_equippedSkills.ContainsKey(index))
                return false; // 이미 장착된 아이템이 있음

            _equippedSkills[index] = item;
            return true;
        }

        public bool Unequip(int index)
        {
            return _equippedSkills.Remove(index);
        }
    }
}
