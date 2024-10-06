using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuffListScript : MonoBehaviour //VeryBadWork & NeedReWorkBuffSystem
{
    private List<GameObject> _icons = new();
    private List<Sprite> _sprites = new();

    [SerializeField] private GameObject _iconPrefab;
    private int _buffCount = 0;

    public void AddIcon(Sprite sprite)
    {
        if (_sprites.IndexOf(sprite) != -1) return;

        _sprites.Add(sprite);
        _icons.Add(Instantiate(_iconPrefab, transform.position, Quaternion.identity, transform));

        _icons[_buffCount].GetComponent<Image>().sprite = _sprites[_buffCount];
        _buffCount++;
    }

    public void DeleteIcon(Sprite sprite)
    {
        _buffCount--;

        int number = _sprites.IndexOf(sprite);

        Destroy(_icons[number]);
        _icons.RemoveAt(number);
        _sprites.RemoveAt(number);
    }
}
