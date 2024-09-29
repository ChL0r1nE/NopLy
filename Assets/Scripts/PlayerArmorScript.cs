using UnityEngine;

public class PlayerArmorScript : MonoBehaviour
{
    public MeshFilter[] ArmorMeshes = new MeshFilter[5];
    public ArmorInfo[] Armors = new ArmorInfo[5];
    public int ArmorValue;

    private int _partID;

    private void Start() => FindObjectOfType<InventoryAmmunitionScript>().SetPlayerAmmunitionScript(this);

    public void SetArmor(ArmorInfo armorInfo)
    {
        _partID = (int)armorInfo.ArmorType;
        Armors[_partID] = armorInfo;

        ArmorMeshes[_partID].mesh = armorInfo.ArmorMesh;

        if (_partID == 4)
            ArmorMeshes[5].mesh = armorInfo.ArmorMesh;

        ArmorValue = 0;
        foreach (ArmorInfo info in Armors)
            ArmorValue += info.Armor;
    }
}
