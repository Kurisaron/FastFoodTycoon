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

    public CookingIngredient[] cookingIngredients;

    public Action<CookingIngredient> startCooking;
    public Action<CookingIngredient> stillCooking;
    public Action<CookingIngredient> endCooking;

    // FUNCTIONS
    public void PrepCooking(WorkStation workStation, CookingType ck)
    {
        this.workStation = workStation;
        cookingType = ck;
        cookingIngredients = new CookingIngredient[cookingType switch
        {
            CookingType.Patty => 8,
            CookingType.Fries => 8,
            CookingType.Drink => 1,
            _ => 0,
        }];
    }

    public bool TryCookingIngredient(out int ingredientIndex)
    {
        Tuple<Ingredient, Ingredient> cookingSequence = GetCookingIngredientTypes();
        
        if (Array.Exists(cookingIngredients, ingredient => ingredient == null) && workStation.WithdrawIngredient(cookingSequence.Item1, 1))
        {
            CookingIngredient newIngredient = new CookingIngredient(cookingSequence.Item1, cookingSequence.Item2, this);

            ingredientIndex = Array.FindIndex(cookingIngredients, ingredient => ingredient == null);
            cookingIngredients[ingredientIndex] = newIngredient;

            StartCoroutine(CookRoutine(newIngredient));
            return true;
        }

        ingredientIndex = 0;
        return false;
    }

    public void CookingIngredient_NextStep(CookingIngredient cookingIngredient)
    {
        StartCoroutine(CookRoutine(cookingIngredient));
    }

    public void PassIngredient(CookingIngredient cookingIngredient)
    {
        WorkStation targetStation;
        if (cookingIngredient.targetIngredient == Ingredient.CookedPatty)
        {
            targetStation = WorkStation.burgerAssembly;
        }
        else
        {
            targetStation = WorkStation.orderBuilding;
        }

        if (targetStation.DepositIngredient(cookingIngredient.targetIngredient, 1))
        {
            Debug.Log("Cooking Station successfully passed " + cookingIngredient.targetIngredient.ToString() + " to " + targetStation.storageType.ToString());

            cookingIngredients[Array.IndexOf(cookingIngredients, cookingIngredient)] = null;
        }

    }

    private Tuple<Ingredient, Ingredient> GetCookingIngredientTypes()
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
                goalIngredient = Ingredient.CompleteFries;
                break;
            case CookingType.Drink:
                ingredientToCook = Ingredient.Soda;
                goalIngredient = Ingredient.CompleteDrink;
                break;
            default:
                Debug.Log("Cooking type invalid");
                cookingType = CookingType.Patty;
                return GetCookingIngredientTypes();
        }

        return new Tuple<Ingredient, Ingredient>(ingredientToCook, goalIngredient);
    }

    public IEnumerator CookRoutine(CookingIngredient cookingIngredient)
    {
        cookingIngredient.isCooking = true;
        cookingIngredient.stepTime = 0.0f;
        Debug.Log("Ingredient is cooking");

        // Initiate UI
        if (startCooking != null) startCooking(cookingIngredient);
        
        while (cookingIngredient.stepTime < 2.0f)
        {
            // Update UI
            if (stillCooking != null) stillCooking(cookingIngredient);

            cookingIngredient.stepTime += Time.deltaTime;
            yield return null;
        }

        // Clear timer UI
        if (endCooking != null) endCooking(cookingIngredient);

        cookingIngredient.StepComplete();
    }
    
    public void DisplayIngredientStatus(bool isComplete)
    {

    }

    // CLASSES
    public class CookingIngredient
    {
        private CookingStation cookingStation;
        
        private Ingredient startingIngredient;
        public Ingredient targetIngredient;

        public bool isCooking;

        public float stepTime;
        public int stepsComplete;
        public int steps;

        public CookingIngredient(Ingredient sI, Ingredient tI, CookingStation cS)
        {
            cookingStation = cS;
            
            startingIngredient = sI;
            targetIngredient = tI;

            stepTime = 0.0f;
            stepsComplete = 0;
            steps = 2;
        }

        public void StepComplete()
        {
            isCooking = false;
            stepsComplete += 1;

            // Display UI based on whether the ingredient is complete
            if (stepsComplete >= steps)
            {
                cookingStation.DisplayIngredientStatus(true);
            }
            else
            {
                cookingStation.DisplayIngredientStatus(false);
            }

            Debug.Log("Ingredient is done cooking step");
        }
    }
}
