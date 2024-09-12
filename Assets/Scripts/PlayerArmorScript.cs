using UnityEngine;

public class PlayerArmorScript : MonoBehaviour
{
    public MeshFilter[] ArmorMeshes = new MeshFilter[5];
    public ArmorInfo[] Armors = new ArmorInfo[5];
    public int Armor;

    private InventoryAmmunitionScript _inventoryAmmunitionScript;
    private int _partID;

    private void Start() => FindObjectOfType<InventoryAmmunitionScript>().SetPlayerAmmunitionScript(this, out _inventoryAmmunitionScript);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerWeaponScript.CanAttack = !PlayerWeaponScript.CanAttack;
            _inventoryAmmunitionScript.SwitchMenu(true, "InventoryAmmunition");
        }
    }

    public void AddArmor(ArmorInfo armorInfo)
    {
        switch (armorInfo.ArmorType)
        {
            case ArmorType.Head:
                _partID = 0;
                break;

            case ArmorType.Body:
                _partID = 1;
                break;
        }

        Armor += armorInfo.Armor;

        ArmorMeshes[_partID].mesh = armorInfo.ArmorMesh;
        Armors[_partID] = armorInfo;
    }
}
