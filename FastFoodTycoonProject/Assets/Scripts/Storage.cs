using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredient
{
    TopBun,
    BottomBun,
    Patty,
    CookedPatty,
    Lettuce,
    Fries,
    CookedFries,
    Soda
}

public enum StorageType
{
    Fridge,
    FlatTop,
    Fryer
}

public class Storage : MonoBehaviour
{
    // VARIABLES
    public static Storage fridge;
    public static Storage flatTop;
    public static Storage fryer;

    public StorageType storageType;
    [HideInInspector]
    public Dictionary<Ingredient, int> ingredients = new Dictionary<Ingredient, int>();
    
    // FUNCTIONS
    private void Awake()
    {
        // Set the static variable and ingredients that this storage can hold
        SetStorage();
    }

    private void SetStorage()
    {
        ingredients = new Dictionary<Ingredient, int>();

        switch (storageType)
        {
            case StorageType.Fridge:
                fridge = this;
                // Can store all non-cooked ingredients
                foreach (Ingredient ingredient in (Ingredient[]) Enum.GetValues(typeof(Ingredient)))
                {
                    if (!ingredient.ToString().Contains("Cooked"))
                    {
                        ingredients.Add(ingredient, 0);
                    }
                    
                }
                break;
            case StorageType.FlatTop:
                flatTop = this;
                // Can store patties (cooked or not)
                foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
                {
                    if (ingredient.ToString().Contains("Patty"))
                    {
                        ingredients.Add(ingredient, 0);
                    }

                }
                break;
            case StorageType.Fryer:
                fryer = this;
                // Can store fries (cooked or not)
                foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
                {
                    if (ingredient.ToString().Contains("Fries"))
                    {
                        ingredients.Add(ingredient, 0);
                    }

                }
                break;
            default:
                Debug.Log("Storage type invalid, ingredients not set.");
                break;
        }
    }

    public bool DepositIngredient(Ingredient ingredient, int amount)
    {
        // Cannot deposit ingredient if this storage cannot accept it
        if (!ingredients.ContainsKey(ingredient)) return false;

        // Add ingredients if storage can accept it
        ingredients[ingredient] += amount;
        return true;
    }

    public bool WithdrawIngredient(Ingredient ingredient, int amount)
    {
        // Cannot withdraw ingredient if this storage cannot contain it or does not have the amount required
        if (!ingredients.ContainsKey(ingredient) || ingredients[ingredient] < amount) return false;

        // Deduct ingredients
        ingredients[ingredient] -= amount;
        return true;
    }
}
