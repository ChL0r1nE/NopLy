public class WorktableStrategy : Strategy
{
    [System.Serializable]
    public class Recipe
    {
        public Slot[] Materials;
        public Slot Result;
    }

    public Recipe[] Recipes;

    private InventoryWorktableScript _inventoryWorktableScript;

    private void Start() => _inventoryWorktableScript = FindObjectOfType<InventoryWorktableScript>();

    public override void Interact()
    {
        _inventoryWorktableScript.SetStrategy(this);
        _inventoryWorktableScript.SwitchMenu(true, "InventoryWorktable");
    }
}
