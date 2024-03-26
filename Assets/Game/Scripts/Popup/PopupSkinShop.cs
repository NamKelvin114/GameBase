using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSkinShop : BasePopup
{
    [SerializeField] private GameObject item;
    [SerializeField] private List<GameObject> currentItems = new List<GameObject>();
    [SerializeField] private RectTransform boardContent;
    private int number = 10;
    protected override void BeforeShow()
    {
        if (boardContent.transform.childCount == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                var ins = Instantiate(item, boardContent);
                currentItems.Add(ins);
            }
        }
    }
}
