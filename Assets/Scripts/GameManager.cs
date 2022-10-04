using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public ClockUI clock;

    public int boardWidth = 250;
    public int boardHeight = 100;
    public int startingWaterHeight = 10;

    public BoardManager boards;
    public BuildingManager buildings;
    public UIController ui;

    public bool gameRunning;
    public bool isPaused;

    [Header("Game Speed/Loop Vars")]
    public float tickSeconds;
    public EventHandler tickEvent;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            if (gameManager != this)
            {
                Destroy(this);
            }
        }
    }

    /*private void Start()
    {
        Initialize();
    }*/

    void OnTick()
    {
        buildings.GameTick();
        boards.GameTick();
    }

    public void Initialize()
    {
        boards.SetupGameBoards(boardWidth, boardHeight,startingWaterHeight);
        ui.cam.SetupCameraBounds(boardWidth, boardHeight);
        buildings.SetupGameStart();

        ui.Initialize();

        StartGame();
    }

    void StartGame()
    {
        gameRunning = true;
        AudioManager.instance.GameStart();
        StartCoroutine("tickRoutine");
    }

    public void EndGame(bool win)
    {
        gameRunning = false;
        AudioManager.instance.GameEnd();
        ui.ShowEndMenu(win);
    }

    IEnumerator tickRoutine()
    {
        float timeElapsed = 0f;

        while (gameRunning)
        {
            if (isPaused)
            {
                yield return null;
            } else
            {
                timeElapsed += Time.deltaTime;

                float debugPercent = timeElapsed / tickSeconds * 100f;
                //Debug.Log("Debug Timer % " + debugPercent + "%, " + timeElapsed + " out of " + tickSeconds);
                clock.UpdateClock(timeElapsed, tickSeconds);

                if(timeElapsed >= tickSeconds)
                {
                    //if (tickEvent != null)
                    //    tickEvent.Invoke(this, new EventArgs());
                    //Make this a function call, so we have a very specific order of operations
                    //Buildings consume (if applicable)
                    //Buildings produce
                    //Water rises
                    OnTick();
                    timeElapsed = 0f;
                }

                yield return null;
            }
        }
    }
}
