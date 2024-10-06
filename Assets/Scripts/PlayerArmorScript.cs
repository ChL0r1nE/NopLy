using UnityEngine;

public class PlayerArmorScript : MonoBehaviour //NeedWorkWithBuffLust, not ArmorBuffZone
{
    public float ArmorBuffModifier
    {
        get => _armorBuffModifier;
        set
        {
            _armorBuffModifier = Mathf.Clamp(value, 1, int.MaxValue);
            CalculateArmorValue();
        }
    }

    public MeshFilter[] ArmorMeshes = new MeshFilter[5];
    public ArmorInfo[] Armors = new ArmorInfo[5];

    public float ArmorModifier;

    private int _partID;
    private float _armorBuffModifier = 1;

    private void Start() => FindObjectOfType<InventoryAmmunitionScript>().SetPlayerAmmunitionScript(this);

    public void SetArmor(ArmorInfo armorInfo)
    {
        _partID = (int)armorInfo.ArmorType;
        Armors[_partID] = armorInfo;

        ArmorMeshes[_partID].mesh = armorInfo.ArmorMesh;

        if (_partID == 4)
            ArmorMeshes[5].mesh = armorInfo.ArmorMesh;

        CalculateArmorValue();
    }

    private void CalculateArmorValue()
    {
        ArmorModifier = ArmorBuffModifier;
        foreach (ArmorInfo info in Armors)
            ArmorModifier += info.ArmorModifier;
    }
}
