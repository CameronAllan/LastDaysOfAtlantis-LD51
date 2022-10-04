using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour//, IPointerClickHandler
{
    public BoardManager board;
    public BuildingManager building;

    public Building selectedBuildingType;

    public Transform buildingPreview;
    Building previewPrefab;
    SpriteRenderer previewRenderer;
    Vector3Int previewCell;

    public Color invalidColor;
    public Color validColor;


    void Update()
    {
        if(selectedBuildingType != null)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int gridCell = board.enviroTiles.WorldToCell(new Vector3(worldPos.x, worldPos.y));

            //Debug.Log(worldPos.ToString());
            if(gridCell.x < board.centreIndex)
            {
                if (previewPrefab.facingRight)
                {
                    previewPrefab.Flip();
                }
            } else if (gridCell.x > board.centreIndex)
            {
                if (!previewPrefab.facingRight)
                {
                    previewPrefab.Flip();
                }
            }


            if(previewCell == null)
            {
                buildingPreview.position = new Vector3(worldPos.x, worldPos.y, 0);
                previewRenderer.color = invalidColor;
            }
            else if(gridCell != previewCell)
            {
                //Snapping
                if (previewPrefab.ValidLocation(gridCell, board))
                {
                    buildingPreview.position = board.enviroTiles.GetCellCenterWorld(gridCell);
                    previewCell = gridCell;

                    if (building.HasResources(previewPrefab))
                    {
                        previewRenderer.color = validColor;
                    } else
                    {
                        previewRenderer.color = invalidColor;
                    }
                    

                    //Debug.Log("Found Valid Building location at Cell " + gridCell.ToString());
                }
                else
                {
                    previewCell = gridCell;
                    buildingPreview.position = new Vector3(worldPos.x, worldPos.y, 0);
                    previewRenderer.color = invalidColor;

                    //Debug.Log("Invalid Building location at Cell " + gridCell.ToString());
                } 
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            DeselectBuilding();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Attempting Build");
            if (selectedBuildingType != null)
            {
                if (previewPrefab.ValidLocation(previewCell, board) && building.HasResources(previewPrefab))
                {
                    Debug.Log("Attempting Build: Location Valid");
                    building.BuildBuilding(selectedBuildingType, previewCell);
                }
            }
        }
    }

    /*
    public void OnPointerClick(PointerEventData data)
    {
        if(data.button == PointerEventData.InputButton.Right)
        {
            DeselectBuilding();
        }

        if(data.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Attempting Build");
            if(selectedBuildingType != null)
            {
                if(previewPrefab.ValidLocation(previewCell, board))
                {
                    Debug.Log("Attempting Build: Location Valid");
                    building.BuildBuilding(selectedBuildingType, previewCell);
                }
            }
        }
    }*/

    public void SelectBuilding(Building building)
    {
        if(selectedBuildingType != null)
        {
            DeselectBuilding();
        }

        selectedBuildingType = building;

        buildingPreview = Instantiate(building).transform;
        Building newBuilding = buildingPreview.GetComponent<Building>();
        if(newBuilding != null)
        {
            previewPrefab = newBuilding;
            previewRenderer = newBuilding.buildingMainRenderer;

            previewRenderer.color = invalidColor;
        }
    }

    public void DeselectBuilding()
    {
        selectedBuildingType = null;

        Destroy(buildingPreview.gameObject);
        buildingPreview = null;
        previewRenderer = null;
    }

    public void PlaceBuilding()
    {

    }
}
