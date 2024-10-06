using UnityEngine;

public class SwitchMenuScript : MonoBehaviour
{
    [SerializeField] private InventoryPlayerScript _inventoryPlayerScript;
    private Inventory _nowInventory;

    private void Start() => _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();

    public void SetMenu(Inventory inventory, bool isOpenInventory)
    {
        if (!isOpenInventory)
        {
            _nowInventory = null;
            _inventoryPlayerScript.SetSecondInventory(_nowInventory);
            return;
        }

        if (_nowInventory)
            _nowInventory.SwitchOpen(false);

        _nowInventory = inventory;
        _inventoryPlayerScript.SetSecondInventory(_nowInventory);
    }
}
