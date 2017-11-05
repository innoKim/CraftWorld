using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

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


    private float[,] heightArr;
    
    private GameObject Map;

    void Start () {
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
        BlockGenerate();        
    }

    void BlockGenerate()
    {
        GroundGenerate();
        WaterGenerate();
    }

    void GroundGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = 0; y < heightArr[x, z]; y++)
                {
                    GameObject newBlock = Instantiate(Blocks[0], new Vector3(x, y, z), Quaternion.identity, Map.transform);
                }
            }
        }
    }
    void WaterGenerate()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int z = 0; z < MapWidth; z++)
            {
                for (int y = (int)heightArr[x, z]; y < WaterLevelHeight; y++)
                {
                    GameObject newBlock = Instantiate(Blocks[5], new Vector3(x, y, z), Quaternion.identity, Map.transform);
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
