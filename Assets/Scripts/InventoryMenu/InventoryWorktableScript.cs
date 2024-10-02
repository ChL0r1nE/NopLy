public class InventoryWorktableScript : Inventory
{
    private WorktableStrategy _worktableStrategy;

    public void SetStrategy(WorktableStrategy strategy) => _worktableStrategy = strategy;

    public override void SwitchOpen(bool baseOpen)
    {
        base.SwitchOpen(baseOpen);

        if (!baseOpen)
            _worktableStrategy.SetOpen(false);
    }

    public void CraftItem(int id)
    {
        if (_inventoryPlayerScript.GetPlayerInventoryScript().DeleteRecipe(_worktableStrategy.Recipes[id].Materials))
            _inventoryPlayerScript.AddItem(_worktableStrategy.Recipes[id].Result, out int remain);
    }
}
