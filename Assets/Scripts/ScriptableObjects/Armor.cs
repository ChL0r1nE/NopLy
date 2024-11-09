using UnityEngine;

namespace Info
{
    public enum ArmorType : int
    {
        Head = 0,
        Body,
        RightHand,
        LeftHand,
        Legs
    }

    [CreateAssetMenu(fileName = "New Armor", menuName = "Armor", order = 3)]
    public class Armor : Item
    {
        [Header("Armor")]
        public Mesh[] ArmorMeshes;

        public ArmorType ArmorType;
        public float ArmorModifier;
    }
}