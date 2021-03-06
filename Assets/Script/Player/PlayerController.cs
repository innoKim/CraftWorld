﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveSpd;
    private Vector3 lerpDir;
    private Vector3 moveDir;

    public float RotSpd;
    public float JumpPower;
    public float ChargePower;

    private Rigidbody rb;
    private Animator anim;

    public bool isGround;
    public bool isWater;
    public bool isPunching;
    public bool isJumping;

    private AttackColider ac;
    private Camera cam;
    private Player player;

    //for attack
    Ray ray;
    RaycastHit hit;
    Vector3 deltaVector;
    LayerMask layer;
    bool hitted;
    float timer;

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        ac = GetComponentInChildren<AttackColider>();
        cam = Camera.main.GetComponent<Camera>();
        player = GetComponent<Player>();

        //for lookdir
        ObjectManager.Instance.player = this.gameObject;
        lerpDir = Quaternion.Euler(new Vector3(0, cam.transform.rotation.eulerAngles.y, 0)) * Vector3.forward;

        //for peek collider
        layer = 1 << LayerMask.NameToLayer("Raycast");

    }

    void Update() {
        Move();
        Jump();

        Attack();
    }

    void Attack()
    {
        if(player.weapon == null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetTrigger("isPunching");
                if (!isPunching) StartCoroutine("MeleeAttack");
            }
        }
        else if (player.weapon.weaponType == Weapon.WeaponType.Bow)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.speed = 1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!isPunching) StartCoroutine("ChargeAttack");
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (timer < 0.3f) timer += Time.deltaTime;
                else anim.speed = 0.0f;
            }
        }
        else if (player.weapon.weaponType != Weapon.WeaponType.Bow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetTrigger("isPunching");
                if (!isPunching) StartCoroutine("MeleeAttack");
            }
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
            ItemManager.Instance.AddItemToInventory(other.name);
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

    private IEnumerator ChargeAttack()
    {
        anim.SetTrigger("isPunching");
        player.weapon.emitPower = 0.0f;
        timer = 0.0f;
        isPunching = true;
        yield return null;

        while (true)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            hitted = Physics.Raycast(ray, out hit, 100.0f, layer);

            if (hitted)
            {
                deltaVector = hit.point - (transform.position + Vector3.up * 0.5f);
                moveDir = new Vector3(deltaVector.x, 0, deltaVector.z).normalized;
            }

            if(player.weapon.emitPower < player.weapon.maxPower) player.weapon.emitPower += ChargePower * Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Mouse0)) break;

            yield return null;
        }
                
        yield return new WaitForSeconds(0.3f);

        player.WeaponFire(deltaVector.normalized);
        
        yield return new WaitForSeconds(0.5f);
        isPunching = false;
    }

    private IEnumerator MeleeAttack()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        hitted = Physics.Raycast(ray, out hit, 100.0f, layer);

        if (hitted)
        {
            deltaVector = hit.point - (transform.position+Vector3.up*0.5f);
            moveDir = new Vector3(deltaVector.x, 0, deltaVector.z).normalized;
        }

        isPunching = true;
        yield return new WaitForSeconds(0.3f);

        if (hitted&&deltaVector.sqrMagnitude<2.0f)
        {
            Weapon weapon = GetComponent<Player>().weapon;
            if(weapon)
            {
                hit.collider.gameObject.SendMessage("Damaged", GetComponent<Player>().weapon.Damage + 1);
            }
            else hit.collider.gameObject.SendMessage("Damaged", 1);
        }


        yield return new WaitForSeconds(0.5f);
        isPunching = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + moveDir);
    }
}
