using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    Rigidbody rb;
    bool used = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (!other.CompareTag("Player"))
        {
            other.SendMessage("Damaged", 2);
            Invoke("DestroyByUsed", 3.0f);
            used = !used;
        }
    }

    private void DestroyByUsed()
    {
        Destroy(this.gameObject);
    }
}
