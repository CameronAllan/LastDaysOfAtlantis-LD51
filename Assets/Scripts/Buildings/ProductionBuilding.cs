using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    public AudioSource audioSource;
    public AudioClip builtClip;
    public AudioClip demoClip;

    public List<BuildingManager.Resources> inputs = new List<BuildingManager.Resources>();

    public List<BuildingManager.Resources> outputs = new List<BuildingManager.Resources>();

    public override void OnBuilt()
    {
        audioSource.clip = builtClip;
        audioSource.Play();

        base.OnBuilt();
    }

    public override void OnDestroyed()
    {
        base.OnDestroyed();

        audioSource.clip = demoClip;
        audioSource.Play();
    }

    public override bool ValidLocation(Vector3Int gridCoord, BoardManager board)
    {
        return base.ValidLocation(gridCoord, board);
    }
}
