namespace CJH_Frame.Item
{
    public interface IItem
    {
        string ItemId { get; }
    }


    public interface IEquipItem : IItem
    {   
        string Slot { get; }
        void Equip(GameCharacter target);
        void Unequip(GameCharacter target);
    }
}
