using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {


    public GameObject holdItem;
    public string itemName;
    int count;

    // Use this for initialization
    void Start() {
        ItemHold();
    }

    // Update is called once per frame
    void Update() {

    }

    void ItemHold()
    {
        holdItem = ItemManager.Instance.GetItemFromPool(itemName);


        if (holdItem == null) return;
        holdItem.transform.parent = this.transform;

        RectTransform rectTransform = holdItem.AddComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(0, 0, -200);
        rectTransform.localScale = new Vector3(100, 100, 100);
        rectTransform.localRotation = Quaternion.Euler(new Vector3(-30, 30, 0));        

        Destroy(holdItem.GetComponent<Rigidbody>());
    }
       

    void ItemDehold()
    {

    }
}
