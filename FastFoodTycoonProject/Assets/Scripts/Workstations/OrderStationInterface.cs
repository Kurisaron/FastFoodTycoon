using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStationInterface : WorkStationInterface
{
    // VARIABLES
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

    /*
    public void CheckOrders()
    {
        foreach (Meal meal in StationMealStorage.meals)
        {
            bool flag = true;

            foreach (Ingredient ingredient in meal.food.Keys)
            {
                if (order.food[ingredient] != meal.food[ingredient]) flag = false;
            }

            if (flag)
            {
                // Remove meal from meal storage and give it to the customer
            }
        }
    }
    */
}
