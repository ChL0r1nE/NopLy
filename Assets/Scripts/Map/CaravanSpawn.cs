using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Map
{
    public class CaravanSpawn : MonoBehaviour
    {
        [SerializeField] private Caravan _caravanPrefab;
        private BinaryFormatter _formatter = new();
        private FileStream _file;

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/CaravansID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);

            foreach (int id in (_formatter.Deserialize(_file) as Data.IDArray).IDs)
                Instantiate(_caravanPrefab).SetData(id);

            _file.Close();
        }
    }
}
