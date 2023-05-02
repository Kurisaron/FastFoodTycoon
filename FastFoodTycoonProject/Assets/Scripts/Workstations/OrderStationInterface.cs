using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderStationInterface : WorkStationInterface
{
    // VARIABLES
    public Text burgerCounterText;
    int burgerCounter = 1;
    public Text fryCounterText;
    int fryCounter = 1;
    public Text drinkCounterText;
    int drinkCounter = 1;
    //private List<Order> customerOrders = new List<Order>();
    
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
    
    void Update()
    {
        //burgerCounterText.text = "x" +
        //fryCounterText.text = "x" +
        //drinkCounterText.text = "x" +
    }
}
