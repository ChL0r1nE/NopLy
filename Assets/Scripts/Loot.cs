using UnityEngine;

public class Loot : MonoBehaviour
{
    public Slot Slot;
    public WeaponSlot WeaponSlot;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        col.GetComponent<PlayerComponent.Inventory>().AddItem(WeaponSlot.IsWhole(0) ? WeaponSlot : Slot, out int remain, true);

        if (remain == 0)
            Destroy(gameObject);
    }
}
