using UnityEngine;

public enum ItemType
{
    Material,
    Food,
    Seed,
    Weapon,
    Armor
}

[System.Serializable]
public class Slot
{
    public Slot(ItemInfo info, int count)
    {
        Info = info;
        _count = count;
    }

    public int Count
    {
        get { return _count; }
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
        _count = Mathf.Clamp(_count - count, 0, Info.MaxStack);

        if (_count == 0)
            Info = null;
    }

    public bool CanDeleteCount(int count, out int remain)
    {
        int May_count = _count - count;
        remain = May_count < 0 ? -May_count : 0;

        return May_count >= 0;
    }
}

[CreateAssetMenu(fileName = "New ItemInfo", menuName = "ItemInfo", order = 1)]
public class ItemInfo : ScriptableObject
{
    [HideInInspector] public GameObject Object = null;

    [Header("Main")]
    public ItemType Type;
    public int ID;

    public Sprite Sprite;
    public string TestSt;
    public int MaxStack;
}