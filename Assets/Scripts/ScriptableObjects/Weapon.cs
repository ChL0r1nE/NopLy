using UnityEngine;

namespace Info
{
    public enum WeaponType
    {
        Default,
        Shaft,
        Chopping
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 2)]
    public class Weapon : Item
    {
        [Header("Weapon")]
        public WeaponType WeaponType;
        public Mesh WeaponMesh;
        public int Damage;
        public int MaxEndurance;
    }
}
