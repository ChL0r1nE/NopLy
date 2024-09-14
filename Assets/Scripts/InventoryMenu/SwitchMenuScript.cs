using UnityEngine;

public class SwitchMenuScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Inventory _nowInventory; //pub
    public bool isOpen = false; //pub

    public void SetMenu(Inventory inventory, string animName, bool isOpen)
    {
        if (_nowInventory && _nowInventory != inventory)
            _nowInventory.SetOpen(false);

        _nowInventory = inventory;
        _animator.SetTrigger(isOpen ? animName : "Close");
    }
}
