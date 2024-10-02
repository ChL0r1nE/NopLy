public class InventoryStorageScript : Inventory
{
    private StorageStrategy _storageStrategy;

    public void SetStrategy(StorageStrategy strategy) => _storageStrategy = strategy;

    public override void SwitchOpen(bool baseOpen)
    {
        base.SwitchOpen(baseOpen);
        _storageStrategy.SetOpen(_isOpen);
    }

    public override void DeleteItem(int id)
    {
        _inventoryPlayerScript.AddItem(_storageStrategy.GetInfo(id), out int remain);
        _storageStrategy.DeleteItem(id);
    }

    public override void AddItem(Slot slot, out int countRemain)
    {
        _storageStrategy.AddItem(slot, out int remain);
        countRemain = remain;
    }
}
