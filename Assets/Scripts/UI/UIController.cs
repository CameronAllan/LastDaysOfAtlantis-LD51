using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public CameraController cam;

    public BuildingMenu buildingMenu;
    public ResourcesUI resourceUI;
    public Toggle hardMode;

    public GameObject mainMenuCanvas;

    public GameObject endMenu;
    public TMPro.TextMeshProUGUI endMenuText;

    public Image normalSpeedHighlight;
    public Image fastForwardHighlight;

    public GameObject mainMenuButtons;

    public void StartGame()
    {
        mainMenuButtons.SetActive(false);

        if (hardMode.isOn)
        {
            GameManager.gameManager.startingWaterHeight = 55;
        } else
        {
            GameManager.gameManager.startingWaterHeight = 40;
        }

        NormalSpeed();

        GameManager.gameManager.Initialize();

        HideMainMenu();
    }

    public void NormalSpeed()
    {
        fastForwardHighlight.color = Color.clear;
        normalSpeedHighlight.color = Color.white;
        GameManager.gameManager.tickSeconds = 10f;
    }

    public void FastForward()
    {
        fastForwardHighlight.color = Color.white;
        normalSpeedHighlight.color = Color.clear;
        GameManager.gameManager.tickSeconds = 1f;
    }

    public void Initialize()
    {
        buildingMenu.SetupEntries();
    }

    void HideMainMenu()
    {
        mainMenuCanvas.SetActive(false);
    }

    void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
    }

    public void ShowEndMenu(bool win)
    {
        if (win)
        {
            endMenuText.text = "Atlantis is Saved!";
        } else
        {
            endMenuText.text = "Atlantis is Lost!";
        }

        endMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
}
