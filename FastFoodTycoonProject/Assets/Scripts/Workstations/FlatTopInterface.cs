using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatTopInterface : WorkStationInterface
{
    // VARIABLES

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

    }

    public void PlacePatty()
    {
        
    }
}
