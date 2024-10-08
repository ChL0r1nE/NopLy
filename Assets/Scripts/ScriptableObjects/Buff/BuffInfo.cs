using UnityEngine;

public enum BuffType
{
    Heal,
    Armor
}

[CreateAssetMenu(fileName = "New BuffInfo", menuName = "BuffInfo", order = 1)]
public class BuffInfo : ScriptableObject
{
    public Sprite Sprite;
    public BuffType Type;
    public float Modifier;
    public float Length;
    public string Name;
}
