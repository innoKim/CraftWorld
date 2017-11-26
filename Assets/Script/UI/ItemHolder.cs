using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {

    public int inventoryIndex;
    public GameObject holdItem;
    
    void Update()
    {
        if(ItemManager.Instance.inventory.Count-1<inventoryIndex)
        {

        }
        else
        {
            if (ItemManager.Instance.inventory[inventoryIndex] == "" && holdItem != null)
            {
                ItemDehold();
            }
            else
            {
                if (holdItem == null)
                {
                    ItemHold(ItemManager.Instance.inventory[inventoryIndex]);
                }
                else if (holdItem.name != ItemManager.Instance.inventory[inventoryIndex])
                {
                    ItemDehold();
                    ItemHold(ItemManager.Instance.inventory[inventoryIndex]);
                }
            }
        }       
    }

    public void ItemHold(string itemName)
    {
        holdItem = ItemManager.Instance.GetItemFromPool(itemName);

        if (holdItem == null) return;
        holdItem.transform.parent = this.transform;

        RectTransform rectTransform = holdItem.AddComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(0, 0, -100);
        if (itemName == "Log") rectTransform.localScale = new Vector3(50, 50, 50);
        else rectTransform.localScale = new Vector3(20, 20, 20);
        rectTransform.localRotation = Quaternion.Euler(new Vector3(-30, 30, 0));

        Destroy(holdItem.GetComponent<Rigidbody>());
    }
       

    void ItemDehold()
    {        
        Destroy(holdItem);
        holdItem = null;
    }
}
