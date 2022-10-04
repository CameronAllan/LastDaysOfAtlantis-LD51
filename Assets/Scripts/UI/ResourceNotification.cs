using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceNotification : MonoBehaviour
{
    public GameObject notificationHolder;
    public TextMeshProUGUI plusMinus;
    public Image image;
    public TextMeshProUGUI number;

    float notificationShowTime = 1f;
    Coroutine currentNotif = null;

    public void TriggerNotification(BuildingManager.Resources resource, int amount)
    {
        if(currentNotif != null)
        {
            StopCoroutine(currentNotif);
        }

        if(amount > 0)
        {
            plusMinus.text = "+";
            number.text = amount.ToString();
        } else if (amount < 0)
        {
            plusMinus.text = "-";
            number.text = amount.ToString();
        }

        image.sprite = GameManager.gameManager.buildings.resourceIcons[GameManager.gameManager.buildings.resourceLookup.IndexOf(resource)];
        currentNotif = StartCoroutine(ShowNotification());
    }

    IEnumerator ShowNotification()
    {
        notificationHolder.SetActive(true);

        yield return new WaitForSeconds(notificationShowTime);

        notificationHolder.SetActive(false);
        currentNotif = null;
        yield break;
    }
}
