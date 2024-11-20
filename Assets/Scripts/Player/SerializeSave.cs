using System.Collections.Generic;
using System.Linq;
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
    public class SerializeSave : MonoBehaviour
    {
        private Inventory _playerInventory;
        private Weapon _playerWeapon;
        private Armor _playerArmor;

        private readonly Serialize _serialize = new();

        private void OnDisable()
        {
            List<Slot> slots = GetComponent<Inventory>().Slots.ToList();

            foreach (Info.Armor armor in GetComponent<Armor>().Armors)
                slots.Add(new(armor, 1));

            _serialize.CreateSave("Player", new Data.PlayerInventory(slots.ToArray(), new(_playerWeapon.GetInfoWeapon(), 1, _playerWeapon.Endurance), GetComponent<Health>().HealthValue));
        }

        private void Start()
        {
            _playerInventory = GetComponent<Inventory>();
            _playerWeapon = GetComponent<Weapon>();
            _playerArmor = GetComponent<Armor>();
            _playerWeapon.Inizilize();
            _playerArmor.Inizilize();

            if (!_serialize.ExistSave("Player"))
            {
                _playerWeapon.SetWeaponSlot(null);
                GetComponent<Health>().HealthValue = 100;

                for (int i = 0; i < 5; i++)
                    _playerArmor.SetDefaultArmor(i);

                _playerInventory.Inizilize();
                return;
            }

            Data.PlayerInventory inventory = _serialize.LoadSave<Data.PlayerInventory>("Player");
            GetComponent<Health>().HealthValue = inventory.Health;

            _playerWeapon.SetWeaponSlot(inventory.WeaponRecord == null ? null : new(ItemDictionary.Instance.GetInfo(inventory.WeaponRecord.ID), inventory.WeaponRecord.Count, inventory.WeaponRecord.Endurance));

            for (int i = 0; i < 10; i++)
            {
                if (inventory.ItemRecords[i] == null) continue;

                if (inventory.ItemRecords[i] is Data.Weapon weaponRecord)
                    _playerInventory.Slots[i] = new WeaponSlot(ItemDictionary.Instance.GetInfo(inventory.ItemRecords[i].ID), weaponRecord.Count, weaponRecord.Endurance);
                else
                    _playerInventory.Slots[i] = new(ItemDictionary.Instance.GetInfo(inventory.ItemRecords[i].ID), inventory.ItemRecords[i].Count);
            }

            for (int i = 10; i < 15; i++)
            {
                if (inventory.ItemRecords[i].ID > 0)
                    _playerArmor.SetArmor(ItemDictionary.Instance.GetInfo(inventory.ItemRecords[i].ID) as Info.Armor);
                else
                    _playerArmor.SetDefaultArmor(i - 10);
            }

            _playerInventory.Inizilize();
        }
    }
}
