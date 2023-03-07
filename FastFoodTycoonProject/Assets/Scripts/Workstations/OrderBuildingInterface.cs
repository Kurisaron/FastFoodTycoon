using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBuildingInterface : WorkStationInterface
{
    // VARIABLES
    public List<BuildingSpawnPoint> spawnPoints;

    public GameObject orderTrayPrefab;
    public GameObject burgerPrefab;
    public GameObject fryTrayPrefab;
    public GameObject drinkPrefab;

    private GameObject orderTray;
    private Meal currentMeal;
    private bool isAssembling;

    [SerializeField]
    private Text orderText;

    // UNITY FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

        NewMeal();
    }

    private void Update()
    {
        orderText.text = GetOrderText();
    }

    // MEAL FUNCTIONS
    public void NewMeal()
    {
        orderTray = Instantiate(orderTrayPrefab, transform);
        orderTray.transform.parent = null;
        orderTray.transform.position = spawnPoints.Find(point => point.id == "Order Tray").point.position;

        currentMeal = new Meal();
        isAssembling = true;
    }


    // COMPLETE MEAL EVENTS
    public void CompleteMealButtonEvent()
    {
        if (isAssembling && currentMeal.HasFood())
        {
            CompleteMeal();
        }
        else
        {
            NoMeal();
        }
    }

    private void CompleteMeal()
    {
        isAssembling = false;
        WorkStation.orderStation.gameObject.GetComponent<MealStorage>().AddMeal(currentMeal);
        Destroy(orderTray);
        NewMeal();
    }

    private void NoMeal()
    {
        Debug.Log("Cannot complete a meal without a tray and/or food");
    }

    // BURGER BUTTON EVENTS
    public void BurgerButtonEvent()
    {
        if (isAssembling)
        {
            AddBurger();
        }
        else
        {
            NoBurger();
        }
    }

    private void AddBurger()
    {
        if (!currentMeal.Contains(Ingredient.CompleteBurger))
        {
            GameObject burger = Instantiate(burgerPrefab, transform);
            burger.transform.parent = null;
            burger.transform.position = spawnPoints.Find(point => point.id == "Burger").point.position;
            burger.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            burger.transform.parent = orderTray.transform;
        }

        currentMeal.AddFood(Ingredient.CompleteBurger, 1);
    }

    private void NoBurger()
    {
        Debug.Log("Cannot add a burger without a tray!");
    }

    // FRIES BUTTON EVENTS
    public void FriesButtonEvent()
    {
        if (isAssembling)
        {
            AddBurger();
        }
        else
        {
            NoBurger();
        }
    }

    private void AddFries()
    {
        if (!currentMeal.Contains(Ingredient.CompleteFries))
        {
            GameObject fries = Instantiate(fryTrayPrefab, transform);
            fries.transform.parent = null;
            fries.transform.position = spawnPoints.Find(point => point.id == "Fries").point.position;
            fries.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            fries.transform.parent = orderTray.transform;
        }

        currentMeal.AddFood(Ingredient.CompleteBurger, 1);
    }

    private void NoFries()
    {
        Debug.Log("Cannot add fries without a tray!");
    }

    // DRINK BUTTON EVENTS
    public void DrinkButtonEvent()
    {
        if (isAssembling)
        {
            AddBurger();
        }
        else
        {
            NoBurger();
        }
    }

    private void AddDrink()
    {
        if (!currentMeal.Contains(Ingredient.CompleteDrink))
        {
            GameObject drink = Instantiate(drinkPrefab, transform);
            drink.transform.parent = null;
            drink.transform.position = spawnPoints.Find(point => point.id == "Drink").point.position;
            drink.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            drink.transform.parent = orderTray.transform;
        }

        currentMeal.AddFood(Ingredient.CompleteBurger, 1);
    }

    private void NoDrink()
    {
        Debug.Log("Cannot add a drink without a tray!");
    }

    // MISC FUNCTIONS
    private string GetOrderText()
    {
        if (!isAssembling || currentMeal == null || !currentMeal.HasFood())
            return "No food in order";

        return currentMeal.GetOrderText();
    }

    // CLASSES
    [Serializable]
    public class BuildingSpawnPoint
    {
        public string id;
        public Transform point;
    }
}
