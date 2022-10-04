using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    public TextMeshProUGUI villagersText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI glassText;

    int currentVillagersCount = 0;
    int currentFoodCount = 0;
    int currentWoodCount = 0;
    int currentStoneCount = 0;
    int currentGlassCount = 0;

    public ResourceNotification villagerNoti;
    public ResourceNotification foodNoti;
    public ResourceNotification woodNoti;
    public ResourceNotification stoneNoti;
    public ResourceNotification glassNoti;


    public void UpdateResourceTotals(Dictionary<BuildingManager.Resources, int> resources)
    {

        foreach (KeyValuePair<BuildingManager.Resources, int> kvp in resources)
        {
            int change = 0;
            switch (kvp.Key)
            {
                case BuildingManager.Resources.Villagers:
                    change = kvp.Value - currentVillagersCount;
                    if(change != 0)
                    {
                        villagerNoti.TriggerNotification(kvp.Key, change);
                    }

                    currentVillagersCount = kvp.Value;
                    villagersText.text = kvp.Value.ToString();
                    break;
                case BuildingManager.Resources.Food:
                    change = kvp.Value - currentFoodCount;
                    if(change != 0)
                    {
                        foodNoti.TriggerNotification(kvp.Key, change);
                    }

                    currentFoodCount = kvp.Value;
                    foodText.text = kvp.Value.ToString();
                    break;
                case BuildingManager.Resources.Wood:
                    change = kvp.Value - currentWoodCount;
                    if(change != 0)
                    {
                        woodNoti.TriggerNotification(kvp.Key, change);
                    }

                    currentWoodCount = kvp.Value;
                    woodText.text = kvp.Value.ToString();
                    break;
                case BuildingManager.Resources.Stone:
                    change = kvp.Value - currentStoneCount;
                    if(change != 0)
                    {
                        stoneNoti.TriggerNotification(kvp.Key, change);
                    }

                    currentStoneCount = kvp.Value;
                    stoneText.text = kvp.Value.ToString();
                    break;
                case BuildingManager.Resources.Glass:
                    change = kvp.Value - currentGlassCount;
                    if(change != 0)
                    {
                        glassNoti.TriggerNotification(kvp.Key, change);
                    }

                    currentGlassCount = kvp.Value;
                    glassText.text = kvp.Value.ToString();
                    break;
            }
        }
    }
}
