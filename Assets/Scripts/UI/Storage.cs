namespace UI
{
    public class Storage : AbstractInventory
    {
        public Interact.Storage StorageStrategy;

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            StorageStrategy.SetOpen(_isOpen);
        }

        public override void DeleteItem(int id)
        {
            _inventoryPlayer.AddItem(StorageStrategy.Slots[id], out _);
            StorageStrategy.DeleteItem(id);
        }

        public override void AddItem(Slot slot, out int countRemain)
        {
            StorageStrategy.AddItem(slot, out int remain);
            countRemain = remain;
        }
    }
}