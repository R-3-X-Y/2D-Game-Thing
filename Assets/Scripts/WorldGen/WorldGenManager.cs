using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenManager : MonoBehaviour
{
    [Header("Terrain Gen")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float smoothness;
    private int[] perlinHeightList; 
    [SerializeField] private string seed = "0";
    [SerializeField] private int smoothAmount;

    [Header("Cave Gen")]
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    private int seedHash;
    [Header("Tile")]
    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile dirtTile;
    [SerializeField] private Tile stoneTile;
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap bgTileMap;
    
    private int[,] map;
    private void Awake()
    {
        if (WorldCreationData.instance != null)
        {
            width = WorldCreationData.instance.width;
            height = WorldCreationData.instance.height;
            seed = WorldCreationData.instance.seed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        seedHash = seed.GetHashCode();
        Random.InitState(seedHash);
        perlinHeightList = new int[width];
        map = GenerateArray(width, height, true);
        TerrainGeneration();
        SmoothMap(smoothAmount);
        BottomCut();
        RenderMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (empty) ? 0 : 1;
            }
        }
        return map;
    }

    public void TerrainGeneration()
    {
        System.Random pesudoRandom = new System.Random(seedHash);
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seedHash) * 60);
            perlinHeight += height / 2;
            perlinHeightList[x] = perlinHeight;
            for (int y = 0; y < perlinHeight; y++)
            {
                map[x, y] = (pesudoRandom.Next(1, 100) < randomFillPercent) ? 1 : 2;
            }
        }
    }

    private void SmoothMap(int smoothAmount)
    {
        for (int i = 0; i < smoothAmount; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perlinHeightList[x]; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightList[x] - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        int surroundingGroundCount = GetSurroundingGroundCount(x, y);
                        if ( surroundingGroundCount > 4)
                        {
                            map[x, y] = 1;
                        }
                        else if (surroundingGroundCount < 4)
                        {
                            map[x, y] = 2;
                        }
                    }
                }
            }
        }
    }

    private int GetSurroundingGroundCount(int gridX, int gridY)
    {
        int groundCount = 0;
        for (int nebX = gridX - 1; nebX <= gridX + 1; nebX++)
        {
            for (int nebY = gridY - 1; nebY <= gridY + 1; nebY++)
            {
                if (nebX >= 0 && nebX < width && nebY < height)
                {
                    if (nebX != gridX || nebY != gridY)
                    {
                        if (map[nebX, nebY] == 1)
                        {
                            groundCount++;
                        }
                    }
                }
            }
        }

        return groundCount;
    }

    public void RenderMap()
    {
        for (int x = 0; x < width; x++)
        {
            int dirtHeight = Random.Range(3, 6);
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    if (map[x, y + 1] == 0)
                    {
                        if (map[x, y - 1] == 1)
                        {
                            groundTileMap.SetTile(new Vector3Int(x, y, 0), grassTile);
                            bgTileMap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                        }
                    }
                    else if (map[x, y + Random.Range(3, 6)] == 0)
                    {
                        groundTileMap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                        bgTileMap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                    }
                    else
                    {
                        groundTileMap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                        bgTileMap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                    }
                    
                    if (x == Mathf.RoundToInt(width / 2))
                    {
                        Camera.main.transform.parent.position = new Vector3(x + 0.5f, y + 5, 0);
                    }
                }
                else if (map[x, y] == 2)
                {
                    bgTileMap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                }
            }
        }
    }

    private void BottomCut()
    {
        int tmpHeight = Mathf.RoundToInt(height / 1.5f);
        for (int x = 0; x < width; x++)
        {
            if (x < width / 2)
            {
                tmpHeight += Random.Range(-4, 2);
            }
            else
            {
                tmpHeight += Random.Range(-1, 5);
            }
            for (int y = 0; y < tmpHeight; y++)
            {
                map[x, y] = 0;
            }
        }
    }
}
