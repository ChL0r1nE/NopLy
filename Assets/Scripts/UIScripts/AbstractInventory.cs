using UnityEngine.UI;
using UnityEngine;

namespace InventoryUI
{
    public abstract class AbstractInventory : MonoBehaviour
    {
        [SerializeField] protected Image[] _images;
        [SerializeField] protected Text[] _textes;

        [SerializeField] protected SwitchMenu _switchMenu;
        protected Player _inventoryPlayer;
        protected Vector2 _inventoryTargetPosition = new(400, -1000);
        protected int _maxSlotCount;
        protected bool _isOpen = false;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _maxSlotCount = _images.Length;
            _rectTransform = GetComponent<RectTransform>();
            _inventoryPlayer = FindObjectOfType<Player>();
        }

        private void LateUpdate()
        {
            if (_rectTransform.anchoredPosition.y != _inventoryTargetPosition.y)
                _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _inventoryTargetPosition, Time.deltaTime * 5000);
        }

        public virtual void AddItem(Slot slot, out int countRemain) => countRemain = slot.Count;

        public virtual void DeleteItem(int id) { }

        public virtual void SwitchOpen(bool baseOpen)
        {
            _isOpen = !_isOpen && baseOpen;
            _inventoryTargetPosition.y = _isOpen ? 0 : -1000;

            _switchMenu.SetMenu(this, _isOpen);
        }

        public virtual void UpdateMenu(Slot[] slots)
        {
            for (int i = 0; i < _maxSlotCount; i++)
            {
                _images[i].gameObject.SetActive(slots[i].Info);

                if (!slots[i].Info) continue;

                _images[i].sprite = slots[i].Info.Sprite;
                _textes[i].text = slots[i].Count != 1 ? $"{slots[i].Count}" : "";
            }
        }
    }
}