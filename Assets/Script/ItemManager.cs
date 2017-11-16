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
        }
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
    }

    public GameObject GetItemFromInven(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]--;
            if (inventory[itemName] <= 0) inventory.Remove(itemName);
            
            GameObject newObj = Instantiate(itemPool[itemName]);
            newObj.name = itemName;
            return newObj;
        }
        else
        {
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
            foreach (KeyValuePair<string, int> pair in inventory)
            {
                info += pair.Key + " : " + pair.Value.ToString() + "\n";
            }            
        }

        GUI.Box(new Rect(10, 10, 100, 300), info);
    }
}
