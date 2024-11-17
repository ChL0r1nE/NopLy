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

    public LocationClass[] Locations;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
