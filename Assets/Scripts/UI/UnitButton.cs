using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class UnitButton : MonoBehaviour
    {
        public void ButtonDown() => MercenariesList.Static.UnitButtonDown(_id);

        [SerializeField] private Text _text;

        private int _id;

        public void SetButtonInfo(int id, int startID, UnitType type)
        {
            if (type == UnitType.RepairSquad)
                GetComponent<Image>().color = Color.green;

            _text.text = $"#{id}; Start {startID}";
            _id = id;
        }
    }
}
