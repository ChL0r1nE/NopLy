using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public static ItemDictionary Instance;

    [SerializeField] private Info.Item[] _items;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public Info.Item GetInfo(int id)
    {
        foreach (Info.Item item in _items)
            if (item.ID == id)
                return item;

        return null;
    }
}
