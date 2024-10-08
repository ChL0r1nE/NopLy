namespace InventoryUI
{
    public class Garden : AbstractInventory
    {
        public Interact.Garden GardenStrategy;

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            GardenStrategy.IsOpen = _isOpen;
        }

        public override void AddItem(Slot slot, out int countRemain)
        {
            GardenStrategy.AddSeed(slot, out int remain);
            countRemain = remain;
        }

        public override void DeleteItem(int id)
        {
            Slot slot = new(GardenStrategy.Slots[id].Info, GardenStrategy.Slots[id].Count);

            _inventoryPlayer.AddItem(slot, out int remain);
            GardenStrategy.SetSlotCount(id, remain);
        }
    }
}