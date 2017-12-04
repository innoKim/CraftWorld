using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ObjectBase, IDamageable
{
    protected Transform targetTransform;
    protected AstarTracer pathFinder;

    public int maxHp;
    public int curHp;

    protected const float senseSqrDist = 25.0f;
    protected const float attackSqrDist = 0.25f;

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public int CurHp
    {
        get { return curHp; }
        set { curHp = value; }
    }

    protected float SqrDistanceFromTraget()
    {
        return (targetTransform.position - transform.position).sqrMagnitude;
    }

    public virtual void Damaged(int damage)
    {
        curHp -= damage;

        //temp[0] : 진동 사이즈, temp[1] : 진동 시간
        float[] temp = new float[2] { 0.05f, 0.2f };
        StartCoroutine("Vibrate", temp);

        if (curHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Destroyed()
    {
        Destroy(this.gameObject);
    }

}
