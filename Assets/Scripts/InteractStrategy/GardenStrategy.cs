using UnityEngine;

public class GardenStrategy : Strategy
{
    public Slot GetInfo(int id) => Slots[id];

    public void SetOpen(bool isOpen) => _isOpen = isOpen;

    public Slot[] Slots = new Slot[4];

    [SerializeField] private MeshFilter[] _plantMeshes = new MeshFilter[4];
    private int[] _plantProgress = new int[4];
    private InventoryGardenScript _inventoryGardenScript;
    private int _nowPlants = 0;
    private bool _isOpen = false;

    private void OnEnable() => TickMachineScript.OnTick += OnTick;

    private void OnDisable() => TickMachineScript.OnTick -= OnTick;

    private void Start() => _inventoryGardenScript = FindObjectOfType<InventoryGardenScript>();

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && _isOpen)
        {
            _isOpen = false;
            _inventoryGardenScript.SwitchOpen(false);
        }
    }

    public bool CanAddSeed(Slot slot) => slot.Info.Type == ItemType.Seed && _nowPlants < 4;

    public override void Interact()
    {
        _isOpen = !_isOpen;
        _inventoryGardenScript.SetStrategy(this);
        _inventoryGardenScript.UpdateMenu(Slots, false);
        _inventoryGardenScript.SwitchOpen(true);
    }

    public void AddSeed(Slot slot)
    {
        slot.Count = 1;
        _nowPlants++;
        int plantNumber = 0;

        for (int i = 0; i < 4; i++)
        {
            if (Slots[i].Info == null)
            {
                Slots[i] = slot;
                plantNumber = i;

                break;
            }
        }

        SeedInfo seedInfo = Slots[plantNumber].Info as SeedInfo;

        _plantProgress[plantNumber] = -1;
        _plantMeshes[plantNumber].mesh = seedInfo.PlantMeshes[0];

        _inventoryGardenScript.UpdateMenu(Slots, false);
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
            if (Slots[i].Info)
            {
                SeedInfo info = Slots[i].Info as SeedInfo;

                if (Slots[i].Info.Type != ItemType.Seed) continue;

                if(_plantProgress[i] < 2)
                    _plantMeshes[i].mesh = info.PlantMeshes[Mathf.Clamp(_plantProgress[i], 0, 1)];
                else if (_plantProgress[i] == 2)
                {
                    _plantMeshes[i].mesh = info.PlantMeshes[2];
                    Slots[i] = new Slot(info.Harvest, 1);
                    continue;
                }
            }
            else
                _plantMeshes[i].mesh = null;
        }
    }

    public void DeleteItem(int id)
    {
        _nowPlants--;
        Slots[id] = new Slot(null, 0);

        ShowPlants();
        _inventoryGardenScript.UpdateMenu(Slots, false);
    }
}
