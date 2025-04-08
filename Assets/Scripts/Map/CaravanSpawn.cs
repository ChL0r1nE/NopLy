using UnityEngine;

namespace Map
{
    public class CaravanSpawn : MonoBehaviour
    {
        public void SpawnCaravan(Data.Caravan caravan) => Instantiate(_caravanPrefab).SetData(caravan);

        public void SpawnRepairSquad(Data.RepairSquad repairSquad) => Instantiate(_repairSquadPrefab).SetData(repairSquad);

        [SerializeField] private Caravan _caravanPrefab;
        [SerializeField] private RepairSquad _repairSquadPrefab;

        private readonly Serialize _serialize = new();

        private void Start()
        {
            if (!_serialize.ExistSave("CaravansID")) return;

            int[] caravansID = _serialize.LoadSave<Data.IDArray>("CaravansID").IDs;
            Data.Caravan caravan;

            foreach (int id in caravansID)
            {
                caravan = _serialize.LoadSave<Data.Caravan>($"Caravan{id}");

                if (caravan.TargetID != -1)
                    Instantiate(_caravanPrefab).SetData(caravan);
            }
        }
    }
}
