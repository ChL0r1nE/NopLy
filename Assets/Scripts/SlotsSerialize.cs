using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public record ItemRecord
{
    public int ID;
    public int Count;
}

[System.Serializable]
public record WeaponRecord : ItemRecord
{
    public int Endurance;
}

[System.Serializable]
public record SlotsData
{
    public SlotsData(Slot[] slots)
    {
        if (slots == null) return;

        ItemRecords = new ItemRecord[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].Item) continue;

            if (slots[i] is WeaponSlot weaponSlot)
                ItemRecords[i] = new WeaponRecord { ID = weaponSlot.Item.ID, Count = weaponSlot.Count, Endurance = weaponSlot.Endurance };
            else
                ItemRecords[i] = new ItemRecord { ID = slots[i].Item.ID, Count = slots[i].Count };
        }
    }

    public ItemRecord[] ItemRecords;
}

public class SlotsSerialize : MonoBehaviour
{
    private static readonly BinaryFormatter _formatter = new();

    private FileStream _file;

    public void DeserializeData(Slot[] slots, string ID)
    {
        if (!File.Exists($"{Application.persistentDataPath}/{ID}.dat")) return;

        _file = File.Open($"{Application.persistentDataPath}/{ID}.dat", FileMode.Open);
        SlotsData data = (SlotsData)_formatter.Deserialize(_file);
        _file.Close();

        for (int i = 0; i < slots.Length; i++)
        {
            if (data.ItemRecords[i] == null) continue;

            for (int j = 0; j < ItemDictionary.Instance.Items.Length; j++)
            {
                if (ItemDictionary.Instance.Items[j].ID != data.ItemRecords[i].ID) continue;

                if (data.ItemRecords[i] is WeaponRecord weaponRecord)
                    slots[i] = new WeaponSlot(ItemDictionary.Instance.Items[j], weaponRecord.Count, weaponRecord.Endurance);
                else
                    slots[i] = new(ItemDictionary.Instance.Items[j], data.ItemRecords[i].Count);
            }
        }
    }

    public void SerializeData(Slot[] data, string name)
    {
        _file = File.Create($"{Application.persistentDataPath}/{name}.dat");
        _formatter.Serialize(_file, new SlotsData(data));
        _file.Close();
    }
}
