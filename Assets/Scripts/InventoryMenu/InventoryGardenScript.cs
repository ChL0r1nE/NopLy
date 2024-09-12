public class InventoryGardenScript : Inventory
{
    private GardenStrategy _gardenStrategy;

    public override bool CanAddItem(Slot slot) => _gardenStrategy.CanAddSeed(slot);

    public void SetStrategy(GardenStrategy strategy) => _gardenStrategy = strategy;

    public override void AddItem(Slot slot)
    {
        if(slot.Count > 1)
            _inventoryPlayerScript.AddItem(new Slot(slot.Info, slot.Count - 1));

        _gardenStrategy.AddSeed(slot);
    }

    public override void DeleteItem(int id)
    {
        _inventoryPlayerScript.AddItem(_gardenStrategy.GetInfo(id));
        _gardenStrategy.DeleteItem(id);
    }
}
