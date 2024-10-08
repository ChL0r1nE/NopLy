using UnityEngine;

namespace Interact
{
    public class Worktable : AbstractInteract
    {
        [System.Serializable]
        public class Recipe
        {
            public Slot[] Materials;
            public Slot Result;
        }

        public Recipe[] Recipes;

        private InventoryUI.Worktable _inventoryWorktable;
        private bool _isOpen = false;

        public void SetOpen(bool isOpen) => _isOpen = isOpen;

        private void Start() => _inventoryWorktable = FindObjectOfType<InventoryUI.Worktable>();

        public override void Interact()
        {
            _isOpen = !_isOpen;
            _inventoryWorktable.WorktableStrategy = this;
            _inventoryWorktable.SwitchOpen(true);
        }

        private void OnTriggerExit(Collider col)
        {
            if (!_isOpen || !col.CompareTag("Player")) return;

            _inventoryWorktable.SwitchOpen(false);
            _isOpen = false;
        }
    }
}
