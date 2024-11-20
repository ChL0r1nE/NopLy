using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class QuestList : MonoBehaviour
    {
        private List<RectTransform> _labelRects = new();
        private int[] _questIDs = new int[0];

        [SerializeField] private RectTransform _questLabel;

        private readonly Serialize _serialize = new();

        private void Start()
        {
            if (!_serialize.ExistSave("QuestsID")) return;

            Data.IDArray activeQuestsRecord = _serialize.LoadSave<Data.IDArray>("QuestsID");
            _questIDs = activeQuestsRecord.IDs;

            Data.Quest questRecord;

            foreach (int id in _questIDs)
            {
                questRecord = _serialize.LoadSave<Data.Quest>($"Quest{id}");
                AddQuestLabel(questRecord.Name, questRecord.Task, questRecord.ID);
            }
        }

        public void AddQuestLabel(string name, string task, int id)
        {
            int[] questIDs = new int[_questIDs.Length + 1];

            for (int i = 0; i < _questIDs.Length; i++)
                questIDs[i] = _questIDs[i];

            questIDs[^1] = id;
            _questIDs = questIDs;

            _labelRects.Add(Instantiate(_questLabel, transform));
            _labelRects[^1].GetChild(0).GetComponent<Text>().text = name;
            _labelRects[^1].GetChild(1).GetComponent<Text>().text = task;
        }

        public void RemoveQuestLabel(int id)
        {
            for (int i = 0; i < _questIDs.Length; i++)
            {
                if (id != _questIDs[i]) continue;

                Destroy(_labelRects[i].gameObject);
                _labelRects.RemoveAt(i);
                return;
            }
        }
    }
}
