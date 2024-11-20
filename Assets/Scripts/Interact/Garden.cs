using UnityEngine;

namespace Interact
{
    public class Garden : AbstractInteract
    {
        public void UpdateMenu() => _inventoryGarden.UpdateMenu(Slots);

        public void SetOpen(bool open) => _isOpen = open;

        public Slot[] Slots;

        [SerializeField] private MeshFilter[] _plantMeshes = new MeshFilter[4];
        private int[] _plantProgress = new int[4];

        [SerializeField] private string _saveID;
        private UI.Garden _inventoryGarden;
        private int _plantNumber;
        private bool _isOpen;

        private readonly Serialize _serialize = new();

        private void OnEnable() => TickMachine.OnTick += OnTick;

        private void OnDisable()
        {
            TickMachine.OnTick -= OnTick;
            _serialize.CreateSave($"Garden{_saveID}", new Data.Slots(Slots));
        }

        private void Start()
        {
            _inventoryGarden = FindObjectOfType<UI.Garden>();

            if (_serialize.ExistSave($"Garden{_saveID}"))
                _serialize.Records2Slots(_serialize.LoadSave<Data.Slots>($"Garden{_saveID}").ItemRecords, Slots);

            for (int i = 0; i < 4; i++)
                _plantMeshes[i].mesh = Slots[i].Item ? (Slots[_plantNumber].Item as Info.Seed).PlantMeshes[0] : null;
        }

        private void OnTriggerExit()
        {
            if (_isOpen)
                _inventoryGarden.SwitchOpen(false);
        }

        public override void Interact()
        {
            _inventoryGarden.GardenStrategy = this;
            _inventoryGarden.SwitchOpen(true);

            if (_isOpen)
                _inventoryGarden.UpdateMenu(Slots);
        }

        public void UpdateNullMeshes()
        {
            for (int i = 0; i < 4; i++)
                if (!Slots[i].Item)
                    _plantMeshes[i].mesh = null;
        }

        public void AddSeed(ref Slot slot)
        {
            if (slot.Item.Type != Info.ItemType.Seed) return;

            for (int i = 0; i < 4; i++)
            {
                if (Slots[i].Item) continue;

                Slots[i] = new(slot.Item, 1);
                _plantProgress[i] = 0;
                _plantMeshes[i].mesh = (Slots[i].Item as Info.Seed).PlantMeshes[0];
                slot.Count--;

                break;
            }

            _inventoryGarden.UpdateMenu(Slots);
        }

        private void OnTick()
        {
            for (int i = 0; i < 4; i++)
                _plantProgress[i]++;

            for (int i = 0; i < 4; i++)
            {
                if (!Slots[i].Item || _plantProgress[i] > 2) continue;

                Info.Seed seed = Slots[i].Item as Info.Seed;
                _plantMeshes[i].mesh = seed.PlantMeshes[_plantProgress[i]];

                if (_plantProgress[i] != 2) continue;

                _plantProgress[i] = 3;
                Slots[i].Item = seed.Harvest;
                Slots[i].Count = Random.Range(2, 5);

                if (_isOpen)
                    _inventoryGarden.UpdateMenu(Slots);
            }
        }
    }
}