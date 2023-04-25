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

    private GameObject orderTray;
    private Meal currentMeal;
    private bool isAssembling;

    AudioSource audioData;

    [SerializeField]
    private Text orderText;
    [SerializeField]
    private GameObject orderWindowContainer;

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
        //orderWindowContainer = DisplayOrder(); //doesn't work rn
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
        isAssembling = true;
    }

    public void ResetMeal()
    {
        isAssembling = false;
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
        }

        currentMeal.AddFood(Ingredient.CompleteFries, 1);
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
            Debug.Log("Order building does not have any completed burgers!");
            return;
        }

        if (!currentMeal.Contains(Ingredient.CompleteDrink))
        {
            GameObject drink = Instantiate(drinkPrefab, transform);
            drink.transform.parent = null;
            drink.transform.position = spawnPoints.Find(point => point.id == "Drink").point.position;
            drink.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
            drink.transform.parent = orderTray.transform;
        }

        currentMeal.AddFood(Ingredient.CompleteDrink, 1);
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

    /*private GameObject DisplayOrder()
    {
        currentMeal.GetItemDisplay().transform.SetParent(orderWindowContainer.transform);
        
        return currentMeal.GetItemDisplay();
    }*/

    // CLASSES
    [Serializable]
    public class BuildingSpawnPoint
    {
        public string id;
        public Transform point;
    }   
}
