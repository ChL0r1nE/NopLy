using UnityEngine;

namespace Info
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Potion", order = 4)]
    public class Potion : Item
    {
        [Header("Potion")]
        public Buff Buff;
    }
}
