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

public class SlotsSerialize
{
    private static readonly BinaryFormatter _formatter = new();

    private FileStream _file;

    public void DeserializeData(Slot[] slots, string fileName)
    {
        if (!File.Exists($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat")) return;

        _file = File.Open($"{UnityEngine.Application.persistentDataPath}/{fileName}.dat", FileMode.Open);
        Data.Slots data = (Data.Slots)_formatter.Deserialize(_file);
        _file.Close();

        for (int i = 0; i < slots.Length; i++)
        {
            if (data.ItemRecords[i] == null) continue;

            for (int j = 0; j < ItemDictionary.Instance.Items.Length; j++)
            {
                if (ItemDictionary.Instance.Items[j].ID != data.ItemRecords[i].ID) continue;

                if (data.ItemRecords[i] is Data.Weapon weaponRecord)
                    slots[i] = new WeaponSlot(ItemDictionary.Instance.Items[j], weaponRecord.Count, weaponRecord.Endurance);
                else
                    slots[i] = new(ItemDictionary.Instance.Items[j], data.ItemRecords[i].Count);
            }
        }
    }

    public void SerializeData(Slot[] slots, string name)
    {
        _file = File.Create($"{UnityEngine.Application.persistentDataPath}/{name}.dat");
        _formatter.Serialize(_file, new Data.Slots(slots));
        _file.Close();
    }
}
