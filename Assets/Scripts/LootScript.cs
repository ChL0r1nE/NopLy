using UnityEngine;

public class LootScript : MonoBehaviour
{
    public Slot Slot;

    private void Start() => Slot.Info.Object = gameObject;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerInventoryScript>().AddItem(Slot, out int remain, true);
    }
}
