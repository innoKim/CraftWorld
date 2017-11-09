using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public enum ObjType
    {
        Soil,
        Sand,
        Gravel,
        Stone,
        Metal,
        Water,
        Soil2,
        Tree,
        Rock,
        None,
    }

    public enum TreeType
    {
        Bush,
        Tree1,
        Tree2,
        Tree3,
        None
    }

    public enum RockType
    {
        Rock,
        None
    }

    public ObjType[,,] objArr = null;
    public GameObject player = null;

    static ObjectManager instance = null;
    
    public static ObjectManager Instance
    {
        get
        {
            if(instance)
            {
                return instance;
            }
            else
            {
                instance = new GameObject("_ObjectManager").AddComponent<ObjectManager>();
                return instance;
            }
        }        
    }

    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(this.gameObject);
	}

    public void InitObjArr(int x, int y, int z)
    {
        objArr = new ObjType[x, y, z];
    }
}
