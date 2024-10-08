using UnityEngine;

[CreateAssetMenu(fileName = "New PotionInfo", menuName = "PotionInfo", order = 4)]
public class PotionInfo : ItemInfo
{
    [Header("Potion")]
    public BuffInfo Buff;
}
