using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public BuildingManager building;

    public bool usePregenSeed = false;
    public int pregenSeed;

    [SerializeField]
    int seedDebug = 0;
    System.Random seedSelector = new System.Random();
    public System.Random random;

    public Tilemap enviroTiles;
    public Tilemap waterTiles;

    public int genHeight;
    public int genWidth;

    [Header("Map Gen Debug")]
    public int centreIndex;
    public int arrayLength;
    public int arrayWidth;
    public int dirtHeight;
    public int dirtStepInt;
    public List<int> dirtSteps;
    public int dirtPadding;

    //TODO - Use the coords to lookup a tile on all the respective boards and figure out wtf's goin on with it
    //public Dictionary<int[,], int> enviroBoard;
    //public Dictionary<int[,], int> buildingBoard;
    //public Dictionary<int[,], int> waterBoard;

    public int[,] enviroBoard;
    public int[,] buildingBoard;
    public int[,] waterBoard;

    //terrain type ints
    //0 - nada
    //1 - rock - water can't penetrate
    //2 - dirt - buildable + water can seep through
    //3 - building

    //Building ints 
    //0 - nada
    //1 - Wall (stops water)
    //2 - Occupied (building already has this tile)

    //water ints 
    //0 - dry
    //1 - soggy

    public Sprite rockSprite;
    public Sprite dirtSprite;
    public Sprite waterSprite;

    [Header("Water Level Debug")]
    public int waterLevel;

    private void Awake()
    {
        seedDebug = Environment.TickCount;
        if (usePregenSeed)
        {
            random = new System.Random(pregenSeed);
        }else
        {
            random = new System.Random(seedDebug);
        }
    }

    /*
    private void Start()
    {
        GenerateBoards(genWidth, genHeight);
        UpdateEnviroTiles();
        RaiseWater(30);
    }
    */

    public void GameTick()
    {
        if(waterLevel < GameManager.gameManager.boardHeight -1 )
        {
            int newWater = waterLevel + 1;
            RaiseWater(newWater);
        } else
        {
            GameManager.gameManager.EndGame(true);
        }
    }

    public void SetupGameBoards(int width, int height, int startingWaterLevel)
    {
        genWidth = width;
        genHeight = height;

        GenerateBoards(genWidth, genHeight);
        UpdateEnviroTiles();
        RaiseWater(startingWaterLevel);
    }

    void GenerateBoards(int width, int height)
    {
        enviroBoard = new int[height, width];
        buildingBoard = new int[height, width];
        waterBoard = new int[height, width];


        //gotta actually do some GENERATIN' for the enviro
        //get the middle of the board, then we need to do some kind of bell curve for the stone, then the dirt
        centreIndex = width / 2 + random.Next(Mathf.RoundToInt(-width / 5),Mathf.RoundToInt (width / 5));
        int islandHeight = height - (height / 6);
        

        arrayLength = enviroBoard.GetLength(0);
        arrayWidth = enviroBoard.GetLength(1);

        int leftSideColumns = centreIndex;
        int rightSideColumns = arrayWidth - (centreIndex + 1);

        //Do a normal-ish distribution of rock
        int[] heightMapRock = new int[arrayWidth];
        int[] heightMapDirt = new int[arrayWidth];
        for (int x = 0; x < arrayWidth; x++)
        {
            if(x == centreIndex)
            {
                heightMapRock[x] = islandHeight;
            } else if (x > centreIndex)
            {
                int distToCenter = x - centreIndex;
                //Linear
                float falloff = 1f - ((float)distToCenter / (float)rightSideColumns); 
                int landHeight = Mathf.RoundToInt(islandHeight * falloff);
                heightMapRock[x] = landHeight;

                //Debug.Log("Column " + x + " is " + distToCenter + " of " + rightSideColumns + " from the center index and has height " + landHeight);
            } else
            {
                int distToCenter = centreIndex - x;
                float falloff = (1f - ((float)distToCenter / (float)leftSideColumns));
                int landHeight = Mathf.RoundToInt(islandHeight * falloff);
                heightMapRock[x] = landHeight;

                //Debug.Log("Column " + x + " is " + distToCenter + " of " + leftSideColumns + " from the center index and has height " + landHeight + "(Falloff: "+ falloff + ")");
            }
        }

        //Then we'll layer on the dirt - TODO:
        dirtHeight = (Mathf.RoundToInt((float)arrayLength / 6f) * 5);
        dirtPadding = Mathf.RoundToInt((float)arrayWidth / 20f);
        dirtStepInt = Mathf.RoundToInt((float)arrayLength / 8f);
        dirtSteps = new List<int>();
        int maxSteps = Mathf.RoundToInt((float)dirtHeight/ (float)dirtStepInt);
        Debug.Log("MaxSteps: " + maxSteps);
        for(int i = 0; i < maxSteps; i++)
        {
            dirtSteps.Add(dirtStepInt * i);
        }

        

        //random, small hills
        int currentYOffset;
        int currentOffsetifeSpan;

        currentYOffset = random.Next(-1, 1);
        currentOffsetifeSpan = random.Next(5, 12);

        /* Big flat plain mode
        for (int x = 0; x < arrayWidth; x++)
        {
            if(x < dirtPadding || x > arrayWidth - dirtPadding)
            {
                //no dirt
                heightMapDirt[x] = 0;

            } else if (x < dirtPadding * 2 || x > arrayWidth - (dirtPadding * 2))
            {
                //ramp up/down dirt
                if(x < centreIndex)
                {
                    int distToCutoff = (dirtPadding * 2) - x;
                    float falloff = (1f - ((float)distToCutoff / (float)dirtPadding));
                    int landHeight = Mathf.RoundToInt(dirtHeight * falloff);
                    heightMapDirt[x] = landHeight;

                } else
                {
                    int distToCutoff = x - (arrayWidth - (dirtPadding * 2));
                    float falloff = (1f - ((float)distToCutoff / (float)dirtPadding));
                    int landHeight = Mathf.RoundToInt(dirtHeight * falloff);
                    heightMapDirt[x] = landHeight;
                }
            } else
            {
                //max dirt + offset
                int landHeight = dirtHeight + currentYOffset;
                heightMapDirt[x] = landHeight;
                currentOffsetifeSpan--;
                if(currentOffsetifeSpan <= 0)
                {
                    currentYOffset = random.Next(-2, 2);
                    currentOffsetifeSpan = random.Next(5, 12);
                }
            }
            
        }
        */

        for (int x = 0; x < arrayWidth; x++)
        {
            if (x < dirtPadding || x > arrayWidth - dirtPadding)
            {
                heightMapDirt[x] = 0;
            } else
            {
                //then just feed 'er into a step funtion
                int rockHeight = heightMapRock[x];
                int landHeight = 0;
                for(int i = 0; i < dirtSteps.Count; i++)
                {
                    if(dirtSteps[i] > rockHeight)
                    {
                        landHeight = dirtSteps[i];
                        break;
                    }
                }

                landHeight += currentYOffset;
                heightMapDirt[x] = landHeight;
                currentOffsetifeSpan--;
                if (currentOffsetifeSpan <= 0)
                {
                    currentYOffset = random.Next(-1, 1);
                    currentOffsetifeSpan = random.Next(5, 12);
                }
            }
        }

        //Set Array based on heightMaps
        for (int y = 0; y < arrayLength; y++)
        {
            for(int x = 0; x < arrayWidth; x++)
            {
                if(y > heightMapRock[x])
                {
                    if(y > heightMapDirt[x])
                    {
                        enviroBoard[y, x] = 0;
                    } else
                    {
                        enviroBoard[y, x] = 2;
                    }
                } else
                {
                    enviroBoard[y, x] = 1;
                }
            }
        }

        //building is blank, except for the victory structure which goes UP HIGH

        building.BuildBuilding(building.objectivePrefab, new Vector3Int(centreIndex, islandHeight, 0));
        
    }

    void UpdateEnviroTiles()
    {
        int arrayLength = enviroBoard.GetLength(0);
        int arrayWidth = enviroBoard.GetLength(1);

        //enviroTiles.DeleteCells()

        for (int y = 0; y < arrayLength; y++)
        {
            for (int x = 0; x < arrayWidth; x++)
            {
                int spriteIndex = enviroBoard[y, x];
                Vector3Int spriteCoord = new Vector3Int(x, y, 0);

                Sprite targetSprite = null;
                switch (spriteIndex)
                {
                    case 0:
                        targetSprite = null;
                        break;
                    case 1:
                        targetSprite = rockSprite;
                        break;
                    case 2:
                        targetSprite = dirtSprite;
                        break;
                    default:
                        targetSprite = null;
                        break;
                }

                TileBase tileBase = enviroTiles.GetTile(spriteCoord);
                Tile tile = null;
                if (tileBase != null)
                {
                    tile = tileBase as Tile;
                    tile.sprite = targetSprite;
                }

                if(tile == null)
                {
                    tile = ScriptableObject.CreateInstance("Tile") as Tile;
                    tile.sprite = targetSprite;
                    enviroTiles.SetTile(spriteCoord, tile);
                }
            }
        }

        enviroTiles.RefreshAllTiles();
    }



    public void RaiseWater(int newHeight)
    {
        //int targetRow = (arrayLength -1) - newHeight;
        int targetRow = newHeight;
        //Debug.Log("Flooding Row " + targetRow);

        //start from one end, walk towards the center until the underlying building board or enviro board stops it
        for(int x = 0; x < arrayWidth; x++)
        {
            //Debug.Log("Checking square " + targetRow + ", " + x + " for flooding");
            if(enviroBoard[targetRow, x] == 1 || buildingBoard[targetRow, x] == 1)
            {
                //Hit a wall, we're done here
                break;
            } else
            {
                FloodTile(targetRow, x);
            }
        }

        //then repeat from the other end
        for(int y = arrayWidth - 1 ; y >= 0; y--)
        {
            //Debug.Log("Checking square " + targetRow + ", " + y + " for flooding");
            if (enviroBoard[targetRow, y] == 1 || buildingBoard[targetRow, y] == 1)
            {
                //Hit a wall, we're done here
                break;
            }
            else
            {
                FloodTile(targetRow, y);
            }
        }

        waterLevel = newHeight;
    }


    void FloodTile(int y, int x)
    {
        //TODO FloodFill if time allows

        waterBoard[y, x] = 1;
        Vector3Int spriteCoord = new Vector3Int(x, y, 0);
        TileBase tileBase = waterTiles.GetTile(spriteCoord);
        Tile tile = null;
        if (tileBase != null)
        {
            tile = tileBase as Tile;
            tile.sprite = waterSprite;
        }

        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance("Tile") as Tile;
            tile.sprite = waterSprite;
            enviroTiles.SetTile(spriteCoord, tile);
        }

        if(buildingBoard[y, x] > 1 || enviroBoard[y, x] > 2)
        {
            Debug.Log("Flooding checking for building at Tile " + x.ToString() + ", " + y.ToString());
            Building toDemo = building.FindBuildingByTile(x, y);
            building.DestroyBuilding(toDemo);
        }

        //Debug.Log("Flooded Tile " + y + ", " + x);

        //then check the tile below until we hit rock, wall or more water
        List<Vector2> tilesToFlood = new List<Vector2>();

        bool flooding = true;
        int checkDepth = 1;
        while (flooding)
        {
            int floodRow = y - checkDepth;

            if(waterBoard[floodRow, x] != 1 && floodRow >= 0)
            {
                if(enviroBoard[floodRow, x] == 1 || buildingBoard[floodRow, x] == 1)
                {
                    flooding = false;
                } else
                {
                    //swimmin with da fishes
                    waterBoard[floodRow, x] = 1;
                    spriteCoord = new Vector3Int(x, floodRow, 0);
                    tileBase = waterTiles.GetTile(spriteCoord);
                    tile = null;
                    if (tileBase != null)
                    {
                        tile = tileBase as Tile;
                        tile.sprite = waterSprite;
                    }

                    if (tile == null)
                    {
                        tile = ScriptableObject.CreateInstance("Tile") as Tile;
                        tile.sprite = waterSprite;
                        enviroTiles.SetTile(spriteCoord, tile);
                    }
                    if (buildingBoard[y, x] > 1 || enviroBoard[y, x] > 2)
                    {
                        Debug.Log("Flooding checking for building at Tile " + x.ToString() + ", " + y.ToString());
                        Building toDemo = building.FindBuildingByTile(x, y);
                        building.DestroyBuilding(toDemo);
                    }

                    checkDepth++;
                }
            } else
            {
                flooding = false;
            }
        }
    }
}
