using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    static private ItemManager instance = null;

    static public ItemManager Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }
            else
            {
                GameObject newInstance = new GameObject("_ItemManager");
                instance = newInstance.AddComponent<ItemManager>();
                return instance;
            }
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        AddPrefabToItemPool("Prefab/Item");
    }
    
    void AddPrefabToItemPool(string path)
    {
        GameObject[] objs = Resources.LoadAll<GameObject>(path);

        itemPool = new Dictionary<string, GameObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            itemPool.Add(objs[i].name, objs[i]);
            Debug.Log(objs[i].name + " is Loaded");
        }
        Debug.Log("God na");
    }

    private Dictionary<string, int> inventory = null;
    public Dictionary<string ,GameObject> itemPool = null;
    
    public void AddItemToInventory(string itemName)
    {
        if (inventory == null) inventory = new Dictionary<string, int>();

        if(inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory.Add(itemName, 1);
        }
        Debug.Log(itemName + "is Added to itemPool"); 
    }

    public GameObject GetItemFromInven(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]--;
            if (inventory[itemName] <= 0) inventory.Remove(itemName);
            
            Debug.Log(itemName + "is Drawed from Inventory");

            GameObject newObj = Instantiate(itemPool[itemName]);
            newObj.name = itemName;
            return newObj;
        }
        else
        {
            Debug.Log("there is no such item in Inventory");

            return null;
        }        
    }

    public GameObject GetItemFromPool(string itemName)
    {
        if (itemPool.ContainsKey(itemName))
        {
            GameObject newObj = Instantiate(itemPool[itemName]);
            newObj.name = itemName;
            return newObj;
        }
        else
        {
            Debug.Log("there is no such item in Pool");

            return null;
        }
    }
}
