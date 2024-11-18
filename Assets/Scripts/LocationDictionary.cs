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

    [SerializeField] private LocationClass[] _locations;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Transform GetTransform(int id)
    {
        foreach (LocationClass location in _locations)
            if (location.ID == id)
                return location.Transform;

        return null;
    }
}
