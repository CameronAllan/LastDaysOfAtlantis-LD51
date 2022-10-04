using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public BoardManager board;
    public ResourcesUI resources;
    
    
    public enum Resources
    {
        Food,
        Villagers,
        Wood,
        Stone,
        Glass
    }

    public List<Resources> startingResourceTypes;
    public List<int> startingResourceAmounts;

    public List<Resources> resourceLookup;
    public List<Sprite> resourceIcons;

    Dictionary<Resources, int> currentResources = new Dictionary<Resources, int>();

    public Building objectivePrefab;
    public List<Building> buildingPrefabs;

    public List<Building> allBuildings;
    public List<ProductionBuilding> resourceBuildings;

    public void Awake()
    {
        //Setup resource dict
        foreach (int i in Enum.GetValues(typeof(Resources)))
        {
            currentResources.Add((Resources)i, 0);
        }
    }

    public void SetupGameStart()
    {
        for(int x = 0; x < startingResourceTypes.Count; x++)
        {
            if(x < startingResourceAmounts.Count)
            {
                currentResources[startingResourceTypes[x]] += startingResourceAmounts[x];
            }
        }

        resources.UpdateResourceTotals(currentResources);
    }

    public void GameTick()
    {
        //loop through all buildings and deduct their resource costs from total (if applicable)
        foreach(ProductionBuilding b in resourceBuildings)
        {
            /* no time for this one - but in future should make the player have to feed population
            if(b.inputs.Count > 0)
            {
                //check that they're met and deduct -> add to prod
                //else continue;
            } else
            {

                
            }*/
            if (!b.destroyed)
            {
                AddResources(b.outputs);
            }
        }


        resources.UpdateResourceTotals(currentResources);
    }

    public bool HasResources(Building prefab)
    {
        bool canPay = true;
        Dictionary<Resources, int> costDict = new Dictionary<Resources, int>();
        foreach(Resources r in prefab.buildCost)
        {
            if (costDict.ContainsKey(r))
            {
                costDict[r]++;
            } else
            {
                costDict.Add(r, 1);
            }
        }

        foreach(KeyValuePair<Resources, int> kvp in costDict)
        {
            if(currentResources[kvp.Key] < costDict[kvp.Key])
            {
                canPay = false;
                break;
            }
        }

        return canPay;
    }

    public void BuildBuilding(Building prefab, Vector3Int gridCell)
    {
        //Spawn the prefab at the coords
        Building newBuilding = Instantiate(prefab, transform).GetComponent<Building>();

        newBuilding.transform.position = board.enviroTiles.GetCellCenterWorld(gridCell);

        if(newBuilding is ProductionBuilding)
        {
            resourceBuildings.Add(newBuilding as ProductionBuilding);
        }

        allBuildings.Add(newBuilding);

        int xCoord = gridCell.x;
        int yCoord = gridCell.y;

        newBuilding.occupiedSquares = new List<Vector3Int>();
        //Set the enviroBoard and buildingBoard based on the building footprint
        if (gridCell.x > board.centreIndex)//newBuilding.facingRight)
        {
            newBuilding.facingRight = true;
            Debug.Log("Building should be flipped");
            newBuilding.transform.eulerAngles = new Vector3(0f, -180f, 0f);
            for (int y = 0; y < newBuilding.height; y++)
            {
                for (int x = 0; x < newBuilding.width; x++)
                {
                    if (newBuilding.buildingType == 3)
                    {
                        board.enviroBoard[yCoord + y, xCoord + x] = 4;
                    }
                    else
                    {
                        board.enviroBoard[yCoord + y, xCoord + x] = 3;
                    }
                    board.buildingBoard[yCoord + y, xCoord - x] = newBuilding.buildingType;
                    
                    newBuilding.occupiedSquares.Add(new Vector3Int(xCoord - x, yCoord + y, 0));
                }
            }
        } else
        {
            for (int y = 0; y < newBuilding.height; y++)
            {
                for (int x = 0; x < newBuilding.width; x++)
                {
                    if(newBuilding.buildingType == 3)
                    {
                        board.enviroBoard[yCoord + y, xCoord + x] = 4;
                    } else
                    {
                        board.enviroBoard[yCoord + y, xCoord + x] = 3;
                    }
                    
                    board.buildingBoard[yCoord + y, xCoord + x] = newBuilding.buildingType;
                    newBuilding.occupiedSquares.Add(new Vector3Int(xCoord + x, yCoord + y, 0));
                }
            }
        }

        DeductResources(newBuilding.buildCost);
        newBuilding.OnBuilt();
    }

    public Building FindBuildingByTile(int x, int y)
    {
        Vector3Int targetTile = new Vector3Int(x, y, 0);
        Building targetBuilding = allBuildings.Find(b => b.occupiedSquares.Contains(targetTile));

        if(targetBuilding != null)
        {
            Debug.Log("Found Building " + targetBuilding.buildingName);
        }

        return targetBuilding;
    }

    public void DestroyBuilding(Building building)
    {
        Debug.Log("Destroying Building!");


        if(building != null)
        {
            if (!building.destroyed)
            {
                building.OnDestroyed();
            }
        }
    }

    public void DeductResources(List<Resources> cost)
    {
        foreach (Resources r in cost)
        {
            if(currentResources[r] <= 1)
            {
                currentResources[r] = 0;
            } else
            {
                currentResources[r]--;
            }
        }

        resources.UpdateResourceTotals(currentResources);
    }

    public void AddResources(List<Resources> income)
    {
        foreach(Resources r in income)
        {
            currentResources[r]++;
        }

        //resources.UpdateResourceTotals(currentResources);
    }
}
