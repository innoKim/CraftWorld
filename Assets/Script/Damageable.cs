using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable: MonoBehaviour, IDamageable {

    public int maxHp;
    public int curHp;
    
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
    
    public void Damaged()
    {
        curHp--;

        //temp[0] : 진동 사이즈, temp[1] : 진동 시간
        float[] temp = new float[2] { 0.05f, 0.2f }; 
        StartCoroutine("Vibrate", temp);

        if(curHp<=0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator Vibrate(float[] vibrateParams)
    {
        float timer = 0.0f;
        Vector3 origin = transform.position;

        while(timer < vibrateParams[1])
        {
            timer += Time.deltaTime;
            transform.position = 
                origin + vibrateParams[0] * new Vector3(Random.RandomRange(-1.0f, 1.0f), Random.RandomRange(-1.0f, 1.0f), Random.RandomRange(-1.0f, 1.0f));

            yield return null;
        }

        transform.position = origin;
    }
}
