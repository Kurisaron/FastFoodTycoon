using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Ingredient
{
    TopBun,
    BottomBun,
    RawPatty,
    CookedPatty,
    Lettuce,
    RawFries,
    CookedFries,
    Soda,
    CompleteBurger,
    CompleteFries,
    CompleteDrink
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

    [HideInInspector]
    public static WorkStation fridge;
    [HideInInspector]
    public static WorkStation flatTop;
    [HideInInspector]
    public static WorkStation fryer;

    // Workstation Storage
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
                    if (!ingredient.ToString().Contains("Cooked") && !ingredient.ToString().Contains("Complete"))
                    {
                        ingredients.Add(ingredient, 0);
                    }

                }
                break;
            case StorageType.FlatTop:
                flatTop = this;
                // Can store patties (cooked or not)
                ingredients.Add(Ingredient.RawPatty, 0);
                ingredients.Add(Ingredient.CookedPatty, 0);
                break;
            case StorageType.Fryer:
                fryer = this;
                // Can store raw or cooked fries
                ingredients.Add(Ingredient.RawFries, 0);
                ingredients.Add(Ingredient.CookedFries, 0);
                break;
            case StorageType.FryWarmer:
                // Can store cooked or complete fries
                ingredients.Add(Ingredient.CookedFries, 0);
                ingredients.Add(Ingredient.CompleteFries, 0);
                break;
            case StorageType.BurgerAssembly:
                // Can store burger ingredients or complete burgers
                ingredients.Add(Ingredient.CookedPatty, 0);
                ingredients.Add(Ingredient.TopBun, 0);
                ingredients.Add(Ingredient.BottomBun, 0);
                ingredients.Add(Ingredient.Lettuce, 0);
                ingredients.Add(Ingredient.CompleteBurger, 0);
                break;
            case StorageType.OrderStation:
                break;
            case StorageType.OrderBuilding:
                // Can store complete food items
                ingredients.Add(Ingredient.CompleteBurger, 0);
                ingredients.Add(Ingredient.CompleteFries, 0);
                ingredients.Add(Ingredient.CompleteDrink, 0);
                break;
            case StorageType.DrinkStation:
                // Can store soda or complete drink
                ingredients.Add(Ingredient.Soda, 0);
                ingredients.Add(Ingredient.CompleteDrink, 0);
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
