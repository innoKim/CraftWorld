using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    
    public Transform targetTransform;

    private Vector3 offSet;
    private Vector3 curPosition;
    private Quaternion curRot;
    private float rotSpd;
    private float rot = 0.0f;
    // Use this for initialization
    void Start()
    {
        curPosition = targetTransform.position;
        offSet = this.transform.position;
        curRot = transform.rotation;
        rotSpd = targetTransform.gameObject.GetComponent<PlayerController>().RotSpd;
    }

    // Update is called once per frame
    void Update()
    {
        rot += Input.GetAxisRaw("Horizontal") * rotSpd * Time.deltaTime;

        if (targetTransform)
        {
            transform.position = targetTransform.position + transform.rotation* offSet;
            transform.rotation = Quaternion.Euler(new Vector3(0,rot,0)) * curRot;
            curPosition = targetTransform.position * 0.05f + curPosition * 0.95f;
        }
    }
}
