using UnityEngine;

public enum ItemType
{
    Material,
    Potion,
    Seed,
    Weapon,
    Armor
}

[System.Serializable]
public record Slot
{
    public Slot(ItemInfo info, int count)
    {
        Info = info;
        _count = count;
    }

    public int Count
    {
        get => _count;
        set
        {
            _count = value;

            if (_count == 0)
                Info = null;
        }
    }

    public ItemInfo Info;

    [SerializeField] private int _count;

    public void AddCount(int count, out int remain)
    {
        remain = _count + count > Info.MaxStack ? _count + count - Info.MaxStack : 0;
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

[CreateAssetMenu(fileName = "New ItemInfo", menuName = "ItemInfo", order = 1)]
public class ItemInfo : ScriptableObject
{
    public Sprite Sprite;
    public ItemType Type;
    public string Name;
    public int ID;
    public int MaxStack;
}