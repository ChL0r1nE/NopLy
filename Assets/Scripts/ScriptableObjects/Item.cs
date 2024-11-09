using UnityEngine;

[System.Serializable]
public record Slot
{
    public Slot(Info.Item item, int count)
    {
        Item = item;
        _count = count;
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

    public void AddCount(int count, out int remain)
    {
        remain = _count + count > Item.MaxStack ? _count + count - Item.MaxStack : 0;
        _count = _count + count - remain;
    }

    public void DeleteCount(int count, out int remain)
    {
        remain = _count - count < 0 ? count - _count : 0;
        Count = _count - count + remain;
    }

    public bool CanDeleteCount(int count, out int remain)
    {
        int mayCount = _count - count;
        remain = mayCount < 0 ? -mayCount : 0;

        return mayCount >= 0;
    }
}

[System.Serializable]
public record WeaponSlot : Slot
{
    public WeaponSlot(Info.Item item, int count, int endurance) : base(item, count)
    {
        Item = item;
        Endurance = endurance;
        _count = count;
    }

    public int Endurance;

    public bool IsWhole(int wear)
    {
        Endurance -= wear;
        return Endurance > 0;
    }
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