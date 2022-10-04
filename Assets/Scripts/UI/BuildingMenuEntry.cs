using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuEntry : MonoBehaviour
{
    BuildingMenu parentMenu;
    public Building targetPrefab;

    public Button button;

    public GameObject effectTextHolder;
    public GameObject effectResourceHolder;
    public Image[] bonusImages;

    public TMPro.TextMeshProUGUI buildingName;
    public Image buildingImage;

    public Image bkgImage;
    public Image[] costImages;

    public Color selectedColour;
    public Color deselectedColour;

    public void PopulateEntry(Building building, BuildingMenu parent)
    {
        targetPrefab = building;
        parentMenu = parent;

        button.onClick.RemoveAllListeners();

        buildingName.text = building.buildingName;
        buildingImage.sprite = building.buildingSprite;

        foreach(Image i in costImages)
        {
            i.gameObject.SetActive(false);
        }

        if(building is ProductionBuilding)
        {
            effectTextHolder.gameObject.SetActive(false);
            effectResourceHolder.gameObject.SetActive(true);
            ProductionBuilding p = building as ProductionBuilding;

            foreach(Image i in bonusImages)
            {
                i.gameObject.SetActive(false);
            }

            for (int x = 0; x < p.outputs.Count; x++)
            {
                BuildingManager.Resources resource = p.outputs[x];
                if (bonusImages.Length > x)
                {
                    bonusImages[x].gameObject.SetActive(true);
                    bonusImages[x].sprite = GameManager.gameManager.buildings.resourceIcons[GameManager.gameManager.buildings.resourceLookup.IndexOf(resource)];
                }
            }

        } else if (building is WaterBlockBuilding)
        {
            effectTextHolder.gameObject.SetActive(true);
            effectResourceHolder.gameObject.SetActive(false);
        } else
        {
            effectTextHolder.gameObject.SetActive(false);
            effectResourceHolder.gameObject.SetActive(false);
        }

        for(int x = 0; x < building.buildCost.Count; x++)
        {
            BuildingManager.Resources resource = building.buildCost[x];
            if(costImages.Length > x)
            {
                costImages[x].gameObject.SetActive(true);
                costImages[x].sprite = GameManager.gameManager.buildings.resourceIcons[GameManager.gameManager.buildings.resourceLookup.IndexOf(resource)];
            }
        }

        button.onClick.AddListener(delegate { parent.BuildingSelected(this); });
    }

    public void SetSelected()
    {
        bkgImage.color = selectedColour;
    }

    public void SetDeselected()
    {
        bkgImage.color = deselectedColour;
    }
}
