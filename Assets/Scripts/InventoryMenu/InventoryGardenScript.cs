public class InventoryGardenScript : Inventory
{
    private GardenStrategy _gardenStrategy;

    public void SetStrategy(GardenStrategy strategy) => _gardenStrategy = strategy;

    public override void SwitchOpen(bool baseOpen)
    {
        base.SwitchOpen(baseOpen);
        _gardenStrategy.SetOpen(_isOpen);
    }

    public override void AddItem(Slot slot, out int countRemain)
    {
        _gardenStrategy.AddSeed(slot, out int remain);
        countRemain = remain;
    }

    public override void DeleteItem(int id)
    {
        Slot slot = new(_gardenStrategy.Slots[id].Info, _gardenStrategy.Slots[id].Count);

        _inventoryPlayerScript.AddItem(slot, out int remain);
        _gardenStrategy.SetSlotCount(id, remain);
    }
}
