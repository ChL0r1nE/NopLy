public class InventoryWorktableScript : Inventory
{
    private WorktableStrategy _worktableStrategy;

    public void SetStrategy(WorktableStrategy strategy) => _worktableStrategy = strategy;

    public void CraftItem(int id)
    {
        if (_inventoryPlayerScript.GetPlayerInventoryScript().DeleteRecipe(_worktableStrategy.Recipes[id].Materials))
            _inventoryPlayerScript.AddItem(_worktableStrategy.Recipes[id].Result);
    }
}
