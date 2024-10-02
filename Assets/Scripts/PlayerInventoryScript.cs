using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public Slot GetSlot(int id) => Slots[id];

    public Slot[] Slots;

    private InventoryPlayerScript _inventoryPlayerScript;

    private void Start()
    {
        _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();
        _inventoryPlayerScript.UpdateMenu(Slots);
    }

    public void AddItem(Slot slot, bool destroyObject = false)
    {
        if (destroyObject)
            Destroy(slot.Info.Object);

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info && Slots[i].Info.ID == slot.Info.ID && Slots[i].Count != Slots[i].Info.MaxStack)
            {
                Slots[i].AddCount(slot.Count, out int remain);

                if (remain != 0)
                    slot = new Slot(slot.Info, remain);
                else
                {
                    _inventoryPlayerScript.UpdateMenu(Slots);
                    return;
                }
            }
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info == null)
            {
                Slots[i] = slot;
                _inventoryPlayerScript.UpdateMenu(Slots);

                return;
            }
        }
    }

    public void DeleteItem(int id)
    {
        Slots[id] = new Slot(null, 0);
        _inventoryPlayerScript.UpdateMenu(Slots);
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
                }
            }
        }

        return true;
    }
}
