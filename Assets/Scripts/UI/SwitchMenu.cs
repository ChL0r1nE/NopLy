using UnityEngine;

namespace UI
{
    public class SwitchMenu : MonoBehaviour
    {
        [SerializeField] private Player _inventoryPlayer;
        private AbstractInventory _secondInventory;

        public void SetMenu(AbstractInventory inventory, bool isOpen)
        {
            if (!isOpen)
            {
                _secondInventory = null;
                _inventoryPlayer.SetSecondInventory(null);
                return;
            }

            _secondInventory?.SwitchOpen(false);

            _secondInventory = inventory;
            _inventoryPlayer.SetSecondInventory(_secondInventory);
        }
    }
}
