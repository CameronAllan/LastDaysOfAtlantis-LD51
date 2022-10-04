using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBuilding : Building
{
    public AudioSource audioSource;
    public AudioClip demoClip;

    public override void OnBuilt()
    {
        
    }

    public override void OnDestroyed()
    {
        audioSource.clip = demoClip;
        audioSource.Play();
        //Lose the game!
        GameManager.gameManager.EndGame(false);
    }
}
