using UnityEngine.UI;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField] protected Image[] _images;
    [SerializeField] protected Text[] _textes;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected int _maxSlotCount;
    protected InventoryPlayerScript _inventoryPlayerScript;
    protected int _nowSlotCount;
    protected bool _isOpen = false;

    private void Awake() => _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();

    public virtual void AddItem(Slot slot) { }

    public virtual void DeleteItem(int id) { }

    public virtual bool CanAddItem(Slot slot) => _nowSlotCount + 1 <= _maxSlotCount;

    public virtual void SwitchMenu(bool isThere, string AnimName)
    {
        _isOpen = !_isOpen && isThere;
        _animator.SetBool(AnimName, _isOpen);

        if (_isOpen)
            _inventoryPlayerScript.SecondInventory = this;
    }

    public virtual void UpdateMenu(Slot[] slots, bool WithText = true)
    {
        _nowSlotCount = 0;
        _maxSlotCount = slots.Length;

        foreach (Slot slot in slots)
        {
            if (slot.Info != null)
                _nowSlotCount++;
        }

        for (int i = 0; i < _maxSlotCount; i++)
        {
            if (i < slots.Length && slots[i].Info != null)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].sprite = slots[i].Info.Sprite;

                if (WithText)
                    _textes[i].text = slots[i].Count != 1 ? slots[i].Count.ToString() : "";

            }
            else
                _images[i].gameObject.SetActive(false);
        }
    }
}
