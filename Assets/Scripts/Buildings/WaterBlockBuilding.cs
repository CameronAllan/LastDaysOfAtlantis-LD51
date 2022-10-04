using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlockBuilding : Building
{
    public AudioSource audioSource;
    public AudioClip builtClip;

    public override void OnBuilt()
    {
        audioSource.clip = builtClip;
        audioSource.Play();
    }

    public override void OnDestroyed()
    {
        
    }
}
