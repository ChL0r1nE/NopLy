using UnityEngine;

namespace Interact
{
    public class Garden : AbstractInteract
    {
        public Slot[] Slots;

        [SerializeField] private MeshFilter[] _plantMeshes = new MeshFilter[4];
        private int[] _plantProgress = new int[4];

        public bool IsOpen = false;

        private InventoryUI.Garden _inventoryGarden;
        private int _plantNumber;

        private void OnEnable() => TickMachine.OnTick += OnTick;

        private void OnDisable() => TickMachine.OnTick -= OnTick;

        private void Start() => _inventoryGarden = FindObjectOfType<InventoryUI.Garden>();

        private void OnTriggerExit()
        {
            if (IsOpen)
                _inventoryGarden.SwitchOpen(false);
        }

        public override void Interact()
        {
            _inventoryGarden.GardenStrategy = this;
            _inventoryGarden.SwitchOpen(true);

            if (IsOpen)
                _inventoryGarden.UpdateMenu(Slots);
        }

        public void AddSeed(Slot slot, out int countRemain)
        {
            countRemain = slot.Count;

            if (slot.Info.Type != ItemType.Seed) return;

            for (int i = 0; i < 4; i++)
            {
                if (Slots[i].Info) continue;

                Slots[i] = slot;
                Slots[i].Count = 1;
                _plantNumber = i;
                countRemain--;

                break;
            }

            SeedInfo seedInfo = Slots[_plantNumber].Info as SeedInfo;

            _plantProgress[_plantNumber] = 0;
            _plantMeshes[_plantNumber].mesh = seedInfo.PlantMeshes[0];

            _inventoryGarden.UpdateMenu(Slots);
        }

        private void OnTick()
        {
            for (int i = 0; i < 4; i++)
                _plantProgress[i]++;

            for (int i = 0; i < 4; i++)
            {
                if (!Slots[i].Info || _plantProgress[i] > 2) continue;

                SeedInfo seedInfo = Slots[i].Info as SeedInfo;
                _plantMeshes[i].mesh = seedInfo.PlantMeshes[_plantProgress[i]];

                if (_plantProgress[i] != 2) continue;

                _plantProgress[i] = 3;
                Slots[i].Info = seedInfo.Harvest;
                Slots[i].Count = Random.Range(2, 5);

                _inventoryGarden.UpdateMenu(Slots);
            }
        }

        public void SetSlotCount(int id, int count)
        {
            Slots[id].Count = count;
            _inventoryGarden.UpdateMenu(Slots);

            if (count == 0)
                _plantMeshes[id].mesh = null;
        }
    }
}