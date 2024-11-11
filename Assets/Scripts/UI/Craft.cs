using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Craft : AbstractInventory
    {
        public Interact.Worktable WorktableStrategy;

        [SerializeField] private List<GameObject> _icons = new();

        private Vector3 _slotPosition = new(-160, 40);
        private int _iconCount = 0;

        public override void SwitchOpen(bool baseOpen)
        {
            base.SwitchOpen(baseOpen);

            if (!baseOpen)
                WorktableStrategy.SetOpen(false);
        }

        public void CraftItem(int id)
        {
            if (_inventoryPlayer.PlayerInventory.DeleteSlots(WorktableStrategy.Recipes[id].Materials))
            {
                if (WorktableStrategy.Recipes[id].Result.Item is Info.Weapon weaponInfo)
                {
                    Info.Weapon infoWeapon = weaponInfo;
                    Slot slot = new WeaponSlot(infoWeapon, WorktableStrategy.Recipes[id].Result.Count, infoWeapon.MaxEndurance);
                    _inventoryPlayer.AddItem(ref slot);
                }
                else
                {
                    Slot slot = new(WorktableStrategy.Recipes[id].Result.Item, WorktableStrategy.Recipes[id].Result.Count);
                    _inventoryPlayer.AddItem(ref slot);
                }
            }
        }

        public void UpdateCraftMenu(Interact.Recipe[] recipes)
        {
            _iconCount = 0;

            for (int i = 0; i < recipes.Length; i++)
            {
                _slotPosition.y = 40 - 80 * i;

                for (int j = 0; j < recipes[i].Materials.Length; j++)
                {
                    _slotPosition.x = -160 + j * 80;
                    _icons[_iconCount].SetActive(true);
                    _icons[_iconCount].GetComponent<RectTransform>().anchoredPosition = _slotPosition;
                    _icons[_iconCount].transform.GetChild(0).GetComponent<Image>().sprite = recipes[i].Materials[j].Item.Sprite;
                    _icons[_iconCount++].transform.GetChild(1).GetComponent<Text>().text = recipes[i].Materials[j].Count != 1 ? $"{recipes[i].Materials[j].Count}" : "";
                }

                _images[i].enabled = true;
                _images[i].sprite = recipes[i].Result.Item.Sprite;
            }

            for (int i = recipes.Length; i < _images.Length; i++)
                _images[i].enabled = false;

            for (int i = _iconCount; i < _icons.Count; i++)
                _icons[i].SetActive(false);
        }
    }
}