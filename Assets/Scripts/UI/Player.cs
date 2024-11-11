using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Player : AbstractInventory
    {
        [ContextMenu("RemoveQickLinks")]
        private void RemoveLinks()
        {
            for (int i = 0; i < 10; i++)
                PlayerPrefs.SetInt($"QuickPanel{i}", -1);
        }

        public void SetEnterID(int id) => _enterID = id;

        public PlayerComponent.Inventory PlayerInventory;

        [SerializeField] private Image[] _quickPanelImages;
        private int[] _quickPanelLinks = new int[10];

        [SerializeField] private Ammunition _inventoryAmmunition;
        private AbstractInventory _secondInventory;
        private int _enterID = -1;

        private void OnEnable()
        {
            bool _isLinks = PlayerPrefs.HasKey($"QuickPanel0");

            for (int i = 0; i < 10; i++)
                _quickPanelLinks[i] = _isLinks ? PlayerPrefs.GetInt($"QuickPanel{i}") : -1;
        }

        private void OnDisable()
        {
            for (int i = 0; i < 10; i++)
                PlayerPrefs.SetInt($"QuickPanel{i}", _quickPanelLinks[i]);
        }

        private void Start()
        {
            _inventoryTargetPosition.x = -400;
            _inventoryPosition.x = -400;
        }

        private void Update()
        {
            for (int i = 48; i < 58; i++)
                if (Input.GetKeyDown((KeyCode)i))
                    InteractQuickPanel(i == 48 ? 9 : i - 49);
        }

        public override void AddItem(ref Slot slot) => PlayerInventory.AddItem(ref slot);

        public override void DeleteItem(int id)
        {
            _secondInventory.AddItem(ref PlayerInventory.Slots[id]);
            PlayerInventory.UpdateMenu();
        }

        public override void UpdateMenu(Slot[] slots)
        {
            base.UpdateMenu(slots);

            for (int i = 0; i < 10; i++)
            {
                _quickPanelImages[i].enabled = _quickPanelLinks[i] != -1 && slots[_quickPanelLinks[i]].Count != 0;

                if (_quickPanelImages[i].enabled)
                    _quickPanelImages[i].sprite = slots[_quickPanelLinks[i]].Item.Sprite;
                else
                    _quickPanelLinks[i] = -1;
            }
        }

        public void SetSecondInventory(AbstractInventory inventory)
        {
            _secondInventory = inventory;
            _inventoryTargetPosition.y = inventory ? 0 : -1000;
        }

        private void InteractQuickPanel(int selectID)
        {
            if (_enterID != -1)
            {
                for (int i = 0; i < 10; i++)
                    if (_quickPanelLinks[i] == _enterID)
                    {
                        _quickPanelLinks[i] = -1;
                        _quickPanelImages[i].enabled = false;

                        break;
                    }

                _quickPanelLinks[selectID] = _enterID;
                _quickPanelImages[selectID].enabled = true;
                _quickPanelImages[selectID].sprite = PlayerInventory.Slots[_enterID].Item.Sprite;

                return;
            }

            if (!_quickPanelImages[selectID].enabled) return;

            _inventoryAmmunition.AddItem(ref PlayerInventory.Slots[_quickPanelLinks[selectID]]);
            PlayerInventory.UpdateMenu();
        }
    }
}