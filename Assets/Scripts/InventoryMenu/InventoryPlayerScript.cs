public class InventoryPlayerScript : Inventory
{
    public Inventory SecondInventory;

    private PlayerInventoryScript _playerInventory;
    private Slot _slot;

    public PlayerInventoryScript GetPlayerInventoryScript() => _playerInventory;

    private void Start() => _playerInventory = FindObjectOfType<PlayerInventoryScript>();

    public override void AddItem(Slot slot) => _playerInventory.AddItem(slot);

    public override void DeleteItem(int id)
    {
        if (!SecondInventory.CanAddItem(_playerInventory.GetInfo(id))) return;

        _slot = _playerInventory.GetInfo(id);

        _playerInventory.DeleteItem(id);
        SecondInventory.AddItem(_slot);
    }
}
