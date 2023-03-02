using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealStorage : MonoBehaviour
{
    private List<Meal> meals;

    private void Awake()
    {
        meals = new List<Meal>();
    }

    public void AddMeal(Meal meal)
    {
        Debug.Log("Meal added to storage.");
        
        meals.Add(meal);
    }

}
