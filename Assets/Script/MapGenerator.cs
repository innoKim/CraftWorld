using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    private enum ObjType
    {
        Soil,
        Sand,
        Gravel,
        Stone,
        Metal,
        Water,
        Soil2,
        Tree,
        Rock,
        None,
    }

    private enum TreeType
    {
        Bush,
        Tree1,
        Tree2,
        Tree3,
        None
    }

    private enum RockType
    {
        Rock,
        None
    }

    public List<GameObject> Blocks;
    public List<GameObject> Trees;
    public List<GameObject> Rocks;

    public int MapWidth;
    public int MapDepth;
    public int MapHeight;
    public int WaterLevelHeight;

    [Range(0, 100)]
    public float perlinScale;

    [Range(0, 20)]
    public float heightScale;

    [Range(0, 1.0f)]
    public float treeRatio;

    [Range(0, 1.0f)]
    public float RockRatio;

    public int xSeed;
    public int zSeed;

    private ObjType[,,] objmapArr;

    private float[,] heightArr;

    private GameObject Map;

    void Start()
    {
        MapGenerate();
    }

    // Update is called once per frame
    void Update() {
    }

    void MapGenerate()
    {
        Map = new GameObject("Map");
        Map.isStatic = true;

        NoiseGenerate();

        ObjMapGenerate();

        BlockGenerate();
    }

    void ObjMapGenerate()
    {
        objmapArr = new ObjType[MapWidth, MapHeight, MapDepth];

        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    objmapArr[x, y, z] = ObjType.None;
                }
            }
        }

        SoilGenerate();
        WaterGenerate();
        GravelGenerate();
        TreeGenerate();
        RockGenerate();
        //MetalGenerate();
    }

    void BlockGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < heightScale; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    switch (objmapArr[x, y, z])
                    {
                        case ObjType.None:
                            continue;
                            break;
                        case ObjType.Tree:
                            {
                                int ranNum = Random.Range(0, (int)TreeType.None);
                                GameObject newTree = Instantiate(Trees[ranNum], new Vector3(x, y - 0.3f, z), Quaternion.Euler(-90, Random.RandomRange(0.0f, 360.0f), 0), Map.transform);
                                if (ranNum == (int)TreeType.Bush) newTree.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.8f, 1.2f);
                                else newTree.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.4f, 0.6f);
                            }
                            break;
                        case ObjType.Rock:
                            {
                                int ranNum = Random.Range(0, (int)RockType.None);
                                GameObject newRock = Instantiate(Rocks[ranNum], new Vector3(x, y-0.3f, z), Quaternion.Euler(-90, Random.RandomRange(0.0f, 360.0f), 0), Map.transform);
                                newRock.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.5f, 1.5f);
                            }
                            break;
                        default:
                            GameObject newBlock = Instantiate(Blocks[(int)objmapArr[x, y, z]], new Vector3(x, y, z), Quaternion.identity, Map.transform);
                            break;
                    }
                }
            }
        }
    }

    void SoilGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = 0; y < heightArr[x, z]; y++)
                {
                    if (y == (int)heightArr[x, z])
                    {
                        objmapArr[x, y, z] = BlendGenerate(ObjType.Soil, ObjType.Soil2, 0.9f);

                        //if (objmapArr[x, y, z] == ObjType.Soil) objmapArr[x, y + 1, z] = BlendGenerate(ObjType.None, ObjType.Tree, 9, 1);
                    }
                    else objmapArr[x, y, z] = BlendGenerate(ObjType.Soil2, ObjType.Metal, 0.8f);
                }
            }
        }
    }

    void GravelGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < heightScale; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    if (objmapArr[x, y, z] == ObjType.Soil || objmapArr[x, y, z] == ObjType.Soil2)
                        if (IsNeighbourOf(ObjType.Water, x, y, z))
                            objmapArr[x, y, z] = BlendGenerate(ObjType.Soil2, ObjType.Sand, 0.3f);
                }
            }
        }
    }

    void MetalGenerate()
    {
        for (int i = 0; i < 100; i++)
        {
            int xRand = Random.Range(0, MapWidth);
            int yRand = (int)Random.Range(0, heightScale);
            int zRand = Random.Range(0, MapDepth);

            if (objmapArr[xRand, yRand, zRand] != ObjType.None)
            {
                objmapArr[xRand, yRand, zRand] = ObjType.Metal;
            }
        }
    }
    
    void WaterGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = (int)heightArr[x, z] + 1; y < WaterLevelHeight; y++)
                {
                    if (objmapArr[x, y, z] == ObjType.None)
                        objmapArr[x, y, z] = ObjType.Water;
                }
            }
        }

        //물 생성이후 물 옆의 블럭들을 자연스럽게 바꾸어준다.

        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < heightScale; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    if (objmapArr[x, y, z] == ObjType.Soil || objmapArr[x, y, z] == ObjType.Soil2)
                        if (IsNeighbourOf(ObjType.Water, x, y, z))
                            objmapArr[x, y, z] = ObjType.Soil2;
                }
            }
        }
    }

    void NoiseGenerate()
    {
        heightArr = new float[MapWidth, MapDepth];

        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapDepth; z++)
            {
                heightArr[x, z] = Mathf.PerlinNoise((float)(x + xSeed) / MapWidth * perlinScale, (float)(z + zSeed) / MapDepth * perlinScale) * heightScale;
            }
        }
    }

    bool IsNeighbourOf(ObjType targetObjType, int x, int y, int z)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (y + j < 0 || y + j >= MapHeight || x + i < 0 || x + i >= MapWidth || z + k < 0 || z + k >= MapDepth) continue;
                    if (i == 0 && j == 0 && k == 0) continue;

                    if (objmapArr[x + i, y + j, z + k] == targetObjType)
                        return true;
                }
            }
        }
        return false;
    }

    

    void TreeGenerate()
    {
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapDepth; j++)
            {
                if (heightArr[i, j] < WaterLevelHeight) continue;

                if(objmapArr[i,(int)heightArr[i,j],j] == ObjType.Soil)
                {
                    objmapArr[i, (int)heightArr[i, j] + 1, j] = BlendGenerate(ObjType.Tree, ObjType.None, treeRatio);
                }
            }
        }
    }

    void RockGenerate()
    {
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapDepth; j++)
            {
                if (heightArr[i, j] < WaterLevelHeight) continue;

                if(objmapArr[i, (int)heightArr[i, j] + 1, j] == ObjType.None)
                    objmapArr[i, (int)heightArr[i, j] + 1, j] = BlendGenerate(ObjType.Rock, ObjType.None, RockRatio);
            }
        }
    }

    ObjType BlendGenerate(ObjType A, ObjType B, float ratio)
    {
        float randomNum = Random.Range(0.0f, 1.0f);

        if (randomNum < ratio) return A;
        else return B;
    }
}
