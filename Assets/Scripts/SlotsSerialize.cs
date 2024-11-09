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

            if (slots[i].Item.GetType() == typeof(Info.Weapon))
                ItemRecords[i] = new WeaponRecord { ID = slots[i].Item.ID, Count = slots[i].Count, Endurance = (slots[i] as WeaponSlot).Endurance };
            else
                ItemRecords[i] = new ItemRecord { ID = slots[i].Item.ID, Count = slots[i].Count };
        }
    }

    public ItemRecord[] ItemRecords;
}


public class SlotsSerialize : MonoBehaviour
{
    private FileStream _file;

    private static readonly BinaryFormatter _formatter = new();

    public SlotsData DeserializeData(string name)
    {
        if (!File.Exists($"{Application.persistentDataPath}/{name}.dat"))
            return new(null);

        _file = File.Open($"{Application.persistentDataPath}/{name}.dat", FileMode.Open);
        SlotsData data = (SlotsData)_formatter.Deserialize(_file);
        _file.Close();

        return data;
    }

    public void SerializeData(SlotsData inData, string name)
    {
        _file = File.Create($"{Application.persistentDataPath}/{name}.dat");
        _formatter.Serialize(_file, inData);
        _file.Close();
    }
}
