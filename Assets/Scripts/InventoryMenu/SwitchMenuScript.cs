using UnityEngine;

public class SwitchMenuScript : MonoBehaviour
{
    [SerializeField] private InventoryPlayerScript _inventoryPlayerScript;
    private Inventory _nowInventory;

    private void Start() => _inventoryPlayerScript = FindObjectOfType<InventoryPlayerScript>();

    public void SetMenu(Inventory inventory)
    {
        if (!_nowInventory)
            _nowInventory = inventory;
        else if (_nowInventory != inventory)
        {
            Debug.Log("Other");
            _nowInventory.SetOpenStrategy(false);
            _nowInventory = inventory;
        }
        else
            _nowInventory = null;

        _inventoryPlayerScript.SetSecondInventory(_nowInventory);
    }
}
