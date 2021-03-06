﻿using System.Collections;
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

    protected HealthBar hpBar = null;
    public HealthBar HpBar
    {
        get
        {
            if (hpBar) return hpBar;
            else
            {
                Debug.Log("new Hpbar");
                GameObject temp = Instantiate(Resources.Load("Prefab/UI/HPBar")) as GameObject;
                temp.transform.parent = this.transform;
                temp.transform.localPosition = new Vector3(0, 0.5f, 0);
                hpBar = temp.GetComponent<HealthBar>();
                return hpBar;
            }
        }
    }


    public virtual void Damaged(int damage)
    {
        curHp -= damage;

        HpBar.SetProgressBar(maxHp, curHp);
        //temp[0] : 진동 사이즈, temp[1] : 진동 시간
        float[] temp = new float[2] { 0.05f, 0.2f };
        StartCoroutine("Vibrate", temp);

        if (curHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected float SqrDistanceFromTraget()
    {
        return (targetTransform.position - transform.position).sqrMagnitude;
    }


    public virtual void Destroyed()
    {
        Destroy(this.gameObject);
    }


    public virtual IEnumerator Vibrate(float[] vibrateParams)
    {
        float timer = 0.0f;
        Vector3 origin = transform.position;

        while (timer < vibrateParams[1])
        {
            timer += Time.deltaTime;
            transform.position =
                origin + vibrateParams[0] * new Vector3(Random.RandomRange(-1.0f, 1.0f), Random.RandomRange(-1.0f, 1.0f), Random.RandomRange(-1.0f, 1.0f));

            yield return null;
        }

        transform.position = origin;
    }
}
