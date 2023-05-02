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
    public static WorkStation fridge;
    [HideInInspector]
    public static WorkStation flatTop;
    [HideInInspector]
    public static WorkStation fryer;
    [HideInInspector]
    public static WorkStation burgerAssembly;
    [HideInInspector]
    public static WorkStation orderBuilding;
    [HideInInspector]
    public static WorkStation orderStation;

    //WorkStation.orderStation.gameObject.GetComponent<MealStorage>()

    // Workstation Storage
    public StorageType storageType;
    [HideInInspector]
    public Dictionary<Ingredient, int> ingredients = new Dictionary<Ingredient, int>();

    public static int fridgeMaxStorage = 200;

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
                Debug.LogError("Storage type for workstation " + gameObject.name + " not valid");
                break;
            case StorageType.Fridge:
                // Set appropriate static "singleton"
                if (fridge == null)
                {
                    fridge = this;

                    // Can store all non-cooked ingredients
                    foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
                    {
                        if (!ingredient.ToString().Contains("Cooked") && !ingredient.ToString().Contains("Complete"))
                        {
                            //ingredients.Add(ingredient, 0);
                            ingredients.Add(ingredient, UnityEngine.Random.Range(1,11));
                        }

                    }

                }
                break;
            case StorageType.FlatTop:
                flatTop = this;
                // Can store patties (cooked or not)
                ingredients.Add(Ingredient.RawPatty, 5);
                ingredients.Add(Ingredient.CookedPatty, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Patty);
                break;
            case StorageType.Fryer:
                // Set appropriate static "singleton"
                fryer = this;
                // Can store raw or cooked fries
                ingredients.Add(Ingredient.RawFries, 5);
                ingredients.Add(Ingredient.CompleteFries, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Fries);
                break;
            case StorageType.DrinkStation:
                // Can store soda or complete drink
                ingredients.Add(Ingredient.Soda, 5);
                ingredients.Add(Ingredient.CompleteDrink, 0);
                gameObject.AddComponent<CookingStation>().PrepCooking(this, CookingType.Drink);
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
                if (orderBuilding == null)
                {
                    orderBuilding = this;
                    // Can store complete food items
                    ingredients.Add(Ingredient.CompleteBurger, 0);
                    ingredients.Add(Ingredient.CompleteFries, 0);
                    ingredients.Add(Ingredient.CompleteDrink, 0);

                }
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
        if (!ingredients.ContainsKey(ingredient))
        {
            Debug.LogError("Ingredient " + ingredient.ToString() + " cannot be deposited in workstation " + typeof(WorkStation).Name);
            return false;
        }

        if (storageType == StorageType.Fridge && GetStorageTotal() >= fridgeMaxStorage)
        {
            Debug.LogWarning("Ingredient " + ingredient.ToString() + " cannot be deposited in the fridge because it is too full");
            return false;
        }

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

    public int GetStorageTotal()
    {
        int total = 0;
        foreach(Ingredient ingredient in ingredients.Keys)
        {
            total += ingredients[ingredient];
        }
        return total;
    }

    public static int FridgeStorageEmptySpaces()
    {
        return fridgeMaxStorage - fridge.GetStorageTotal();
    }
}
