using UnityEngine;

public class Loot : MonoBehaviour
{
    public Slot Slot;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        col.GetComponent<PlayerComponent.Inventory>().AddItem(Slot, out int remain, true);

        if (remain == 0)
            Destroy(gameObject);
    }
}
