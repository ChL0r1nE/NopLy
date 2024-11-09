using UnityEngine;

namespace Interact
{
    [System.Serializable]
    public class Recipe
    {
        public Slot[] Materials;
        public Slot Result;
    }

    public class Worktable : AbstractInteract
    {
        public void SetOpen(bool isOpen) => _isOpen = isOpen;

        public Recipe[] Recipes;

        private UI.Craft _inventoryCraft;
        private bool _isOpen = false;

        private void Start() => _inventoryCraft = FindObjectOfType<UI.Craft>();

        public override void Interact()
        {
            _isOpen = !_isOpen;
            _inventoryCraft.WorktableStrategy = this;
            _inventoryCraft.SwitchOpen(true);

            if (_isOpen)
                _inventoryCraft.UpdateCraftMenu(Recipes);
        }

        private void OnTriggerExit(Collider col)
        {
            if (!_isOpen || !col.CompareTag("Player")) return;

            _inventoryCraft.SwitchOpen(false);
            _isOpen = false;
        }
    }
}
