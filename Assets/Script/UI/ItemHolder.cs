using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {

    GameObject texture;
    RawImage rawImage;
    string itemName;
    int count;

	// Use this for initialization
	void Start () {
        texture = new GameObject("texture");
        texture.AddComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
