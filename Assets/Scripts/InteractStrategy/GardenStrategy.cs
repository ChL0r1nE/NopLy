using UnityEngine;

public class GardenStrategy : Strategy
{
    public Slot GetSlot(int id) => Slots[id];

    public void SetOpen(bool isOpen) => _isOpen = isOpen;

    public Slot[] Slots = new Slot[4];

    [SerializeField] private MeshFilter[] _plantMeshes = new MeshFilter[4];
    private int[] _plantProgress = new int[4];

    private InventoryGardenScript _inventoryGardenScript;
    private int _nowPlants = 0;
    private int _plantNumber;
    private bool _isOpen = false;

    private void OnEnable() => TickMachineScript.OnTick += OnTick;

    private void OnDisable() => TickMachineScript.OnTick -= OnTick;

    private void Start() => _inventoryGardenScript = FindObjectOfType<InventoryGardenScript>();

    private void OnTriggerExit()
    {
        if (_isOpen)
            _inventoryGardenScript?.SwitchOpen(false);
    }

    public override void Interact()
    {
        _inventoryGardenScript.SetStrategy(this);
        _inventoryGardenScript.SwitchOpen(true);

        if (_isOpen)
            _inventoryGardenScript.UpdateMenu(Slots);
    }

    public void AddSeed(Slot slot, out int countRemain)
    {
        countRemain = slot.Count;

        if (slot.Info.Type != ItemType.Seed) return;

        _nowPlants++;

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

        _inventoryGardenScript.UpdateMenu(Slots);
    }

    private void OnTick()
    {
        for (int i = 0; i < 4; i++)
            _plantProgress[i]++;

        ShowPlants();
    }

    private void ShowPlants()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!Slots[i].Info)
            {
                _plantMeshes[i].mesh = null;
                continue;
            }

            if (_plantProgress[i] > 2) continue;

            SeedInfo seedInfo = Slots[i].Info as SeedInfo;
            _plantMeshes[i].mesh = seedInfo.PlantMeshes[_plantProgress[i]];

            if (_plantProgress[i] == 2)
            {
                _plantProgress[i]++;
                Slots[i].Info = seedInfo.Harvest;
                Slots[i].Count = Random.Range(2, 5);

                _inventoryGardenScript.UpdateMenu(Slots);
            }
        }
    }

    public void DeleteItem(int id)
    {
        _nowPlants--;
        Slots[id] = new(null, 0);

        ShowPlants();
        _inventoryGardenScript.UpdateMenu(Slots);
    }
}
