using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuickPanelScript : MonoBehaviour
{
    public Image[] Images = new Image[10];

    public void UpdateQuickPanel(List<ItemInfo> itemsInfo)
    {
        for(int i = 0; i < itemsInfo.Count; i++)
            Images[i].sprite = itemsInfo[i].Sprite;
    }
}
