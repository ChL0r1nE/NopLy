using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public Slot[] Slots;

    private InventoryPlayerScript _inventoryPlayerScript;

    private void Start()
    {
        _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();
        _inventoryPlayerScript.UpdateMenu(Slots);
    }

    public void SetSlotCount(int i, int count)
    {
        Slots[i].Count = count;
        _inventoryPlayerScript.UpdateMenu(Slots);
    }

    public void AddItem(Slot slot, out int countRemain)
    {
        countRemain = 0;

        foreach (Slot foreachSlot in Slots)
        {
            if (foreachSlot.Info && foreachSlot.Info.ID == slot.Info.ID)
            {
                foreachSlot.AddCount(slot.Count, out int remain);

                if (remain != 0)
                    slot.Count = remain;
                else
                {
                    _inventoryPlayerScript.UpdateMenu(Slots);
                    return;
                }
            }
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info) continue;

            Slots[i] = slot;
            _inventoryPlayerScript.UpdateMenu(Slots);
            return;
        }

        _inventoryPlayerScript.UpdateMenu(Slots);
        countRemain = slot.Count;
    }

    public bool DeleteRecipe(Slot[] recipe)
    {
        bool CanDeleteRecipe = true;

        foreach (Slot recipeSlot in recipe)
        {
            int remainCount = recipeSlot.Count;
            bool CanDeleteSlot = false;

            foreach (Slot slot in Slots)
            {
                if (slot.Info == recipeSlot.Info)
                {
                    CanDeleteSlot |= slot.CanDeleteCount(remainCount, out int remain);

                    remainCount = remain;

                    if (remain == 0) break;
                }
            }

            CanDeleteRecipe &= CanDeleteSlot;

            if (!CanDeleteRecipe)
                return false;
        }

        foreach (Slot recipeSlot in recipe)
        {
            int remainCount = recipeSlot.Count;

            foreach (Slot slot in Slots)
            {
                if (slot.Info == recipeSlot.Info)
                {
                    slot.DeleteCount(remainCount, out int remain);
                    remainCount = remain;

                    if (remain == 0) break;
                }
            }
        }

        return true;
    }
}
