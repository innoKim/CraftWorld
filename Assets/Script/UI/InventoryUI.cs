using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    GameObject itemHolder;
    List<GameObject> holders;
    
	void Start () {

        itemHolder = Resources.Load<GameObject>("Prefab/UI/ItemHolder");
        itemHolder.GetComponent<Image>().sprite = transform.parent.GetComponent<Image>().sprite;

        holders = new List<GameObject>();

        for (int i=0;i<15;i++)
        {
            GameObject newHolder = Instantiate(itemHolder, this.transform);
            newHolder.name = "ItemHolder" + i.ToString();
            holders.Add(newHolder);
        }
    }
}
