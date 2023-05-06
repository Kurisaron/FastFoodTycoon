using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OrderBuildingInterface : WorkStationInterface
{
    // VARIABLES
    public List<BuildingSpawnPoint> spawnPoints;

    public GameObject orderTrayPrefab;
    public GameObject burgerPrefab;
    public GameObject fryTrayPrefab;
    public GameObject drinkPrefab;
    //public GameObject customerPrefab;

    public Text burgerAmountText;
    public Text friesAmountText;
    public Text drinkAmountText;

    public GameObject burgerLayout;
    public GameObject friesLayout;
    public GameObject drinkLayout;

    private GameObject orderTray;
    private Meal currentMeal;
    private bool isAssembling;

    AudioSource audioData;

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

        audioData = GetComponent<AudioSource>();

        orderText.text = GetOrderText();

        burgerAmountText.text = "x " + WorkStation.orderBuilding.ingredients[Ingredient.CompleteBurger];
        friesAmountText.text = "x " + WorkStation.orderBuilding.ingredients[Ingredient.CompleteFries];
        drinkAmountText.text = "x " + WorkStation.orderBuilding.ingredients[Ingredient.CompleteDrink];
    }

    // MEAL FUNCTIONS
    public void NewMeal()
    {
        orderTray = Instantiate(orderTrayPrefab, transform);
        orderTray.transform.parent = null;
        orderTray.transform.position = spawnPoints.Find(point => point.id == "Order Tray").point.position;
        orderTray.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
        orderTray.transform.localScale *= 3.0f;

        currentMeal = new Meal();
        burgerLayout.SetActive(false);
        friesLayout.SetActive(false);
        drinkLayout.SetActive(false);
        isAssembling = true;
    }

    public void ResetMeal()
    {
        isAssembling = false;

        // Return items from the meal to the storage
        foreach(Ingredient ingredient in currentMeal.food.Keys)
        {
            if (workStation.DepositIngredient(ingredient, currentMeal.food[ingredient])) Debug.Log(currentMeal.food[ingredient] + " " + ingredient.ToString() + " returned to order building storage");
            else Debug.LogError("Could not return " + currentMeal.food[ingredient] + " " + ingredient.ToString() + " to order building storage");
        }

        Destroy(orderTray);
        NewMeal();
        Debug.Log("Meal reset.");
    }


    // COMPLETE MEAL EVENTS
    public void CompleteMealButtonEvent()
    {
        //print("delete customer");
        //GameObject.Destroy(GameObject.Find("CustomerOne(Clone)"));
        if (isAssembling && currentMeal.HasFood())
        {
            CompleteMeal();
        }
        else
        {
            NoMeal();
        }
    }

    public void CompleteMeal()
    {
        isAssembling = false;

        WorkStation.orderStation.gameObject.GetComponent<MealStorage>().AddMeal(currentMeal);

        audioData.Play();

        Destroy(orderTray);
        NewMeal();
        //print("delete customer");
        GameObject.Destroy(GameObject.Find("CustomerOne(Clone)"));
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
        if (!workStation.WithdrawIngredient(Ingredient.CompleteBurger, 1))
        {
            Debug.Log("Order building does not have any completed burgers!");
            return;
        }
        
        if (!currentMeal.Contains(Ingredient.CompleteBurger))
        {
            GameObject burger = Instantiate(burgerPrefab, transform);
            burger.transform.parent = null;
            burger.transform.position = spawnPoints.Find(point => point.id == "Burger").point.position;
            burger.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            burger.transform.parent = orderTray.transform;
            burgerLayout.SetActive(true);
            
        }
        
        currentMeal.AddFood(Ingredient.CompleteBurger, 1);
        burgerLayout.GetComponentInChildren<Text>().text = "x" + currentMeal.food[Ingredient.CompleteBurger].ToString();
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
            AddFries();
        }
        else
        {
            NoFries();
        }
    }

    private void AddFries()
    {
        if (!workStation.WithdrawIngredient(Ingredient.CompleteFries, 1))
        {
            Debug.Log("Order building does not have any completed fries!");
            return;
        }

        if (!currentMeal.Contains(Ingredient.CompleteFries))
        {
            GameObject fries = Instantiate(fryTrayPrefab, transform);
            fries.transform.parent = null;
            fries.transform.position = spawnPoints.Find(point => point.id == "Fries").point.position;
            fries.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            fries.transform.parent = orderTray.transform;
            friesLayout.SetActive(true);
        }

        currentMeal.AddFood(Ingredient.CompleteFries, 1);
        friesLayout.GetComponentInChildren<Text>().text = "x" + currentMeal.food[Ingredient.CompleteFries].ToString();
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
            AddDrink();
        }
        else
        {
            NoDrink();
        }
    }

    private void AddDrink()
    {
        if (!workStation.WithdrawIngredient(Ingredient.CompleteDrink, 1))
        {
            Debug.Log("Order building does not have any completed drinks!");
            return;
        }

        if (!currentMeal.Contains(Ingredient.CompleteDrink))
        {
            GameObject drink = Instantiate(drinkPrefab, transform);
            drink.transform.parent = null;
            drink.transform.position = spawnPoints.Find(point => point.id == "Drink").point.position;
            drink.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            drink.transform.parent = orderTray.transform;
            drinkLayout.SetActive(true);
        }

        currentMeal.AddFood(Ingredient.CompleteDrink, 1);
        drinkLayout.GetComponentInChildren<Text>().text = "x" + currentMeal.food[Ingredient.CompleteDrink].ToString();
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
