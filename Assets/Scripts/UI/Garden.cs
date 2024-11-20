namespace UI
{
    public class Garden : AbstractInventory
    {
        public Interact.Garden GardenStrategy;

        public override void AddItem(ref Slot slot) => GardenStrategy.AddSeed(ref slot);

        public override void DeleteItem(int id)
        {
            _inventoryPlayer.AddItem(ref GardenStrategy.Slots[id]);
            GardenStrategy.UpdateNullMeshes();
            GardenStrategy.UpdateMenu();
        }

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            GardenStrategy.SetOpen(_isOpen);
        }
    }
}