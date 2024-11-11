using UnityEngine;

public class Loot : MonoBehaviour
{
    public Slot Slot;
    public int WeaponEndurance;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.TryGetComponent(out PlayerComponent.Inventory inventory)) return;

        if (WeaponEndurance != 0)
            Slot = new WeaponSlot(Slot.Item, Slot.Count, WeaponEndurance);

        inventory.AddItem(ref Slot, true);

        if (!Slot.Item)
            Destroy(gameObject);
    }
}
