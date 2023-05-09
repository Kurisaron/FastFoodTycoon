using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public List<CustomerController> customers = new List<CustomerController>();
    public delegate void CustomerDespawned();
    public static event CustomerDespawned OnCustomerDespawned;
    public float speed = 4;
    public Vector3 target;
    public Vector3 target2;
    public GameObject LimitWall;
    public GameObject OrderIndicator;
    Rigidbody CustomerRB;
    public Transform CustomerPos;
    public Dictionary<Ingredient, int> food;

    void Awake()
    {
        LimitWall = GameObject.Find("CustomerLimitWall");
        target2 = LimitWall.transform.position;
        OrderIndicator.SetActive(false);
        food = new Dictionary<Ingredient, int>();
        int ingredientAmount = 0;
        foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
        {
            if (ingredient.ToString().Contains("Complete"))
            {
                food.Add(ingredient, UnityEngine.Random.Range(0, 3));
                ingredientAmount += food[ingredient];
            }
        }
        if (ingredientAmount <= 0)
        {
            switch(UnityEngine.Random.Range(0,3))
            {
                case 0:
                    food[Ingredient.CompleteBurger] = 1;
                    break;
                case 1:
                    food[Ingredient.CompleteFries] = 1;
                    break;
                case 2:
                    food[Ingredient.CompleteDrink] = 1;
                    break;
                default:
                    food[Ingredient.CompleteBurger] = 1;
                    break;
            }
        }
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        CheckCompletion();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Register")
        {
            OrderIndicator.SetActive(true);
        }
        /*else if (other.tag == "Player")
        {
            float step = speed * Time.deltaTime;
            OrderIndicator.SetActive(false);
        }    */ 
        else if (other.tag == "LimitWall")
        {
            GameManager.Instance.EarnMoney(50);
            CustomerSpawnOne.Instance.customers.Remove(this);
            CustomerSpawnOne.Instance.SpawnNewCustomer();
            Destroy(this.gameObject);
        }
    }

    public void CheckCompletion()
    {
        bool flag = true;
        foreach (Ingredient ingredient in food.Keys)
        {
            if (WorkStation.orderStation.gameObject.GetComponent<MealStorage>().food[ingredient] < food[ingredient])
                flag = false;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if (flag)
        {
            foreach (Ingredient ingredient1 in food.Keys)
            {
                WorkStation.orderStation.gameObject.GetComponent<MealStorage>().food[ingredient1] -= food[ingredient1];
                Debug.Log(ingredient1.ToString() + " Count Equals " + WorkStation.orderStation.gameObject.GetComponent<MealStorage>().food[ingredient1].ToString());
            }
            StartCoroutine(CompletionRoutine());
        }
    }

    private IEnumerator CompletionRoutine()
    {
        while (Vector3.Distance(transform.position, target2) > 0.1f)
        {
            //float step = speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target2, 0.1f);
            yield return null;
        }
        //GameManager.Instance.EarnMoney(50);
        //CustomerSpawnOne.Instance.customers.Remove(this);
    }
}