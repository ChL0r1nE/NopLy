using UnityEngine;

public enum WeaponPlace
{
    Main,
    Sub
}

public enum WeaponType
{
    Default,
    Shaft,
    Chopping
}

[CreateAssetMenu(fileName = "New WeaponInfo", menuName = "WeaponInfo", order = 2)]
public class WeaponInfo : ItemInfo
{
    [Header("Weapon")]
    public WeaponPlace WeaponPlace;
    public WeaponType WeaponType;
    public Mesh WeaponMesh;
    public int Damage;
}
