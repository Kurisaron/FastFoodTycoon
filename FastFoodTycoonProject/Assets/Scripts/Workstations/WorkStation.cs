using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StorageType
{
    None,
    Fridge,
    FlatTop,
    Fryer,
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
    public static WorkStation fridge1;
    [HideInInspector]
    public static WorkStation fridge2;
    [HideInInspector]
    public static WorkStation flatTop;
    [HideInInspector]
    public static WorkStation fryer1;
    [HideInInspector]
    public static WorkStation fryer2;
    [HideInInspector]
    public static WorkStation burgerAssembly;
    [HideInInspector]
    public static WorkStation orderBuilding;
    [HideInInspector]
    public static WorkStation orderStation;

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
        GameManager.Instance.stationOpened = true;
        GameManager.Instance.openBuyMenuButton.SetActive(false);
        SceneManager.LoadScene(GetStationSceneName(), LoadSceneMode.Additive);
    }

    public void UnloadStationScene()
    {
        GameManager.Instance.stationOpened = false;
        GameManager.Instance.openBuyMenuButton.SetActive(true);
        PlayerInputEvents.Instance.ResetWorkstationAction();
        SceneManager.UnloadSceneAsync(GetStationSceneName());
    }

    private string GetStationSceneName()
    {
        string sceneName = new String(gameObject.name.Where(Char.IsLetter).ToArray());
        sceneName += "Scene";

        return sceneName;
    }

    private void SetStorage()
    {
        ingredients = new Dictionary<Ingredient, int>();

        switch (storageType)
        {
            case StorageType.None:
                break;
            case StorageType.Fridge:
                // Set appropriate static "singleton"
                if (gameObject.name.Contains("1"))
                {
                    fridge1 = this;
                }
                else
                {
                    fridge2 = this;
                }

                // Can store all non-cooked ingredients
                foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
                {
                    if (!ingredient.ToString().Contains("Cooked") && !ingredient.ToString().Contains("Complete"))
                    {
                        ingredients.Add(ingredient, 0);
                        //ingredients.Add(ingredient, UnityEngine.Random.Range(0,3));
                    }

                }
                break;
            case StorageType.FlatTop:
                flatTop = this;
                // Can store patties (cooked or not)
                ingredients.Add(Ingredient.RawPatty, 0);
                ingredients.Add(Ingredient.CookedPatty, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Patty);
                break;
            case StorageType.Fryer:
                // Set appropriate static "singleton"
                if (gameObject.name.Contains("1"))
                {
                    fryer1 = this;
                }
                else
                {
                    fryer2 = this;
                }

                // Can store raw or cooked fries
                ingredients.Add(Ingredient.RawFries, 0);
                ingredients.Add(Ingredient.CompleteFries, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Fries);
                break;
            case StorageType.BurgerAssembly:
                burgerAssembly = this;
                // Can store burger ingredients or complete burgers
                ingredients.Add(Ingredient.CookedPatty, 0);
                ingredients.Add(Ingredient.Buns, 0);
                ingredients.Add(Ingredient.Lettuce, 0);
                ingredients.Add(Ingredient.CompleteBurger, 0);
                break;
            case StorageType.OrderStation:
                orderStation = this;
                gameObject.AddComponent<MealStorage>();
                break;
            case StorageType.OrderBuilding:
                orderBuilding = this;
                // Can store complete food items
                ingredients.Add(Ingredient.CompleteBurger, 0);
                ingredients.Add(Ingredient.CompleteFries, 0);
                ingredients.Add(Ingredient.CompleteDrink, 0);
                break;
            case StorageType.DrinkStation:
                // Can store soda or complete drink
                ingredients.Add(Ingredient.Soda, 0);
                ingredients.Add(Ingredient.CompleteDrink, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Drink);
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
        Debug.Log(gameObject.name + " " + amount.ToString() + " " + ingredient.ToString() + " deposited");
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

    // COLLECTIVE FRIDGE FUNCTIONS
    public static int CollectiveFridge_GetIngredientCount(Ingredient ingredient)
    {
        if (fridge1 == null || fridge2 == null)
        {
            Debug.LogError("Fridge not set properly");
            return 0;
        }

        return fridge1.ingredients[ingredient] + fridge2.ingredients[ingredient];
    }

    public static bool CollectiveFridge_WithdrawIngredient(Ingredient ingredient, int amount)
    {
        if (!fridge1.ingredients.ContainsKey(ingredient) || !fridge2.ingredients.ContainsKey(ingredient) || CollectiveFridge_GetIngredientCount(ingredient) < amount) return false;

        int amountNeeded = amount;
        int fridgeAmount = amountNeeded > fridge1.ingredients[ingredient] ? fridge1.ingredients[ingredient] : amountNeeded;
        if (fridge1.WithdrawIngredient(ingredient, fridgeAmount))
        {
            Debug.Log("Collective Fridge withdrew " + amount.ToString() + " " + ingredient.ToString() + " from Fridge 1");
        }
        amountNeeded -= fridgeAmount;

        if (amountNeeded == 0) return true;

        if (fridge2.WithdrawIngredient(ingredient, amountNeeded))
        {
            Debug.Log("Collective Fridge withdrew " + amount.ToString() + " " + ingredient.ToString() + " from Fridge 2");
        }

        return true;
    }
}
