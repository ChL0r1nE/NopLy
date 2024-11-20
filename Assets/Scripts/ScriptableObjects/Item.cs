using UnityEngine;

[System.Serializable]
public record Slot
{
    public Slot(Info.Item item, int count)
    {
        Item = item;
        Count = count;
    }

    public int Count
    {
        get => _count;
        set
        {
            _count = value;

            if (_count == 0)
                Item = null;
        }
    }

    public Info.Item Item;

    [SerializeField] protected int _count;

    public void AddCount(ref int count)
    {
        if (_count + count > Item.MaxStack)
        {
            count = _count + count - Item.MaxStack;
            _count = Item.MaxStack;

            return;
        }

        _count += count;
        count = 0;
    }

    public void DeleteCount(ref int count)
    {
        if (_count <= count)
        {
            count -= _count;
            Count = 0;

            return;
        }

        _count -= count;
        count = 0;
    }
}

[System.Serializable]
public record WeaponSlot : Slot
{
    public WeaponSlot(Info.Item item, int count, int endurance) : base(item, count)
    {
        Item = item;
        Endurance = endurance;
        Count = count;
    }

    public int Endurance;
}

namespace Info
{
    public enum ItemType
    {
        Material,
        Potion,
        Seed,
        Weapon,
        Armor
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 1)]
    public class Item : ScriptableObject
    {
        public Sprite Sprite;
        public ItemType Type;
        public string Name;
        public int ID;
        public int MaxStack;
    }
}