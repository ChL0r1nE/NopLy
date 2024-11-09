using UnityEngine;

namespace Info
{
    [CreateAssetMenu(fileName = "New Seed", menuName = "Seed", order = 6)]
    public class Seed : Item
    {
        [Header("Seed")]
        public Mesh[] PlantMeshes;
        public Item Harvest;
    }
}
