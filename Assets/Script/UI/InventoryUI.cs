using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    GameObject itemHolder;
    List<GameObject> holders;

    public GameObject selectedHolder = null;

	void Start () {

        itemHolder = Resources.Load<GameObject>("Prefab/UI/ItemHolder");
        itemHolder.GetComponent<Image>().sprite = transform.parent.GetComponent<Image>().sprite;

        holders = new List<GameObject>();

        for (int i=0;i<15;i++)
        {
            GameObject newHolder = Instantiate(itemHolder, this.transform);
            newHolder.name = "ItemHolder" + i.ToString();
            newHolder.GetComponent<ItemHolder>().inventoryIndex = i;
            holders.Add(newHolder);
        }
    }

    public void SelectHolder(GameObject holder)
    {
        if (holder.GetComponent<ItemHolder>().holdItem != null)
        {
            if(selectedHolder == null)
            {
                selectedHolder = holder;
                selectedHolder.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            }
            else if(selectedHolder == holder)
            {
                selectedHolder.GetComponent<Image>().color = Color.white;
                selectedHolder = null;
            }
            else
            {
                selectedHolder.GetComponent<Image>().color = Color.white;
                selectedHolder = holder;
                selectedHolder.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            }
        }
        else
        {
            selectedHolder.GetComponent<Image>().color = Color.white;
            selectedHolder = null;            
        }
    }    

    public void EquipItem()
    {
        if(selectedHolder)
        {
            GameObject targetObject = selectedHolder.GetComponent<ItemHolder>().holdItem;
            if (targetObject.CompareTag("EquipableItem"))
            {
                ObjectManager.Instance.player.GetComponent<Player>().Equip(ItemManager.Instance.GetItemFromPool(targetObject.name));
            }            
        }

        Debug.Log("EquipClicked");
    }

    void UseItem()
    {

    }

    void DestroyItem()
    {
    }
}


