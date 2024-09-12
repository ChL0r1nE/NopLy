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

    public void SetCount(int count) => Count = count;

    public void AddCount(int count, out int remain)
    {
        remain = 0;
        Count += count;

        if (Count > Info.MaxStack)
        {
            remain = Count - Info.MaxStack;
            Count = Info.MaxStack;
        }
    }

    public void DeleteCount(int count, out int remain)
    {
        remain = 0;
        Count -= count;

        if (Count <= 0)
        {
            remain = -Count;
            Info = null;
        }
    }

    public bool CanDeleteCount(int count, out int remain)
    {
        remain = 0;
        int MayCount = Count - count;

        if (MayCount >= 0)
            return true;
        else
        {
            remain = -MayCount;
            return false;
        }
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