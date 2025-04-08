using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Button : MonoBehaviour
    {
        public void ButtonDown() => _mercenariesList.CaravanButtonDown(_id);

        [SerializeField] private Text _text;

        private MercenariesList _mercenariesList;
        private int _id;

        public void SetButtonInfo(MercenariesList mercenariesList, int id, int startID, UnitType type) //typeForTest?
        {
            if (type == UnitType.RepairSquad) GetComponent<Image>().color = Color.green; //Test

            _mercenariesList = mercenariesList;
            _text.text = $"#{id}; Start {startID}";
            _id = id;
        }
    }
}
