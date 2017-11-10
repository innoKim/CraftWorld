using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpd;
    private float spd;

    public float RotSpd;
    public float JumpPower;


    private Rigidbody rb;
    private Animator anim;

    public bool isGround;
    public bool isWater;

    public Quaternion lookDirection;
    public Transform CameraTransform;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        ObjectManager.Instance.player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Jump();

        AnimationSetting();
    }

    void AnimationSetting()
    {
        anim.SetFloat("Speed", spd);
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
        if (Input.GetKey(KeyCode.LeftShift)) spd = Input.GetAxisRaw("Vertical") * MoveSpd * 2.0f;
        else spd = Input.GetAxisRaw("Vertical") * MoveSpd;

        //if (Mathf.Abs(spd) > 0.1f) transform.rotation = lookDirection;

        transform.position += transform.forward * spd * Time.deltaTime;
        //lookDirection = lookDirection * Quaternion.Euler(new Vector3(0, Input.GetAxisRaw("Horizontal") * RotSpd * Time.deltaTime, 0));
        //transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * RotSpd * Time.deltaTime, 0));
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
