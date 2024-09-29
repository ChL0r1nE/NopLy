using UnityEngine.UI;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField] protected Image[] _images;
    [SerializeField] protected Text[] _textes;
    [SerializeField] protected SwitchMenuScript _switchMenuScript;
    protected InventoryPlayerScript _inventoryPlayerScript;
    protected Vector2 _inventoryTargetPosition = new(400, 0);
    protected int _maxSlotCount;
    protected int _nowSlotCount;
    protected bool _isOpen = false;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();
    }

    private void LateUpdate()
    {
        _inventoryTargetPosition.y = _isOpen ? 0 : -1000;
        _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _inventoryTargetPosition, 30);
    }

    public virtual void AddItem(Slot slot) { }

    public virtual void DeleteItem(int id) { }

    public virtual bool CanAddItem(Slot slot) => _nowSlotCount + 1 <= _maxSlotCount;

    public virtual void SwitchOpen(bool baseOpen)
    {
        _isOpen = !_isOpen && baseOpen;
        _switchMenuScript.SetMenu(this);
    }

    public virtual void UpdateMenu(Slot[] slots, bool WithText = true)
    {
        _nowSlotCount = 0;
        _maxSlotCount = slots.Length;

        foreach (Slot slot in slots)
        {
            if (slot.Info)
                _nowSlotCount++;
        }

        for (int i = 0; i < _maxSlotCount; i++)
        {
            if (i < slots.Length && slots[i].Info)
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
