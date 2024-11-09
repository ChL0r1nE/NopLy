using UnityEngine;

namespace UI
{
    public class SwitchMenu : MonoBehaviour
    {
        [SerializeField] private Player _inventoryPlayer;
        private AbstractInventory _nowInventory;

        private void Start() => _inventoryPlayer = FindObjectOfType<Player>();

        public void SetMenu(AbstractInventory inventory, bool isOpenInventory)
        {
            if (!isOpenInventory)
            {
                _nowInventory = null;
                _inventoryPlayer.SetSecondInventory(null);
                return;
            }

            _nowInventory?.SwitchOpen(false);

            _nowInventory = inventory;
            _inventoryPlayer.SetSecondInventory(_nowInventory);
        }
    }
}
