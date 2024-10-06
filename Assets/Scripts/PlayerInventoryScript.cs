using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public Slot[] Slots;

    private InventoryPlayerScript _inventoryPlayerScript;
    private LootListScript _lootListScript;
    private int _slotCount;

    private void Start()
    {
        _lootListScript = FindObjectOfType<LootListScript>();
        _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();
        _inventoryPlayerScript.UpdateMenu(Slots);
    }

    public void SetSlotCount(int i, int count)
    {
        Slots[i].Count = count;
        _inventoryPlayerScript.UpdateMenu(Slots);
    }

    public void AddItem(Slot slot, out int countRemain, bool showLoot = false)
    {
        countRemain = slot.Count;
        _slotCount = slot.Count;

        foreach (Slot foreachSlot in Slots)
        {
            if (!foreachSlot.Info || foreachSlot.Info.ID != slot.Info.ID) continue;

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
                if (Slots[i].Info) continue;

                Slots[i] = slot;
                countRemain = 0;
                break;
            }

        _inventoryPlayerScript.UpdateMenu(Slots);

        if (showLoot && countRemain != _slotCount)
            _lootListScript.AddLootLabel(slot.Info, _slotCount - countRemain);
    }

    public bool DeleteRecipe(Slot[] recipe)
    {
        bool CanDeleteRecipe = true;
        bool CanDeleteSlot;

        foreach (Slot recipeSlot in recipe)
        {
            _slotCount = recipeSlot.Count;
            CanDeleteSlot = false;

            foreach (Slot slot in Slots)
            {
                if (slot.Info != recipeSlot.Info) continue;

                CanDeleteSlot |= slot.CanDeleteCount(_slotCount, out int remain);
                _slotCount = remain;

                if (remain == 0) break;
            }

            CanDeleteRecipe &= CanDeleteSlot;

            if (!CanDeleteRecipe) return false;
        }

        foreach (Slot recipeSlot in recipe)
        {
            _slotCount = recipeSlot.Count;

            foreach (Slot slot in Slots)
            {
                if (slot.Info != recipeSlot.Info) continue;

                slot.DeleteCount(_slotCount, out int remain);
                _slotCount = remain;

                if (remain == 0) break;
            }
        }

        return true;
    }
}
