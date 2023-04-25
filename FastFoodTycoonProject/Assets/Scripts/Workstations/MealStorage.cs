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
        }
    }

    /*
    public void SubtractMeal (Meal meal)
    {
        Debug.Log("Meal added to storage.");

        meals.Remove(meal);
    }
    */

}
