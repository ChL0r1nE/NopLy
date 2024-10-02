using UnityEngine;

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
    private bool _isOpen = false;

    public void SetOpen(bool isOpen) => _isOpen = isOpen;

    private void Start() => _inventoryWorktableScript = FindObjectOfType<InventoryWorktableScript>();

    public override void Interact()
    {
        _isOpen = !_isOpen;
        _inventoryWorktableScript.SetStrategy(this);
        _inventoryWorktableScript.SwitchOpen(true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!_isOpen || !col.CompareTag("Player")) return;

        _inventoryWorktableScript.SwitchOpen(false);
        _isOpen = false;
    }
}
