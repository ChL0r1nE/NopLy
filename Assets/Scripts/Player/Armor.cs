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
                _armorBuffModifier = Mathf.Clamp(value, 0f, float.MaxValue);
                UpdateArmor();
            }
        }

        public Info.Armor[] Armors;

        public float ArmorValue { get; private set; }

        [SerializeField] private MeshFilter[] _armorMeshes;
        [SerializeField] private Info.Armor[] _defaultArmors;

        private UI.Ammunition _ammunitionUI;
        private float _armorBuffModifier = 0f;
        private int _partID;

        public void SetDefaultArmor(int id) => SetArmor(_defaultArmors[id]);

        public void Inizilize()
        {
            _ammunitionUI = FindObjectOfType<UI.Ammunition>();
            _ammunitionUI.SetPlayerArmor(this);
            UpdateArmor();
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

            UpdateArmor();
        }

        public void SetArmor(Info.Armor[] armors)
        {
            foreach (Info.Armor armor in armors)
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
            }

            UpdateArmor();
        }

        private void UpdateArmor()
        {
            _ammunitionUI.UpdateMenu(null);
            ArmorValue = 1 + ArmorBuffModifier;

            foreach (Info.Armor armor in Armors)
                ArmorValue += armor.ArmorModifier;
        }
    }
}
