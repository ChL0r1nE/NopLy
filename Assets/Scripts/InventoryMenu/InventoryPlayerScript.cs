using UnityEngine.UI;
using UnityEngine;

public class InventoryPlayerScript : Inventory
{
    [SerializeField] private Image[] _quickPanelImages = new Image[10];
    private int[] _quickPanelLink = new int[10];

    [SerializeField] private PlayerInventoryScript _playerInventory;
    [SerializeField] private InventoryAmmunitionScript _inventoryAmmunition;
    private Inventory _secondInventory;
    private int _enterID;

    public void SetSecondInventory(Inventory inventory)
    {
        _secondInventory = inventory;
        _isOpen = inventory;
    }

    public PlayerInventoryScript GetPlayerInventoryScript() => _playerInventory;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
            _quickPanelLink[i] = -1;

        _inventoryTargetPosition.x = -400;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            InteractQuickPanel(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            InteractQuickPanel(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            InteractQuickPanel(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            InteractQuickPanel(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            InteractQuickPanel(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            InteractQuickPanel(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            InteractQuickPanel(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            InteractQuickPanel(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            InteractQuickPanel(8);
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            InteractQuickPanel(9);
    }

    private void InteractQuickPanel(int selectID)
    {
        if (_enterID == -1)
        {
            if (_quickPanelLink[selectID] == -1 || !_inventoryAmmunition.CanAddItem(_playerInventory.GetSlot(_quickPanelLink[selectID]))) return;

            Slot slot = _playerInventory.GetSlot(_quickPanelLink[selectID]);

            _playerInventory.DeleteItem(_quickPanelLink[selectID]);
            _inventoryAmmunition.AddItem(slot);

            if (_playerInventory.Slots[_quickPanelLink[selectID]].Count == 0)
            {
                _quickPanelImages[selectID].gameObject.SetActive(false);
                _quickPanelLink[selectID] = -1;
            }

            return;
        }

        for (int i = 0; i < 10; i++)
        {
            if (_quickPanelLink[i] == _enterID)
            {
                _quickPanelImages[i].gameObject.SetActive(false);
                _quickPanelLink[i] = -1;
            }
        }

        _quickPanelLink[selectID] = _enterID;
        _quickPanelImages[selectID].sprite = _playerInventory.Slots[_enterID].Info.Sprite;
        _quickPanelImages[selectID].gameObject.SetActive(true);
    }

    public void SetEnterID(int id) => _enterID = id;

    public override void AddItem(Slot slot) => _playerInventory.AddItem(slot);

    public override void DeleteItem(int id)
    {
        if (!_secondInventory.CanAddItem(_playerInventory.GetSlot(id))) return;

        Slot slot = _playerInventory.GetSlot(id);

        _playerInventory.DeleteItem(id);
        _secondInventory.AddItem(slot);
    }
}
