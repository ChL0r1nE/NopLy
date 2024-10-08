using UnityEngine;

namespace PlayerComponent
{
    public class Armor : MonoBehaviour
    {
        public float ArmorBuffModifier
        {
            get => _armorBuffModifier;
            set
            {
                _armorBuffModifier = Mathf.Clamp(value, 1f, float.MaxValue);
                CalculateArmorValue();
            }
        }

        public MeshFilter[] ArmorMeshes = new MeshFilter[5];
        public ArmorInfo[] Armors = new ArmorInfo[5];

        public float ArmorModifier;

        private int _partID;
        private float _armorBuffModifier = 1f;

        private void Start() => FindObjectOfType<InventoryUI.Ammunition>().PlayerArmor = this;

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
}
