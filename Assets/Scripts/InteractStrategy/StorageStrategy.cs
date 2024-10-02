using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StorageStrategy : Strategy
{
    public Slot GetInfo(int id) => Slots[id];

    public Slot[] Slots = new Slot[10];

    public void SetOpen(bool isOpen)
    {
        _isOpen = isOpen;
        _animator.SetBool("IsOpen", _isOpen);
    }

    [SerializeField] private Animator _animator;
    private InventoryStorageScript _inventoryStorageScript;
    private bool _isOpen = false;

    private void Start() => _inventoryStorageScript = FindObjectOfType<InventoryStorageScript>();

    private void OnTriggerExit()
    {
        if (_isOpen)
            _inventoryStorageScript.SwitchOpen(false);
    }

    public override void Interact()
    {
        _inventoryStorageScript.SetStrategy(this);
        _inventoryStorageScript.SwitchOpen(true);

        if (_isOpen)
            _inventoryStorageScript.UpdateMenu(Slots);
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
                    _inventoryStorageScript.UpdateMenu(Slots);
                    return;
                }
            }
        }

        countRemain = slot.Count;

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info) continue;

            Slots[i] = slot;
            countRemain = 0;
            break;
        }

        _inventoryStorageScript.UpdateMenu(Slots);
    }

    public void DeleteItem(int id)
    {
        Slots[id] = new(null, 0);
        _inventoryStorageScript.UpdateMenu(Slots);
    }
}
