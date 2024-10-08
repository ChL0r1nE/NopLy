using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    [SerializeField] private InventoryUI.Player _inventoryPlayer;
    private InventoryUI.AbstractInventory _nowInventory;

    private void Start() => _inventoryPlayer = FindObjectOfType<InventoryUI.Player>();

    public void SetMenu(InventoryUI.AbstractInventory inventory, bool isOpenInventory)
    {
        if (!isOpenInventory)
        {
            _nowInventory = null;
            _inventoryPlayer.SetSecondInventory(_nowInventory);
            return;
        }

        _nowInventory?.SwitchOpen(false);

        _nowInventory = inventory;
        _inventoryPlayer.SetSecondInventory(_nowInventory);
    }
}
