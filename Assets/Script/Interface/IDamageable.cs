using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable{

    int MaxHp { get; set; }
    int CurHp { get; set; }
    
    void Damaged();
    void Destroyed();
}
