using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public static ItemDictionary Instance;

    public Info.Item[] Items;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
