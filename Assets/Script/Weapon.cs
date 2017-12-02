using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquipItem
{
    public enum WeaponType
    {
        None,
        Sword,
        Bow       
    }

    public WeaponType weaponType;
    public int Damage;
    public GameObject bullet;
    public float eminPower;
}
