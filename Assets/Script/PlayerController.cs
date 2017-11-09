using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpd;
    public float RotSpd;
    public float JumpPower;

    private Rigidbody rb;

    public bool isGround;
    public bool isWater;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ObjectManager.Instance.player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Jump();
    }

    void Jump()
    {
        if(isGround&&Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
            isGround = false;
        }
        else if(isWater && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
        }
    }
    void Move()
    {
        transform.position += transform.forward * Input.GetAxisRaw("Vertical")*MoveSpd * Time.deltaTime;
        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * RotSpd * Time.deltaTime, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Terrain"))
        {
            isGround = true;
        }
        if(other.CompareTag("Water"))
        {
            isWater = true;
            rb.drag = 5.0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isWater = false;
            rb.drag = 0.0f;
        }
    }

    
}
