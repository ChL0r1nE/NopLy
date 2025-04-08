using System.Collections.Generic;
using UnityEngine;

public class LocationDictionary : MonoBehaviour
{
    public static LocationDictionary Instance;

    [System.Serializable]
    public class LocationClass
    {
        public int ID;
        public Transform Transform;
    }

    private Serialize _serialize = new();

    [SerializeField] private LocationClass[] _locations;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_serialize.ExistSave("LocationsID")) return;

        List<int> IDs = new();

        foreach (LocationClass location in _locations)
            IDs.Add(location.ID);

        bool[] alive = { true, true, true };
        _serialize.CreateSave("Location4500", new Data.Enemy(alive));
        _serialize.CreateSave("Location4501", new Data.Production(0, 0, true));
        _serialize.CreateSave("Location4502", new Data.Slots(null));
        _serialize.CreateSave("Location4503", new Data.Enemy(alive));

        _serialize.CreateSave("LocationsID", new Data.IDArray { IDs = IDs.ToArray() });
    }

    public Transform GetTransform(int id)
    {
        foreach (LocationClass location in _locations)
            if (location.ID == id)
                return location.Transform;

        return null;
    }
}
