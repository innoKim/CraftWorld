using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpd;
    private Vector3 lerpDir;

    public float RotSpd;
    public float JumpPower;

    private Rigidbody rb;
    private Animator anim;

    public bool isGround;
    public bool isWater;
    
    public Transform CameraTransform;

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        ObjectManager.Instance.player = this.gameObject;
        lerpDir = Quaternion.Euler(new Vector3(0, CameraTransform.rotation.eulerAngles.y, 0))*Vector3.forward;
    }

    void Update() {
        Move();
        Jump();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("isPunching");
        }
    }

    void Jump()
    {
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
            isGround = false;
        }
        else if (isWater && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
        }
    }

    void Move()
    {
        Vector3 inputDir = (Vector3.forward * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal")).normalized;
        Vector3 moveDir = Quaternion.Euler(new Vector3(0, CameraTransform.rotation.eulerAngles.y, 0)) * inputDir;

        if(moveDir.sqrMagnitude>=0.1f)
        {
            lerpDir = Vector3.Slerp(lerpDir, moveDir, 0.1f);
            anim.SetBool("isMoving", true);
        }
        else anim.SetBool("isMoving", false);

        Vector3 moveVelocity;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVelocity = moveDir * MoveSpd * 2.0f;
            anim.SetBool("isRunning", true);
        }
        else
        {
            moveVelocity = moveDir * MoveSpd;
            anim.SetBool("isRunning", false);
        }

        transform.LookAt(transform.position + lerpDir);
        transform.position += moveVelocity * Time.deltaTime;
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
