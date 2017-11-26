using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjectBase, IDamageable {

    public int maxHp;
    public int curHp;

    public Weapon weapon;
    public Armor armor;

    public Transform rightHandTransform;
    public Transform leftHandTransform;

    public Vector3 rotation;

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

    public virtual void Damaged(int damage)
    {
        curHp-= damage;

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

    public void Equip(EquipItem item)
    {
        if(item.equipType == EquipItem.EquipType.Weapon)
        {
            weapon = item as Weapon;
        }
        else if(item.equipType == EquipItem.EquipType.Armor)
        {
            armor = item as Armor;
        }
    }
    
    void Update()
    {
        if (weapon)
        {
            if(weapon.weaponType == Weapon.WeaponType.Sword)
            {
                weapon.transform.position = rightHandTransform.position;
                weapon.transform.rotation = rightHandTransform.rotation * Quaternion.Euler(new Vector3(90, 0, 50));
            }
            else if(weapon.weaponType == Weapon.WeaponType.Bow)
            {
                weapon.transform.position = leftHandTransform.position;
                weapon.transform.rotation = leftHandTransform.rotation * Quaternion.Euler(new Vector3(0, 0, -90));
            }            
        }
    }
}
