using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable{

    HealthBar HpBar { get;}
    int MaxHp { get; set; }
    int CurHp { get; set; }
    
    void Damaged(int damage);
    void Destroyed();
}
