using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{

    ObjectManager.ObjType objType;


    void SetObjType(ObjectManager.ObjType type)
    {
        objType = type;
    }
}
