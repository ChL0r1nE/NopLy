using UnityEngine;

namespace Info
{
    public enum BuffType
    {
        Heal,
        Armor
    }

    [CreateAssetMenu(fileName = "New Buff", menuName = "Buff", order = 5)]
    public class Buff : ScriptableObject
    {
        public Sprite Sprite;
        public BuffType Type;
        public float Modifier;
        public float Length;
        public string Name;
    }
}
