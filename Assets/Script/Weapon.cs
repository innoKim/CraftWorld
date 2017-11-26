using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquipItem
{
    public enum WeaponType
    {
        Sword,
        Bow,
        None
    }

    public WeaponType weaponType;
    public int Damage;
}
