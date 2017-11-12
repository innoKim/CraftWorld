using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform target;
    public Vector3 lerpPosition;

    [Range(5.0f, 20.0f)]
    public float MaxCameraDistance;

    [Range(1.0f, 5.0f)]
    public float MinCameraDistance;

    [Range(0.0f, 5.0f)]
    public float CameraZoomSpd;

    [Range(0.0f, 2.0f)]
    public float CameraRotationSpd;

    [Range(0.0f, 100.0f)]
    public float CameraTrackSpd;

    float distanceFromTarget;
    Vector3 direction;

    //for MouseDrag
    Vector3 startPt;
    Vector3 deltaPt;

    void Start()
    {
        distanceFromTarget = transform.localPosition.magnitude;
        direction = transform.rotation.eulerAngles;
    }

    void Update()
    {
        ScreenRotation();
        Zoom();

        lerpPosition = Vector3.Lerp(lerpPosition, target.position, CameraTrackSpd*0.001f);

        transform.position = lerpPosition + Quaternion.Euler(direction.x,direction.y,direction.z)*Vector3.forward * distanceFromTarget;
        transform.LookAt(lerpPosition);
    }

    void ScreenRotation()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            startPt = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.Mouse2))
        {
            deltaPt = Input.mousePosition - startPt;
            direction = direction + new Vector3(deltaPt.y, deltaPt.x, 0)* CameraRotationSpd;
            startPt = Input.mousePosition;
        }
    }
    void Zoom()
    {
        distanceFromTarget -=Input.GetAxisRaw("Mouse ScrollWheel") * CameraZoomSpd;

        if (distanceFromTarget < MinCameraDistance) distanceFromTarget = MinCameraDistance;
        if (distanceFromTarget > MaxCameraDistance) distanceFromTarget = MaxCameraDistance;
    }
}
