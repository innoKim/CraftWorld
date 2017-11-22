using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCube : ObjectBase, IDamageable, IDropable {


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

    public virtual void Damaged()
    {
        curHp--;

        //temp[0] : 진동 사이즈, temp[1] : 진동 시간
        float[] temp = new float[2] { 0.05f, 0.2f };
        StartCoroutine("Vibrate", temp);

        if (curHp <= 0)
        {
            Destroy(this.gameObject);
            Drop(dropProbability);
        }
    }

    public virtual void Destroyed()
    {
        Destroy(this.gameObject);
        Vector3 pos = transform.position;
        ObjectManager.Instance.objArr[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)] = ObjectManager.ObjType.None;
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

    public string[] dropItems;
    public float dropProbability;

    public string[] DropItems
    {
        get { return dropItems; }
        set { dropItems = value; }
    }

    public float DropProbability
    {
        get { return dropProbability; }
        set { dropProbability = value; }
    }
    
    public virtual void Drop(float probability)
    {
        float ranNum = Random.Range(0, 0f);
        if (ranNum < probability)
        {
            int itemNumber = Random.Range(0, dropItems.Length);

            GameObject newObject = ItemManager.Instance.GetItemFromPool(dropItems[itemNumber]);
            newObject.transform.position = transform.position;
            newObject.transform.rotation = Quaternion.identity;
        }
    }
}
