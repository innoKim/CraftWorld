using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    Image front;
    Transform camera;

    void Start()
    {
    }

    void Update()
    {
        transform.LookAt(camera.position);
    }

    public void SetProgressBar(int max, int cur)
    {
        if(front == null)
        {
            front = transform.GetChild(1).GetComponent<Image>();
            camera = Camera.main.transform;
            transform.LookAt(camera.position);
        }
        front.fillAmount = (float)cur / (float)max;
    }
}
