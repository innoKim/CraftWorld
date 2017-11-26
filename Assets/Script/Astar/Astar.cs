using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float worldPosX,worldPosZ;
    public int x, z;
    public Node parent = null;

    public int gCost = 0;
    public int hCost = 0;
    public int FCost
    {
        get { return gCost + hCost; }
    }

    public int Height
    {
        get
        {
            return ObjectManager.Instance.Height(x, z);
        }
    }
}

public class AstarGrid
{
    Node[,] nodes = null;
    int width, depth;

    public void SetGrid(int width, int depth)
    {
        this.width = width;
        this.depth = depth;

        nodes = new Node[width, depth];
       
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                nodes[i, j] = new Node();
                nodes[i, j].worldPosX = i;
                nodes[i, j].worldPosZ = j;
                nodes[i, j].x = i;
                nodes[i, j].z = j;
            }
        }
    }    

    public List<Node> GetNeighbours(int x, int z)
    {
        List<Node> neighbours = new List<Node>();

        if (nodes != null)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int _i = x + i,_j = z+j;
                    if (_i < 0 || _i >= width) continue;
                    if (_j < 0 || _j>= depth) continue;

                    //물속의 지형이거나
                    if (nodes[_i, _j].Height < ObjectManager.Instance.waterHeight) continue;
                    //지형위에 오브젝트가 있거나
                    if (ObjectManager.Instance.objArr[_i, (int)nodes[_i, _j].Height, _j] != ObjectManager.ObjType.None) continue;
                    //대각이동시 걸리는 곳이 있거나
                    if(Mathf.Abs(i)+ Mathf.Abs(j)==2)
                    {
                        if (x < 0 || x >= width) continue;
                        if (ObjectManager.Instance.objArr[x, (int)nodes[x, _j].Height, _j] != ObjectManager.ObjType.None) continue;

                        if (z < 0 || z >= depth) continue;
                        if (ObjectManager.Instance.objArr[_i, (int)nodes[_i, z].Height, z] != ObjectManager.ObjType.None) continue;
                    }

                    neighbours.Add(nodes[_i, _j]);
                }
            }
        }
        return neighbours;
    }

    public List<Node> GetNeighbours(Node A)
    {
        return GetNeighbours(A.x, A.z);
    }

    public Node GetNode(int x, int z)
    {
        int _x = x, _z = z;

        Mathf.Clamp(_x, 0, width);
        Mathf.Clamp(_z, 0, depth);

        return nodes[_x, _z];
    }

    static public int GetDistance(Node A, Node B)
    {
        int w, d;
        w = Mathf.Abs(A.x - B.x);
        d = Mathf.Abs(A.z - B.z);

        if (w > d) return (w - d) * 10 + d * 14;
        else return (d - w) * 10 + w * 14;
    }
}

public class Astar : MonoBehaviour {

    public AstarGrid grid;

    static private Astar instance;
    
    static public Astar Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }
            else
            {
                GameObject obj = new GameObject("_AstarManager");
                instance = obj.AddComponent<Astar>();
                instance.grid = new AstarGrid();
                instance.grid.SetGrid(ObjectManager.Instance.mapWidth, ObjectManager.Instance.mapDepth);

                return instance;
            }
        }
    }
    
    public List<Node> pathFind(Transform start, Transform dest)
    {
        return pathFind(Mathf.RoundToInt(start.position.x), Mathf.RoundToInt(start.position.z), Mathf.RoundToInt(dest.position.x), Mathf.RoundToInt(dest.position.z));
    }

    public List<Node> pathFind(int startX, int startZ, int destX, int destZ)
    {
        int n = 0;

        Node startNode = grid.GetNode(startX, startZ);
        Node targetNode = grid.GetNode(destX, destZ);

        List<Node> openSet = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();

        openSet.Add(startNode);
        
        while (openSet.Count > 0)
        {
            n++;
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            ClosedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (ClosedSet.Contains(neighbour)) continue;

                int costToNeighbour = currentNode.gCost + AstarGrid.GetDistance(currentNode, neighbour);

                if (costToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = costToNeighbour;
                    neighbour.hCost = AstarGrid.GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.Log(n);
        return null;
    }

    List<Node> RetracePath(Node startNode, Node targetNode)
    {
        List<Node> temp = new List<Node>();

        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            temp.Add(currentNode);
            currentNode = currentNode.parent;
        }

        temp.Reverse();

        return temp; 
    }
}
