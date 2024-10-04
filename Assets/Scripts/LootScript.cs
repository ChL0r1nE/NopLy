using UnityEngine;

public class LootScript : MonoBehaviour
{
    public Slot Slot;

    private void Start() => Slot.Info.Object = gameObject;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        col.GetComponent<PlayerInventoryScript>().AddItem(Slot, out int remain);

        if (remain == 0)
            Destroy(gameObject);
    }
}
