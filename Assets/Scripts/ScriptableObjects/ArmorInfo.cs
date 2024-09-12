using UnityEngine;

public enum ArmorType
{
    Head,
    Body,
    LeftHand,
    RightHand,
    Legs
}

[CreateAssetMenu(fileName = "New ArmorInfo", menuName = "ArmorInfo", order = 3)]
public class ArmorInfo : ItemInfo
{
    [Header("Armor")]
    public ArmorType ArmorType;
    public Mesh ArmorMesh;
    public int Armor;
}
