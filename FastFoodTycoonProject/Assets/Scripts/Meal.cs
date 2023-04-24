using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meal
{
    public Dictionary<Ingredient, int> food;

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

    public bool Contains(Ingredient ingredient, int amount)
    {
        return food.ContainsKey(ingredient) && food[ingredient] >= amount;
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

    public GameObject GetItemDisplay()
    {
        GameObject itemDisplay = new GameObject("No Food");
        foreach (KeyValuePair<Ingredient, int> kvp in food)
        {
            itemDisplay = new GameObject(kvp.Key.ToString());
            itemDisplay.AddComponent<RectTransform>();
            itemDisplay.AddComponent<Image>();
            itemDisplay.GetComponent<Image>().sprite = Resources.Load<Sprite>("/2D Assets/Menu Food Sprites/" + kvp.Key.ToString());
        }      
        return itemDisplay;
    }
}
