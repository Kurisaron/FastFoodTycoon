using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatTopInterface : WorkStationInterface
{
    // VARIABLES
    public Transform[] flatTop_SpawnPoints;

    // PROPERTIES
    private CookingStation FlatTopCooker
    {
        get
        {
            return workStation.gameObject.GetComponent<CookingStation>();
        }
    }
    
    // FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

        PlayerInputEvents.Instance.workstationAction = FlatTopTapEvent;
    }

    private void FlatTopTapEvent(RaycastHit hit)
    {
        if (hit.collider.gameObject.name.Contains("CookSelection"))
        {
            FlatTopCooker.TryCookingIngredient();
        }
    }
}
