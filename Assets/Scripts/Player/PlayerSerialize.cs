using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PlayerComponent
{
    [System.Serializable]
    public record PlayerData : SlotsData
    {
        public PlayerData(Slot[] slots, WeaponSlot weapon, float health) : base(slots)
        {
            if (slots == null) return;

            ItemRecords = new ItemRecord[slots.Length];

            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].Item) continue;

                if (slots[i] is WeaponSlot weaponSlot)
                    ItemRecords[i] = new WeaponRecord { ID = slots[i].Item.ID, Count = slots[i].Count, Endurance = weaponSlot.Endurance };
                else
                    ItemRecords[i] = new ItemRecord { ID = slots[i].Item.ID, Count = slots[i].Count };
            }

            if (weapon.Item != null)
                WeaponRecord = new WeaponRecord { ID = weapon.Item.ID, Count = weapon.Count, Endurance = weapon.Endurance };
            else
                WeaponRecord = null;

            Health = health;
        }

        public WeaponRecord WeaponRecord;
        public float Health;
    }

    public class PlayerSerialize : MonoBehaviour
    {
        private BinaryFormatter _formatter = new();
        private FileStream _file;
        private Inventory _playerInventory;
        private Weapon _playerWeapon;
        private Armor _playerArmor;
        private bool _isWeapon;

        private void OnDisable()
        {
            List<Slot> slots = GetComponent<Inventory>().Slots.ToList();

            foreach (Info.Armor armor in GetComponent<Armor>().Armors)
                slots.Add(new(armor, 1));

            _file = File.Create($"{Application.persistentDataPath}/Player.dat");
            _formatter.Serialize(_file, new PlayerData(slots.ToArray(), new(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.Endurance), GetComponent<Health>().HealthValue));
            _file.Close();
        }

        private void Start()
        {
            _playerInventory = GetComponent<Inventory>();
            _playerWeapon = GetComponent<Weapon>();
            _playerArmor = GetComponent<Armor>();
            _playerWeapon.Inizilize();
            _playerArmor.Inizilize();

            if (!File.Exists($"{Application.persistentDataPath}/Player.dat"))
            {
                _playerWeapon.SetWeaponSlot(null);
                GetComponent<Health>().HealthValue = 100;

                for (int i = 0; i < 5; i++)
                    _playerArmor.SetDefaultArmor(i);

                _playerInventory.Inizilize();
                return;
            }

            _file = File.Open($"{Application.persistentDataPath}/Player.dat", FileMode.Open);
            PlayerData data = (PlayerData)_formatter.Deserialize(_file);
            _file.Close();

            _isWeapon = data.WeaponRecord != null;
            GetComponent<Health>().HealthValue = data.Health;

            if (!_isWeapon)
                _playerWeapon.SetWeaponSlot(null);

            for (int i = 10; i < 15; i++)
            {
                if (data.ItemRecords[i].ID < 0)
                    _playerArmor.SetDefaultArmor(i - 10);
            }

            foreach (Info.Item item in ItemDictionary.Instance.Items)
            {
                if (_isWeapon && data.WeaponRecord.ID == item.ID)
                    _playerWeapon.SetWeaponSlot(new(item, data.WeaponRecord.Count, data.WeaponRecord.Endurance));

                for (int i = 0; i < 10; i++)
                {
                    if (data.ItemRecords[i] == null || item.ID != data.ItemRecords[i].ID) continue;

                    if (data.ItemRecords[i] is WeaponRecord weaponRecord)
                        _playerInventory.Slots[i] = new WeaponSlot(item, weaponRecord.Count, weaponRecord.Endurance);
                    else
                        _playerInventory.Slots[i] = new(item, data.ItemRecords[i].Count);
                }

                List<Info.Armor> armors = new();

                for (int i = 10; i < 15; i++)
                {
                    if (data.ItemRecords[i] == null || item.ID != data.ItemRecords[i].ID) continue;

                    armors.Add(item as Info.Armor);
                    _playerArmor.SetArmor(armors.ToArray());
                }
            }

            _playerInventory.Inizilize();
        }
    }
}
