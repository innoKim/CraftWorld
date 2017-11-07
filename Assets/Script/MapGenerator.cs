using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    private enum ObjType
    {
        Soil,
        Sand,
        Gravel,
        Rock,
        Metal,
        Water,
        None,
    }

    public List<GameObject> Blocks;
    
    public int MapWidth;
    public int MapDepth;
    public int WaterLevelHeight;
    
    [Range(0, 100)]
    public float perlinScale;

    [Range(0, 20)]
    public float heightScale;

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
	void Update () {
    }

    void MapGenerate()
    {
        Map = new GameObject("Map");
        Map.isStatic = true;

        NoiseGenerate(0, 0);

        ObjMapGenerate();
        BlockGenerate();
    }

    void ObjMapGenerate()
    {
        objmapArr = new ObjType[MapWidth, (int)heightScale, MapDepth];

        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < heightScale; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    objmapArr[x, y, z] = ObjType.None;
                }
            }
        }

        GroundGenerate();
        MetalGenerate();
        RockGenerate();        
        WaterGenerate();
    }

    void BlockGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < heightScale; y++)
            {
                for (int z = 0; z < MapWidth; z++)
                {
                    if(objmapArr[x,y,z] !=ObjType.None)
                    {
                        GameObject newBlock = Instantiate(Blocks[(int)objmapArr[x, y, z]], new Vector3(x, y, z), Quaternion.identity, Map.transform);
                    }
                }
            }
        }
    }

    void GroundGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = 0; y < heightArr[x, z]; y++)
                {
                    objmapArr[x, y, z] = ObjType.Soil;
                }               
            }
        }
    }

    void MetalGenerate()
    {
        for(int i=0;i<100;i++)
        {
            int xRand = Random.Range(0, MapWidth);
            int yRand = (int)Random.Range(0, heightScale);
            int zRand = Random.Range(0, MapDepth);

            if (objmapArr[xRand,yRand,zRand]!=ObjType.None)
            {
                objmapArr[xRand, yRand, zRand] = ObjType.Metal;
            }
        }
    }

    void RockGenerate()
    {
        for (int i = 0; i < 30; i++)
        {
            int xRand = Random.Range(0, MapWidth);
            int zRand = Random.Range(0, MapDepth);

            if (objmapArr[xRand, (int)heightArr[xRand, zRand]+1, zRand] == ObjType.None)
            {
                objmapArr[xRand, (int)heightArr[xRand, zRand] + 1, zRand] = ObjType.Rock;
            }
        }
    }
    
    void WaterGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = (int)heightArr[x, z]+1; y < WaterLevelHeight; y++)
                {
                    if(objmapArr[x,y,z]==ObjType.None)
                        objmapArr[x, y, z] = ObjType.Water;
                }
            }
        }
    }

    void NoiseGenerate(int xOrg, int zOrg)
    {
        heightArr = new float[MapWidth,MapDepth];

        for(int x=0;x<MapWidth;x++)
        {
            for(int z=0;z<MapDepth;z++)
            {
                heightArr[x,z] = Mathf.PerlinNoise((float)(x+ xSeed) /MapWidth*perlinScale, (float)(z+ zSeed) /MapDepth* perlinScale) * heightScale;
            }
        }
    }
}
