using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    
    public Transform targetTransform;

    private Vector3 offSet;
    private Vector3 curPosition;
    private Quaternion curRot;
    // Use this for initialization
    void Start()
    {
        curPosition = targetTransform.position;
        offSet = this.transform.position;
        curRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform)
        {
            transform.position = curPosition + targetTransform.rotation* offSet;
            transform.rotation = targetTransform.rotation*curRot;

            curPosition = targetTransform.position * 0.05f + curPosition * 0.95f;
        }
    }
}
