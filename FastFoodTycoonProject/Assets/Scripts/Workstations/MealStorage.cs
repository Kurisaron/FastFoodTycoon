using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealStorage : MonoBehaviour
{
    // old
    //public List<Meal> meals;

    // new
    public Dictionary<Ingredient, int> food;



    private void Awake()
    {
        //meals = new List<Meal>();

        food = new Dictionary<Ingredient, int>();
        foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
        {
            if (ingredient.ToString().Contains("Complete"))
                food.Add(ingredient, 0);
        }

        
    }

    
    public void AddMeal(Meal meal)
    {
        Debug.Log("Meal added to storage.");
        
        //meals.Add(meal);

        foreach(Ingredient ingredient in meal.food.Keys)
        {
            food[ingredient] += meal.food[ingredient];
            Debug.Log(ingredient.ToString() + " count in meal storage is " + food[ingredient].ToString());
        }
    }

    /*
    public void SubtractMeal (Meal meal)
    {
        Debug.Log("Meal added to storage.");

        meals.Remove(meal);
    }
    */

    /*
    public void ReferenceFunction()
    {

        bool flag = true;
        foreach (Ingredient ingredient1 in order.food.Keys)
        {
            if (food[ingredient1] < order.food[ingredient1])
            {
                flag = false;
            }
        }

        if (flag)
        {
            // Pass food
            foreach (Ingredient ingredient1 in order.food.Keys)
            {
                food[ingredient1] -= order.food[ingredient1];
            }
        }

        
        if (!FindObjectOfType<CustomerController>())
        {
            // Spawn customer
        }

    
        if (FindObjectsOfType<CustomerController>().Length < 8)
        {

        }
    }
    */

}
