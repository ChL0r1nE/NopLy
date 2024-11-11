namespace UI
{
    public class Storage : AbstractInventory
    {
        public Interact.Storage StorageStrategy { private get; set; }

        public override void AddItem(ref Slot slot) => StorageStrategy.AddItem(ref slot);

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);
            StorageStrategy.SetOpen(_isOpen);
        }

        public override void DeleteItem(int id)
        {
            _inventoryPlayer.AddItem(ref StorageStrategy.Slots[id]);
            StorageStrategy.UpdateMenu();
        }
    }
}