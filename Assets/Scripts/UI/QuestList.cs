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
        private int[] _questIDs;

        [SerializeField] private RectTransform _questLabel;
        private BinaryFormatter _formatter = new();
        private FileStream _file;

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/ActiveQuests.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/ActiveQuests.dat", FileMode.Open);
            ActiveQuestIDsRecord activeQuestsRecord = (ActiveQuestIDsRecord)_formatter.Deserialize(_file);
            _file.Close();

            _questIDs = activeQuestsRecord.IDs;
            QuestRecord questRecord;

            foreach (int id in _questIDs)
            {
                _file = File.Open($"{Application.persistentDataPath}/Quest{id}.dat", FileMode.Open);
                questRecord = (QuestRecord)_formatter.Deserialize(_file);
                _file.Close();

                AddQuestLabel(questRecord.Name, questRecord.Task);
            }
        }

        public void AddQuestLabel(string name, string task)
        {
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
