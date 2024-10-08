namespace InventoryUI
{
    public class Worktable : AbstractInventory //MonoBeh?
    {
        public Interact.Worktable WorktableStrategy;

        public override void SwitchOpen(bool baseOpen) //ToInterface
        {
            base.SwitchOpen(baseOpen);

            if (!baseOpen)
                WorktableStrategy.SetOpen(false);
        }

        public void CraftItem(int id)
        {
            if (_inventoryPlayer.PlayerInventory.DeleteRecipe(WorktableStrategy.Recipes[id].Materials))
                _inventoryPlayer.AddItem(new(WorktableStrategy.Recipes[id].Result.Info, WorktableStrategy.Recipes[id].Result.Count), out int remain);
        }
    }
}