using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {

    public GameObject holdItem;
    int count = 0;

    private void Start()
    {
    }

    public void ItemHold(string itemName)
    {
        if (holdItem == null)
        {
            holdItem = ItemManager.Instance.GetItemFromPool(itemName);

            if (holdItem == null) return;
            holdItem.transform.parent = this.transform;

            RectTransform rectTransform = holdItem.AddComponent<RectTransform>();

            rectTransform.localPosition = new Vector3(0, 0, -100);
            if(itemName == "Log") rectTransform.localScale = new Vector3(50, 50, 50);
            else rectTransform.localScale = new Vector3(20, 20, 20);
            rectTransform.localRotation = Quaternion.Euler(new Vector3(-30, 30, 0));

            Destroy(holdItem.GetComponent<Rigidbody>());
        }
        else if (holdItem.name == itemName)
        {
            count++;
        }        
    }
       

    void ItemDehold()
    {
        if (count > 1) count--;
        else
        {
            count = 0;
            Destroy(holdItem);
            holdItem = null;
        }
    }
}
