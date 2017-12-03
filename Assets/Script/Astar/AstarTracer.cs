using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTracer : MonoBehaviour {

    //inspector
    public Transform targetTransform;
    public float PathFindPeriod;
    public float spd;

    Rigidbody rb;

    //trace variable
    public bool isTracing;
    public bool isJumping;
    float timer = 0.0f;

    LinkedList<Node> path;

    Node afterNode;
    public Vector3 afterNodePos;
    Vector3 targetDir;
        
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        path = new LinkedList<Node>();
        afterNode = null;

        if(!GetComponent<SphereCollider>())
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.center = new Vector3(0, -0.4f, 0);
            sc.radius = 0.2f;
            sc.isTrigger = true;
        }
    }

    void Update () {
        if(isTracing)
        {
            if (!targetTransform)
            {
                Debug.Log("There is no target to trace.");
                return;
            }
            else
            {
                timer += Time.deltaTime;
                if(timer>PathFindPeriod)
                {
                    timer = 0.0f;
                    GetPath();
                }

                if(path.Count>0)
                {
                    TracePath();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isTracing = true;
        }
    }

    void TracePath()
    {        
        if(afterNode == null)
        {
            SetAfterNode();
        }

        if ((afterNodePos-transform.position).sqrMagnitude>0.1f)
        {
            targetDir = (afterNodePos - transform.position).normalized;
            transform.LookAt(transform.position + targetDir);

            //높이가 한칸차이나면 점프 그 이상은 이동 종료
            if(!isJumping&&afterNodePos.y > transform.position.y + 0.1f)
            {
                isJumping = true;
                rb.AddForce(Vector3.up * 250 + (afterNodePos - transform.position) * 50);
            }
            else 
            {
                if(!isJumping)
                    rb.MovePosition(transform.position + targetDir * spd * Time.deltaTime);
            }
        }
        else
        {
            SetAfterNode();
        }

        if ((transform.position - targetTransform.position).sqrMagnitude < 1.0f|| afterNodePos.y > transform.position.y + 1.1f)
        {
            isTracing = false;
            afterNode = null;
            path.Clear();
        }
    }

    void SetAfterNode()
    {
        afterNode = path.First.Value;
        afterNodePos = new Vector3(path.First.Value.worldPosX, path.First.Value.Height, path.First.Value.worldPosZ);
        path.RemoveFirst();
    }

    void GetPath()
    {
        List<Node> targetPath = Astar.Instance.pathFind(this.transform, targetTransform);

        if(targetPath == null)
        {
            Debug.Log("There is no path.");
            return;
        }

        if(targetTransform==null)
        {
            Debug.Log("Path is empty.");
            return;
        }

        path.Clear();
        for(int i=0;i<targetPath.Count;i++)
        {
            path.AddLast(targetPath[i]);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            Invoke("Landing", 0.1f);
        }
    }

    void Landing()
    {
        isJumping = false;
    }
}

