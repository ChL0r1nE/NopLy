using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public abstract class AbstractInventory : MonoBehaviour
    {
        [SerializeField] protected Image[] _images;
        [SerializeField] protected Text[] _textes;

        [SerializeField] protected SwitchMenu _switchMenu;
        protected Player _inventoryPlayer;
        protected Vector2 _inventoryTargetPosition = new(400f, -1000f);
        protected Vector2 _inventoryPosition = new(400f, -1000f);
        protected int _slotsCount;
        protected bool _isOpen = false;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _slotsCount = _images.Length;
            _rectTransform = GetComponent<RectTransform>();
            _inventoryPlayer = FindObjectOfType<Player>();
        }

        private void LateUpdate()
        {
            if (_rectTransform.anchoredPosition.y == _inventoryTargetPosition.y) return;

            _inventoryPosition.y = Mathf.MoveTowards(_rectTransform.anchoredPosition.y, _inventoryTargetPosition.y, Time.deltaTime * 5000);
            _rectTransform.anchoredPosition = _inventoryPosition;
        }

        public virtual void AddItem(ref Slot slot) { }

        public virtual void DeleteItem(int id) { }

        public virtual void SwitchOpen(bool baseOpen)
        {
            _isOpen = !_isOpen && baseOpen;
            _inventoryTargetPosition.y = _isOpen ? 0f : -1000f;

            _switchMenu.SetMenu(this, _isOpen);
        }

        public virtual void UpdateMenu(Slot[] slots)
        {
            for (int i = 0; i < _slotsCount; i++)
            {
                _images[i].gameObject.SetActive(slots[i].Item);

                if (!slots[i].Item) continue;

                _images[i].sprite = slots[i].Item.Sprite;
                _textes[i].text = slots[i].Count != 1 ? $"{slots[i].Count}" : "";
            }
        }
    }
}