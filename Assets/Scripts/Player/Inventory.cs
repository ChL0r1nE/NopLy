using UnityEngine;

namespace PlayerComponent
{
    public class Inventory : MonoBehaviour
    {
        public Slot[] Slots;

        private UI.Player _inventoryPlayer;
        private UI.LootList _lootList;
        private int _slotCount;

        public void Inizilize()
        {
            _lootList = FindObjectOfType<UI.LootList>();
            _inventoryPlayer = FindObjectOfType<UI.Player>();
            _inventoryPlayer.PlayerInventory = this;
        }

        public void SetSlotCount(int i, int count)
        {
            if (i != -1)
                Slots[i].Count = count;

            _inventoryPlayer.UpdateMenu(Slots);
        }

        public void AddItem(Slot slot, out int countRemain, bool showLoot = false)
        {
            countRemain = slot.Count;
            _slotCount = slot.Count;

            foreach (Slot foreachSlot in Slots)
            {
                if (!foreachSlot.Item || foreachSlot.Item.ID != slot.Item.ID) continue;

                foreachSlot.AddCount(slot.Count, out int remain);
                countRemain = remain;

                if (countRemain != 0)
                    slot.Count = countRemain;
                else
                    break;
            }

            if (countRemain != 0)
                for (int i = 0; i < Slots.Length; i++)
                {
                    if (Slots[i].Item) continue;

                    Slots[i] = slot;
                    countRemain = 0;
                    break;
                }

            _inventoryPlayer.UpdateMenu(Slots);

            if (showLoot && countRemain != _slotCount)
                _lootList.AddLootLabel(slot.Item, _slotCount - countRemain);
        }

        public bool DeleteRecipe(Slot[] recipe)
        {
            bool canDeleteSlot;
            bool isFreeSlot = false;

            foreach (Slot recipeSlot in recipe)
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

            foreach (Slot recipeSlot in recipe)
            {
                _slotCount = recipeSlot.Count;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    slot.DeleteCount(_slotCount, out int remain);
                    _slotCount = remain;

                    if (remain == 0) break;
                }
            }

            return true;
        }
    }
}
