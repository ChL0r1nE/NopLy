using UnityEngine;

public enum ArmorType : int
{
    Head = 0,
    Body,
    RightHand,
    LeftHand,
    Legs
}

[CreateAssetMenu(fileName = "New ArmorInfo", menuName = "ArmorInfo", order = 3)]
public class ArmorInfo : ItemInfo
{
    [Header("Armor")]
    public Mesh[] ArmorMeshes;

    public ArmorType ArmorType;
    public float ArmorModifier;
}
