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
        inventory = new List<string>();

        AddItemToInventory("Bow");
        AddItemToInventory("Sword");
    }
    
    void AddPrefabToItemPool(string path)
    {
        GameObject[] objs = Resources.LoadAll<GameObject>(path);

        itemPool = new Dictionary<string, GameObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            itemPool.Add(objs[i].name, objs[i]);
        }
    }

    public List<string> inventory = null;
    public Dictionary<string, GameObject> itemPool = null;

    public void AddItemToInventory(string itemName)
    {
        if (inventory.Count < 15)
            inventory.Add(itemName);
        else Debug.Log("Inventory is full");
        return;
    }

    public GameObject GetItemFromInven(string itemName)
    {
        if(inventory.Remove(itemName))
        {
            GameObject newObj = Instantiate(itemPool[itemName]);
            newObj.name = itemName;
            return newObj;
        }
        else
        {
            Debug.Log("there is no item has such name");
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
            return null;
        }
    }

    void OnGUI()
    {
        string info = "";
        
        if(inventory != null)
        {            
            foreach (string itemName in inventory)
            {
                info += itemName + "\n";
            }            
        }

        GUI.Box(new Rect(10, 10, 100, 300), info);
    }
}