using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace Map
{
    public class CaravanSpawn : MonoBehaviour
    {
        public void Spawn(Data.Caravan caravan) => Instantiate(_caravanPrefab).SetData(caravan);

        [SerializeField] private Caravan _caravanPrefab;
        private BinaryFormatter _formatter = new();
        private FileStream _file;

        private void Start()
        {
            if (!File.Exists($"{Application.persistentDataPath}/CaravansID.dat")) return;

            _file = File.Open($"{Application.persistentDataPath}/CaravansID.dat", FileMode.Open);
            int[] caravansID = (_formatter.Deserialize(_file) as Data.IDArray).IDs;
            _file.Close();

            foreach (int id in caravansID)
            {
                _file = File.Open($"{Application.persistentDataPath}/Caravan{id}.dat", FileMode.Open);
                Data.Caravan caravan = _formatter.Deserialize(_file) as Data.Caravan;
                _file.Close();

                if (caravan.TargetID != -1)
                    Instantiate(_caravanPrefab).SetData(caravan);
            }
        }
    }
}
