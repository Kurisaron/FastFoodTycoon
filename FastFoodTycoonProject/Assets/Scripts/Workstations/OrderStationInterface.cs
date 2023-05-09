using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderStationInterface : WorkStationInterface
{
    // VARIABLES
    public Text burgerCounterText;
    int burgerCounter = 0;
    public Text fryCounterText;
    int fryCounter = 0;
    public Text drinkCounterText;
    int drinkCounter = 0;
    
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
        burgerCounter = 0;
        fryCounter = 0;
        drinkCounter = 0;

        if (CustomerSpawnOne.Instance == null) Debug.LogError("No instance of CustomerSpawnOne");
        if (CustomerSpawnOne.Instance.customers[0] == null) Debug.LogError("No customer 0");

        foreach (Ingredient ingredient in CustomerSpawnOne.Instance.customers[0].food.Keys)
        {
            Debug.Log("Customer 0 has " + ingredient.ToString() + " in dictionary");
        }
        
        burgerCounterText.text = "x" + CustomerSpawnOne.Instance.customers[0].food[Ingredient.CompleteBurger];
        fryCounterText.text = "x" + CustomerSpawnOne.Instance.customers[0].food[Ingredient.CompleteFries];
        drinkCounterText.text = "x" + CustomerSpawnOne.Instance.customers[0].food[Ingredient.CompleteDrink];
        
    }
    
    void Update()
    {

    }
}