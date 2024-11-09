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

        public Info.Armor[] Armors;

        [SerializeField] private MeshFilter[] _armorMeshes;

        public float ArmorValue { get; private set; }

        private float _armorBuffModifier = 1f;
        private int _partID;

        private void Start()
        {
            FindObjectOfType<UI.Ammunition>().PlayerArmor = this;
            CalculateArmorValue();
        }

        public void SetArmor(Info.Armor armor)
        {
            _partID = (int)armor.ArmorType;

            Armors[_partID] = armor;

            if (_partID > 1)
            {
                _partID = 2 + (_partID - 2) * 3;

                for (int i = 0; i < 3; i++)
                    _armorMeshes[_partID + i].mesh = armor.ArmorMeshes[i];

                if (_partID == 8)
                    for (int i = 0; i < 3; i++)
                        _armorMeshes[_partID + i + 3].mesh = armor.ArmorMeshes[i];
            }
            else
                _armorMeshes[_partID].mesh = armor.ArmorMeshes[0];

            CalculateArmorValue();
        }

        private void CalculateArmorValue()
        {
            ArmorValue = 1 + ArmorBuffModifier;

            foreach (Info.Armor armor in Armors)
                ArmorValue += armor.ArmorModifier;
        }
    }
}
