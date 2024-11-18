using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public record PlayerInventory : Slots
    {
        public PlayerInventory(Slot[] slots, WeaponSlot weapon, float health) : base(slots)
        {
            SlotsSerialize(slots);

            if (weapon.Item != null)
                WeaponRecord = new Weapon { ID = weapon.Item.ID, Count = weapon.Count, Endurance = weapon.Endurance };
            else
                WeaponRecord = null;

            Health = health;
        }

        public Weapon WeaponRecord;
        public float Health;
    }
}

namespace PlayerComponent
{
    public class Serialize : MonoBehaviour
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
            _formatter.Serialize(_file, new Data.PlayerInventory(slots.ToArray(), new(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.Endurance), GetComponent<Health>().HealthValue));
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
            Data.PlayerInventory data = (Data.PlayerInventory)_formatter.Deserialize(_file);
            _file.Close();

            GetComponent<Health>().HealthValue = data.Health;
            _isWeapon = data.WeaponRecord != null;

            if (!_isWeapon)
                _playerWeapon.SetWeaponSlot(null);

            for (int i = 10; i < 15; i++)
            {
                if (data.ItemRecords[i].ID < 0)
                    _playerArmor.SetDefaultArmor(i - 10);
            }

            if (_isWeapon)
                _playerWeapon.SetWeaponSlot(new(ItemDictionary.Instance.GetInfo(data.WeaponRecord.ID), data.WeaponRecord.Count, data.WeaponRecord.Endurance));

            for (int i = 0; i < 10; i++)
            {
                if (data.ItemRecords[i] == null) continue;

                if (data.ItemRecords[i] is Data.Weapon weaponRecord)
                    _playerInventory.Slots[i] = new WeaponSlot(ItemDictionary.Instance.GetInfo(data.ItemRecords[i].ID), weaponRecord.Count, weaponRecord.Endurance);
                else
                    _playerInventory.Slots[i] = new(ItemDictionary.Instance.GetInfo(data.ItemRecords[i].ID), data.ItemRecords[i].Count);
            }

            for (int i = 10; i < 15; i++)
                if (data.ItemRecords[i].ID > 0)
                    _playerArmor.SetArmor(ItemDictionary.Instance.GetInfo(data.ItemRecords[i].ID) as Info.Armor);

            _playerInventory.Inizilize();
        }
    }
}
