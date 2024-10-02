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
        Count = count;
    }

    public ItemInfo Info;
    public int Count;

    public void AddCount(int count, out int remain)
    {
        remain = Count + count > Info.MaxStack ? Count + count - Info.MaxStack : 0;
        Count = Mathf.Clamp(Count + count, 0, Info.MaxStack);
    }

    public void DeleteCount(int count, out int remain)
    {
        remain = Count - count < 0 ? count - Count : 0;
        Count = Mathf.Clamp(Count - count, 0, Info.MaxStack);

        if (Count == 0)
            Info = null;
    }

    public bool CanDeleteCount(int count, out int remain)
    {
        int MayCount = Count - count;
        remain = MayCount < 0 ? -MayCount : 0;

        return MayCount >= 0;
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