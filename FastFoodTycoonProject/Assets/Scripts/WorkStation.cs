using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    None,
    Fridge,
    FlatTop,
    Fryer,
    FryWarmer,
    BurgerAssembly,
    OrderStation,
    OrderBuilding,
    DrinkStation,
    Other
}

public class WorkStation : MonoBehaviour
{
    // VARIABLES
    [Tooltip("The point that workers stand at to work at the station (Pathfinding)")]
    public Transform goToPoint;

    public static WorkStation fridge;
    public static WorkStation flatTop;
    public static WorkStation fryer;

    public StorageType storageType;
    [HideInInspector]
    public Dictionary<Ingredient, int> ingredients = new Dictionary<Ingredient, int>();

    // FUNCTIONS
    private void Awake()
    {
        SetStorage();
    }

    public void LoadStationScene()
    {
        SceneManager.LoadScene(gameObject.name + "Scene", LoadSceneMode.Additive);
    }

    public void UnloadStationScene()
    {
        SceneManager.UnloadSceneAsync(gameObject.name + "Scene");
    }

    private void SetStorage()
    {
        ingredients = new Dictionary<Ingredient, int>();

        switch (storageType)
        {
            case StorageType.None:
                break;
            case StorageType.Fridge:
                fridge = this;
                // Can store all non-cooked ingredients
                foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
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
            case StorageType.FryWarmer:
                break;
            case StorageType.BurgerAssembly:
                break;
            case StorageType.OrderStation:
                break;
            case StorageType.OrderBuilding:
                break;
            case StorageType.DrinkStation:
                break;
            case StorageType.Other:
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
