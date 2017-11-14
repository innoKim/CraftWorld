using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpd;
    private Vector3 lerpDir;
    private Vector3 moveDir;

    public float RotSpd;
    public float JumpPower;

    private Rigidbody rb;
    private Animator anim;

    public bool isGround;
    public bool isWater;
    public bool isPunching;
    public bool isJumping;

    private AttackColider ac;
    private Camera cam;

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ac = GetComponentInChildren<AttackColider>();
        cam = Camera.main.GetComponent<Camera>();

        ObjectManager.Instance.player = this.gameObject;
        lerpDir = Quaternion.Euler(new Vector3(0, cam.transform.rotation.eulerAngles.y, 0)) * Vector3.forward;
    }

    void Update() {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("isPunching");
            if (!isPunching) StartCoroutine("Punch");
        }
    }

    void Jump()
    {
        if (isPunching) return;

        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
            isGround = false;
            anim.SetTrigger("isJumping");
        }
        else if (isWater && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpPower);
        }
    }

    void Move()
    {
        lerpDir = Vector3.Slerp(lerpDir, moveDir, RotSpd*Time.deltaTime);
        transform.LookAt(transform.position + lerpDir);

        if (isPunching && isGround) return;
        if (isGround && isJumping) return;

        Vector3 inputDir = (Vector3.forward * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal")).normalized;
        moveDir = Quaternion.Euler(new Vector3(0, cam.transform.rotation.eulerAngles.y, 0)) * inputDir;

        if (moveDir.sqrMagnitude >= 0.1f)
        {
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
        transform.position += moveVelocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain") || other.CompareTag("Tree")||other.CompareTag("Rock"))
        {
            isGround = true;
        }
        if(other.CompareTag("Water"))
        {
            isWater = true;
            rb.drag = 5.0f;
        }
        if(other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
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

    private IEnumerator Punch()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 deltaVector = new Vector3();
        bool hitted = Physics.Raycast(ray, out hit, 100.0f);

        if (hitted)
        {
            deltaVector = hit.point - (transform.position+Vector3.up*0.5f);
            moveDir = new Vector3(deltaVector.x, 0, deltaVector.z).normalized;
        }

        isPunching = true;
        yield return new WaitForSeconds(0.3f);

        if (hitted&&deltaVector.sqrMagnitude<2.0f&&!hit.collider.CompareTag("Player"))
            hit.collider.gameObject.SendMessage("Damaged");

        yield return new WaitForSeconds(0.5f);
        isPunching = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + moveDir);
    }
}
