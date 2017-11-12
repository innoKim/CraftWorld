using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColider : MonoBehaviour {
        
    [Range(0.0f,1.0f)]
    public float vibrateTime;

    [Range(0.0f, 1.0f)]
    public float vibrateDistance;

    PlayerController pc;
    
    // Use this for initialization
    void Start () {
        pc = transform.parent.GetComponent<PlayerController>();
	}
	
    private void OnTriggerStay(Collider other)
    {
        if (pc.isPunching)
        {
            if (other.tag == "Terrain"|| other.tag == "Rock" || other.tag == "Tree")
            {
                other.gameObject.SendMessage("Damaged");
                gameObject.SetActive(false);
            }
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
