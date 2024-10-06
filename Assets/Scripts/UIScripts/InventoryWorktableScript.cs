using UnityEngine;

public class InventoryWorktableScript : Inventory //MonoBeh?
{
    private WorktableStrategy _worktableStrategy;

    public void SetStrategy(WorktableStrategy strategy) => _worktableStrategy = strategy;

    public override void SwitchOpen(bool baseOpen) //ToInterface
    {
        base.SwitchOpen(baseOpen);

        if (!baseOpen)
            _worktableStrategy.SetOpen(false);
    }

    public void CraftItem(int id)
    {
        if (_inventoryPlayerScript.GetPlayerInventoryScript().DeleteRecipe(_worktableStrategy.Recipes[id].Materials))
            _inventoryPlayerScript.AddItem(new(_worktableStrategy.Recipes[id].Result.Info, _worktableStrategy.Recipes[id].Result.Count), out int remain);
    }
}
