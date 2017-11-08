using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCube : MonoBehaviour {

    public float amplitude;

    float offSet;
    Vector3 origin;
	// Use this for initialization
	void Start () {
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        offSet = Mathf.PerlinNoise((origin.x+Time.time), (origin.z + Time.time))-0.5f;
        transform.position = origin + Vector3.up * offSet * amplitude;
    }
}
