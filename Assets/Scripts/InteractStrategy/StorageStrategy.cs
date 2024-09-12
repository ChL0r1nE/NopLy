using UnityEngine;

public class StorageStrategy : Strategy
{
    public Slot GetInfo(int id) => Slots[id];

    public Slot[] Slots = new Slot[10];

    private Animator _animator;
    private InventoryStorageScript _inventoryStorageScript;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _inventoryStorageScript = FindObjectOfType<InventoryStorageScript>();
    }

    public override void Interact()
    {
        PlayerWeaponScript.CanAttack = !PlayerWeaponScript.CanAttack;

        _isOpen = !_isOpen;
        _animator.SetTrigger("SwitchStorage");
        _inventoryStorageScript.SwitchMenu(true, "InventoryStorage");
        _inventoryStorageScript.SetStrategy(this);
        _inventoryStorageScript.UpdateMenu(Slots);
    }

    private void OnTriggerExit(Collider col)
    {
        PlayerWeaponScript.CanAttack = true;

        if (col.CompareTag("Player"))
        {
            if (_isOpen)
            {
                _isOpen = false;
                _animator.SetTrigger("SwitchStorage");
            }

            _inventoryStorageScript.SwitchMenu(false, "InventoryStorage");
        }
    }

    public void AddItem(Slot slot)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info != null && Slots[i].Info.ID == slot.Info.ID && Slots[i].Count != Slots[i].Info.MaxStack)
            {
                Slots[i].AddCount(slot.Count, out int remain);

                if (remain != 0)
                    slot = new Slot(slot.Info, remain);
                else
                {
                    _inventoryStorageScript.UpdateMenu(Slots);
                    return;
                }
            }
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Info == null)
            {
                Slots[i] = slot;
                _inventoryStorageScript.UpdateMenu(Slots);

                return;
            }
        }
    }

    public void DeleteItem(int id)
    {
        Slots[id] = new Slot(null, 0);
        _inventoryStorageScript.UpdateMenu(Slots);
    }
}
