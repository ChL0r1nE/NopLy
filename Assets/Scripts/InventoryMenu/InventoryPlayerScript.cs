public class InventoryPlayerScript : Inventory
{
    private Inventory _secondInventory;

    private PlayerInventoryScript _playerInventory;
    private Slot _slot;

    public void SetSecondInventory(Inventory inventory)
    {
        _isOpen = inventory;
        _secondInventory = inventory;
    }

    public PlayerInventoryScript GetPlayerInventoryScript() => _playerInventory;

    private void Start()
    {
        _playerInventory = FindObjectOfType<PlayerInventoryScript>();
        _inventoryTargetPosition.x = -400;
    }

    public override void AddItem(Slot slot) => _playerInventory.AddItem(slot);

    public override void DeleteItem(int id)
    {
        if (!_secondInventory.CanAddItem(_playerInventory.GetInfo(id))) return;

        _slot = _playerInventory.GetInfo(id);

        _playerInventory.DeleteItem(id);
        _secondInventory.AddItem(_slot);
    }
}
