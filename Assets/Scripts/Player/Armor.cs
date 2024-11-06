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

        public ArmorInfo[] Armors;

        [SerializeField] private MeshFilter[] _armorMeshes;

        public float ArmorValue;

        private int _partID;
        private float _armorBuffModifier = 1f;

        private void Start() => FindObjectOfType<InventoryUI.Ammunition>().PlayerArmor = this;

        public void SetArmor(ArmorInfo armorInfo)
        {
            _partID = (int)armorInfo.ArmorType;

            Armors[_partID] = armorInfo;

            if (_partID > 1)
            {
                _partID = 2 + (_partID - 2) * 3;

                for (int i = 0; i < 3; i++)
                    _armorMeshes[_partID + i].mesh = armorInfo.ArmorMeshes[i];

                if(_partID == 8)
                    for (int i = 0; i < 3; i++)
                        _armorMeshes[_partID + i + 3].mesh = armorInfo.ArmorMeshes[i];
            }
            else
                _armorMeshes[_partID].mesh = armorInfo.ArmorMeshes[0];

            CalculateArmorValue();
        }

        private void CalculateArmorValue()
        {
            ArmorValue = ArmorBuffModifier;

            foreach (ArmorInfo info in Armors)
                ArmorValue += info.ArmorModifier;
        }
    }
}
