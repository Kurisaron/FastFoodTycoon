using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStationInterface : WorkStationInterface
{
    // VARIABLES
    
    // PROPERTIES
    private MealStorage StationMealStorage
    {
        get
        {
            return workStation.gameObject.GetComponent<MealStorage>();
        }
    }
    
    // FUNCTIONS
    protected override void Awake()
    {
        base.Awake();


    }
}
