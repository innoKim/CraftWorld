using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WaterCube : MonoBehaviour {

    public float amplitude;

    bool isTop;
    float offSet;
    Vector3 origin;

    public MapGenerator mapgen;


    // Use this for initialization
    void Start () {
        origin = transform.position;

        if (ObjectManager.Instance.objArr[Mathf.RoundToInt(origin.x), Mathf.RoundToInt(origin.y) + 1, Mathf.RoundToInt(origin.z)]
            == ObjectManager.ObjType.None) isTop = true;
        else isTop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTop) return;
        if (Vector3.SqrMagnitude(origin - ObjectManager.Instance.player.transform.position) > 225.0f) return;

        offSet = Mathf.PerlinNoise((origin.x + Time.time)/5.0f, (origin.z + Time.time) / 5.0f) - 0.5f;
        transform.position = origin + Vector3.up * offSet * amplitude;
    }
}
