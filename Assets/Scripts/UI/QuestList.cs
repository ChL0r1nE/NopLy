using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class QuestList : MonoBehaviour
    {
        private List<RectTransform> _labelRects = new();
        private int[] _questIDs = new int[0];

        [SerializeField] private RectTransform _questLabel;
        private BinaryFormatter _formatter = new();
        private FileStream _file;

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/QuestsID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/QuestsID.dat", FileMode.Open);
            Data.IDArray activeQuestsRecord = (Data.IDArray)_formatter.Deserialize(_file);
            _file.Close();

            _questIDs = activeQuestsRecord.IDs;
            Data.Quest questRecord;

            foreach (int id in _questIDs)
            {
                _file = File.Open($"{Application.persistentDataPath}/Quest{id}.dat", FileMode.Open);
                questRecord = (Data.Quest)_formatter.Deserialize(_file);
                _file.Close();

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
