using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Data
{
    [System.Serializable]
    public record Item
    {
        public int ID;
        public int Count;
    }

    [System.Serializable]
    public record Weapon : Item
    {
        public int Endurance;
    }

    [System.Serializable]
    public record Slots
    {
        public Slots(Slot[] slots) => SlotsSerialize(slots);

        public Item[] ItemRecords;

        protected void SlotsSerialize(Slot[] slots)
        {
            if (slots == null) return;

            ItemRecords = new Item[slots.Length];

            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].Item) continue;

                if (slots[i] is WeaponSlot weaponSlot)
                    ItemRecords[i] = new Weapon { ID = weaponSlot.Item.ID, Count = weaponSlot.Count, Endurance = weaponSlot.Endurance };
                else
                    ItemRecords[i] = new Item { ID = slots[i].Item.ID, Count = slots[i].Count };
            }
        }
    }
}

public class Serialize
{
    private static readonly BinaryFormatter _formatter = new();

    private FileStream _file;

    public bool ExistSave(string fileName) => File.Exists($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat");

    public T LoadSave<T>(string fileName)
    {
        if (!File.Exists($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat"))
            return default;

        _file = File.Open($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat", FileMode.Open);
        T data = (T)_formatter.Deserialize(_file);
        _file.Close();

        return data;
    }

    public void CreateSave(string fileName, object data)
    {
        _file = File.Create($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat");
        _formatter.Serialize(_file, data);
        _file.Close();
    }

    public void Records2Slots(Data.Item[] itemRecords, Slot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (itemRecords[i] == null) continue;

            if (itemRecords[i] is Data.Weapon weaponRecord)
                slots[i] = new WeaponSlot(ItemDictionary.Instance.GetInfo(itemRecords[i].ID), weaponRecord.Count, weaponRecord.Endurance);
            else
                slots[i] = new(ItemDictionary.Instance.GetInfo(itemRecords[i].ID), itemRecords[i].Count);
        }
    }

    public Data.Item[] Slots2Record(Slot[] slots)
    {
        Data.Item[] itemRecords = new Data.Item[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].Item) continue;

            if (slots[i] is WeaponSlot weaponSlot)
                itemRecords[i] = new Data.Weapon { ID = weaponSlot.Item.ID, Count = weaponSlot.Count, Endurance = weaponSlot.Endurance };
            else
                itemRecords[i] = new Data.Item { ID = slots[i].Item.ID, Count = slots[i].Count };
        }

        return itemRecords;
    }
}
