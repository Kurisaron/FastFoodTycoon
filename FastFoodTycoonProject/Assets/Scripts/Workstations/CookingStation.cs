using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookingType
{
    Patty,
    Fries,
    Drink
}

public class CookingStation : MonoBehaviour
{
    // VARIABLES
    private WorkStation workStation;
    private CookingType cookingType;

    private List<CookingIngredient> cookingIngredients = new List<CookingIngredient>();

    // FUNCTIONS
    public void PrepCooking(WorkStation workStation, CookingType ck)
    {
        this.workStation = workStation;
        cookingType = ck;

    }

    public void TryCookingIngredient()
    {
        Ingredient ingredientToCook, goalIngredient;
        switch (cookingType)
        {
            case CookingType.Patty:
                ingredientToCook = Ingredient.RawPatty;
                goalIngredient = Ingredient.CookedPatty;
                break;
            case CookingType.Fries:
                ingredientToCook = Ingredient.RawFries;
                goalIngredient = Ingredient.CookedFries;
                break;
            case CookingType.Drink:
                ingredientToCook = Ingredient.Soda;
                goalIngredient = Ingredient.CompleteDrink;
                break;
            default:
                Debug.Log("Cooking type invalid");
                return;
        }

        if (cookingIngredients.Count < GetCookingCapacity() && workStation.WithdrawIngredient(ingredientToCook, 1))
        {
            CookingIngredient newIngredient = new CookingIngredient(ingredientToCook, goalIngredient);
            cookingIngredients.Add(newIngredient);
            StartCoroutine(CookRoutine(newIngredient));
        }
    }

    private int GetCookingCapacity()
    {
        // If you are looking at this, look up "switch as expression c#" before re-using it
        return cookingType switch
        {
            CookingType.Patty => 8,
            CookingType.Fries => 4,
            CookingType.Drink => 1,
            _ => 0,
        };
    }

    public IEnumerator CookRoutine(CookingIngredient cookingIngredient)
    {
        float interval = 0.1f;

        while (cookingIngredient.cookingLife <= 1.0f)
        {
            
            
            cookingIngredient.cookingLife += interval;
            yield return new WaitForSeconds(interval);
        }


    }

    // CLASSES
    public class CookingIngredient
    {
        private Ingredient startingIngredient;
        private Ingredient targetIngredient;

        public float cookingLife;
        public int steps;

        public CookingIngredient(Ingredient sI, Ingredient tI)
        {
            startingIngredient = sI;
            targetIngredient = tI;

            cookingLife = 0.0f;
            steps = 2;
        }
    }
}
