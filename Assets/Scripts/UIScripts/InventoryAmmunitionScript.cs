using UnityEngine;

public class InventoryAmmunitionScript : Inventory
{
    public void SetPlayerAmmunitionScript(PlayerArmorScript playerArmorScript) => _playerArmorScript = playerArmorScript;

    [SerializeField] private ArmorInfo[] _defaultArmors;

    [SerializeField] private CameraFollowingScript _cameraFollowingScript;
    [SerializeField] private PlayerHealthScript _playerHealthScript;
    [SerializeField] private WeaponInfo _defaultMainWeapon;
    private PlayerWeaponScript _playerMainWeaponScript;
    private PlayerArmorScript _playerArmorScript;

    private void Start()
    {
        _inventoryTargetPosition.x = -310;
        _playerArmorScript = FindObjectOfType<PlayerArmorScript>();
        _playerMainWeaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponScript>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchOpen(true);
    }

    public override void SwitchOpen(bool baseOpen)
    {
        base.SwitchOpen(baseOpen);
        _cameraFollowingScript.SwitchToPlayer();
    }

    public override void AddItem(Slot slot, out int countRemain)
    {
        countRemain = slot.Count - 1;

        switch (slot.Info.Type)
        {
            case ItemType.Food:
                FoodInfo foodInfo = slot.Info as FoodInfo;
                _playerHealthScript.Heal(foodInfo.Health);
                break;
            case ItemType.Weapon:
                WeaponInfo weaponInfo = slot.Info as WeaponInfo;

                if (_playerMainWeaponScript.WeaponInfo != _defaultMainWeapon)
                    _inventoryPlayerScript.AddItem(new(_playerMainWeaponScript.WeaponInfo, 1), out int remain); //AddItemOff

                if (weaponInfo.WeaponPlace == WeaponPlace.Main)
                    _playerMainWeaponScript.SetWeapon(weaponInfo);

                break;
            case ItemType.Armor:
                ArmorInfo armorInfo = slot.Info as ArmorInfo;
                _playerArmorScript.SetArmor(armorInfo);
                break;
            default:
                countRemain = slot.Count;
                break;
        }

        UpdateMenu(null);
    }

    public override void DeleteItem(int id)
    {
        if (id == 0)
        {
            _inventoryPlayerScript.AddItem(new(_playerMainWeaponScript.WeaponInfo, 1), out int remain);

            if (remain == 0)
                _playerMainWeaponScript.SetWeapon(_defaultMainWeapon);
        }
        else
        {
            _inventoryPlayerScript.AddItem(new(_playerArmorScript.Armors[id - 2], 1), out int remain);

            if (remain == 0)
                _playerArmorScript.SetArmor(_defaultArmors[id - 2]);
        }

        UpdateMenu(null);
    }

    public override void UpdateMenu(Slot[] slots)
    {
        _images[0].gameObject.SetActive(_playerMainWeaponScript.WeaponInfo.WeaponType != WeaponType.Default);

        if (_playerMainWeaponScript.WeaponInfo.WeaponType != WeaponType.Default)
            _images[0].sprite = _playerMainWeaponScript.WeaponInfo.Sprite;

        for (int i = 0; i < 5; i++)
        {
            _images[i + 2].gameObject.SetActive(_playerArmorScript.Armors[i].Sprite);

            if (_playerArmorScript.Armors[i].Sprite)
                _images[i + 2].sprite = _playerArmorScript.Armors[i].Sprite;
        }
    }
}
