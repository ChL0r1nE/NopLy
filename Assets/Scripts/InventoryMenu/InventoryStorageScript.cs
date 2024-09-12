public class InventoryStorageScript : Inventory
{
    private StorageStrategy _storageStrategy;

    public void SetStrategy(StorageStrategy strategy) => _storageStrategy = strategy;

    public override void DeleteItem(int id)
    {
        _inventoryPlayerScript.AddItem(_storageStrategy.GetInfo(id));
        _storageStrategy.DeleteItem(id);
    }

    public override void AddItem(Slot slot) => _storageStrategy.AddItem(slot);
}
