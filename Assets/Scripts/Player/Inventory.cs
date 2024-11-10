using UnityEngine;

namespace PlayerComponent
{
    public class Inventory : MonoBehaviour
    {
        public Slot[] Slots;

        private UI.Player _inventoryPlayer;
        private UI.LootList _lootList;
        private int _slotCount;

        private void Start()
        {
            _lootList = FindObjectOfType<UI.LootList>();
            _inventoryPlayer = FindObjectOfType<UI.Player>();
            _inventoryPlayer.PlayerInventory = this;

            _inventoryPlayer.UpdateMenu(Slots);
        }

        public void SetSlotCount(int i, int count)
        {
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
            bool canDeleteRecipe = true;
            bool canDeleteSlot;

            foreach (Slot recipeSlot in recipe)
            {
                _slotCount = recipeSlot.Count;
                canDeleteSlot = false;

                foreach (Slot slot in Slots)
                {
                    if (slot.Item != recipeSlot.Item) continue;

                    canDeleteSlot |= slot.CanDeleteCount(_slotCount, out int remain);
                    _slotCount = remain;

                    if (remain == 0) break;
                }

                canDeleteRecipe &= canDeleteSlot;

                if (!canDeleteRecipe) return false;
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
