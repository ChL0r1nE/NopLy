using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public static ItemDictionary Instance;

    public Info.Item[] Items;

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
}
