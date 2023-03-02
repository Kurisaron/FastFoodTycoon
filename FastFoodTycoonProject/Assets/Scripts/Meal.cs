using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meal
{
    private Dictionary<Ingredient, int> food;

    public Meal()
    {
        food = new Dictionary<Ingredient, int>();
    }

    public void AddFood(Ingredient ingredient, int amount)
    {
        // Add food to the meal
        if(food.ContainsKey(ingredient))
        {
            food[ingredient] += amount;
        }
        else
        {
            food.Add(ingredient, amount);
        }


    }

    public bool Contains(Ingredient ingredient)
    {
        return food.ContainsKey(ingredient) && food[ingredient] > 0;
    }

    public bool HasFood()
    {
        return food != null && food.Count > 0;
    }
    public string GetOrderText()
    {
        string order = "Food in order:\n";
        foreach (KeyValuePair<Ingredient, int> kvp in food)
        {
            order += kvp.Key.ToString() + ": " + kvp.Value.ToString() + "\n";
        }

        return order;
    }
}
