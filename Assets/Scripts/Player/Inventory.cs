using UnityEngine;

namespace PlayerComponent
{
    public class Inventory : MonoBehaviour
    {
        public void UpdateMenu() => _inventoryPlayer.UpdateMenu(Slots);

        public Slot[] Slots;

        private UI.Player _inventoryPlayer;
        private UI.LootList _lootList;
        private int _slotCount;

        public void Inizilize()
        {
            _lootList = FindObjectOfType<UI.LootList>();
            _inventoryPlayer = FindObjectOfType<UI.Player>();
            _inventoryPlayer.PlayerInventory = this;
            _inventoryPlayer.UpdateMenu(Slots);
        }

        public void AddItem(ref Slot slot, bool showLoot = false)
        {
            int remain = slot.Count;
            _slotCount = slot.Count;

            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(slot.Count, out remain);
                slot.Count = remain;

                if (remain != 0) continue;

                _inventoryPlayer.UpdateMenu(Slots);

                if (showLoot)
                    _lootList.AddLootLabel(foreachSlot.Item.Sprite, foreachSlot.Item.Name, _slotCount);

                return;
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item) continue;

                if (slot is WeaponSlot weaponSlot)
                    Slots[i] = new WeaponSlot(slot.Item, remain, weaponSlot.Endurance);
                else
                    Slots[i] = new(slot.Item, remain);

                slot.Count = 0;

                _inventoryPlayer.UpdateMenu(Slots);

                if (showLoot)
                    _lootList.AddLootLabel(Slots[i].Item.Sprite, Slots[i].Item.Name, _slotCount);

                return;
            }

            if (showLoot && _slotCount - remain != 0)
                _lootList.AddLootLabel(slot.Item.Sprite, slot.Item.Name, _slotCount - remain);
        }

        public bool DeleteSlots(Slot[] slots)
        {
            bool canDeleteSlot;
            bool isFreeSlot = false;

            foreach (Slot recipeSlot in slots)
            {
                canDeleteSlot = false;
                _slotCount = recipeSlot.Count;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    canDeleteSlot = slot.Count - _slotCount >= 0;
                    isFreeSlot |= slot.Count - _slotCount == 0;
                    _slotCount = slot.Count - _slotCount;

                    if (canDeleteSlot) break;
                }

                if (!canDeleteSlot) return false;
            }

            if (!isFreeSlot)
            {
                foreach (Slot slot in Slots)
                    isFreeSlot |= !slot.Item;

                if (!isFreeSlot)
                    return false;
            }

            foreach (Slot recipeSlot in slots)
            {
                _slotCount = recipeSlot.Count;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    slot.DeleteCount(_slotCount, out _slotCount);

                    if (_slotCount == 0) break;
                }
            }

            return true;
        }
    }
}
