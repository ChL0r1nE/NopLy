using UnityEngine;

[CreateAssetMenu(fileName = "New SeedInfo", menuName = "SeedInfo", order = 3)]
public class SeedInfo : ItemInfo
{
    [Header("Seed")]
    public Mesh[] PlantMeshes;
    public ItemInfo Harvest;
}
