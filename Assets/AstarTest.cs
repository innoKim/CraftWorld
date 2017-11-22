using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTest : MonoBehaviour {

    public Transform targetTransform;
    List<Node> path;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            path = Astar.Instance.pathFind(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), Mathf.RoundToInt(targetTransform.position.x), Mathf.RoundToInt(targetTransform.position.z));
        }
    } 
}
