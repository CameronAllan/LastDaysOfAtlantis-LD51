using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    public Player humanPlayer;

    public List<BuildingMenuEntry> entries;

    public void SetupEntries()
    {
        foreach(BuildingMenuEntry e in entries)
        {
            e.gameObject.SetActive(false);
        }


        for(int x = 0; x < GameManager.gameManager.buildings.buildingPrefabs.Count; x++)
        {
            if(x < entries.Count)
            {
                entries[x].gameObject.SetActive(true);
                entries[x].PopulateEntry(GameManager.gameManager.buildings.buildingPrefabs[x], this);
            }
        }
    }

    public void BuildingSelected(BuildingMenuEntry entry)
    {
        foreach(BuildingMenuEntry e in entries)
        {
            e.SetDeselected();
        }

        entry.SetSelected();

        humanPlayer.SelectBuilding(entry.targetPrefab);
    }
}
