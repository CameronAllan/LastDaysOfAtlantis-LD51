using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int buildingID;
    public int buildingType;
    //1 - wall
    //2 - regular (stackable)
    //3 - unstackable

    public string buildingName;
    public Sprite buildingSprite;
    public SpriteRenderer buildingMainRenderer;

    public bool facingRight;
    public int width;
    public int height;

    public List<int> targetCellTypes;
    public List<int> backgroundTypes;
    public List<int> foundationTypes;

    public bool advancedFoundation;
    public int[] foundationTiles;

    public List<BuildingManager.Resources> buildCost;

    public List<Vector3Int> occupiedSquares;

    public abstract void OnBuilt();

    public abstract void OnDestroyed();

    public virtual bool ValidLocation(Vector3Int gridCoord, BoardManager board)
    {
        bool goodFoundation = true;
        bool goodBackground = true;

        if(gridCoord == null)
        {
            //Debug.Log("INVALID LOCATION: Null");
            return false;
        }

        //validate the coords
        if(gridCoord.x >= board.arrayWidth || gridCoord.x < 0)
        {
            //Debug.Log("INVALID LOCATION: Out of bounds");
            return false;
        }

        if (gridCoord.y >= board.arrayLength || gridCoord.y < 0)
        {
            //Debug.Log("INVALID LOCATION: Out of bounds");
            return false;
        }
        
        if (!targetCellTypes.Contains(board.enviroBoard[gridCoord.y, gridCoord.x]) || board.waterBoard[gridCoord.y, gridCoord.x] == 1)
        {
            //Debug.Log("INVALID LOCATION: Bad target cell - Type " + board.enviroBoard[gridCoord.y, gridCoord.x]);
            return false;
        }

        int xCoord = gridCoord.x;
        int yCoord = gridCoord.y;

        int yFoundation = yCoord - 1;

        
        if (facingRight)
        {
            for (int x = 0; x < width; x++)
            {
                if ((xCoord - x) >= 0 && (xCoord - x) < board.arrayWidth && yFoundation >= 0 && yFoundation < board.arrayLength)
                {
                    if (!foundationTypes.Contains(board.enviroBoard[yFoundation, xCoord - x]))
                    {
                        //Debug.Log("INVALID LOCATION: Bad foundation cells");
                        goodFoundation = false;
                        break;
                    }
                } else
                {
                    goodFoundation = false;
                    break;
                } 
            }

            if (goodFoundation)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if ((xCoord - x) >= 0 && (xCoord - x) < board.arrayWidth && (yCoord + y) >= 0 && (yCoord + y) < board.arrayLength)
                        {
                            if (!backgroundTypes.Contains(board.enviroBoard[yCoord + y, xCoord - x]))
                            {
                                goodBackground = false;
                                //Debug.Log("INVALID LOCATION: Bad background cells");
                                break;
                            }
                        } else
                        {
                            goodBackground = false;
                            //Debug.Log("INVALID LOCATION: Bad background cells");
                            break;
                        }
                            
                    }
                }
            }
        } else
        {
            for (int x = 0; x < width; x++)
            {
                if ((xCoord + x) >= 0 && (xCoord + x) < board.arrayWidth && yFoundation >= 0 && yFoundation < board.arrayLength)
                {
                    if (!foundationTypes.Contains(board.enviroBoard[yFoundation, xCoord + x]))
                    {
                        //Debug.Log("INVALID LOCATION: Bad foundation cells");
                        goodFoundation = false;
                        break;
                    }
                }
                else
                {
                    goodFoundation = false;
                    break;
                }
                 
            }

            if (goodFoundation)
            {
                for (int y = 0; y < width; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if((xCoord + x) >= 0 && (xCoord + x) < board.arrayWidth && (yCoord + y) >= 0 && (yCoord + y) < board.arrayLength)
                        {
                            if (!backgroundTypes.Contains(board.enviroBoard[yCoord + y, xCoord + x]))
                            {
                                goodBackground = false;
                                Debug.Log("INVALID LOCATION: Bad background cells");
                                break;
                            }
                        } else
                        {
                            goodBackground = false;
                            Debug.Log("INVALID LOCATION: Bad background cells");
                            break;
                        }
                        
                    }
                }
            }
        }
        

        bool valid = goodBackground && goodFoundation;
        return valid;
    }

    public virtual void Flip()
    {
        if (facingRight)
        {
            facingRight = false;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            //buildingMainRenderer.flipX = false;

        } else
        {
            facingRight = true;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            //buildingMainRenderer.flipX = true;
        }
    }
}
