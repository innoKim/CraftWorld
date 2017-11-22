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

        for(int i=0;i<8;i++)
        {
            string itemName;
            switch(Random.Range(0, 3))
            {
                case 0:
                    itemName = "Log";
                    break;

                case 1:
                    itemName = "Stone";
                    break;

                case 2:
                    itemName = "Soil";
                    break;

                default:
                    itemName = "";
                    break;
            }
            holders[i].GetComponent<ItemHolder>().ItemHold(itemName);
        }
    }
}
