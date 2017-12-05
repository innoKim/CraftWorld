using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy {


    void Start()
    {
        pathFinder = GetComponent<AstarTracer>();
        targetTransform = ObjectManager.Instance.player.transform;
    }

    private void Update()
    {
        if(targetTransform == null) targetTransform = ObjectManager.Instance.player.transform;

        if (SqrDistanceFromTraget()<attackSqrDist)
        {
            Attack();
        }
        else if(SqrDistanceFromTraget()<senseSqrDist)
        {
            GetToTarget();
        }
        else
        {
            Wandering();
        }
    }
    
    void Wandering()
    {

    }

    void GetToTarget()
    {

    }

    void Attack()
    {

    }
}
