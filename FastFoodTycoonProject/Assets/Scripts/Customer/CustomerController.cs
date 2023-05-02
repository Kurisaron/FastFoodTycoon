using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    //OrderBuildingInterface OrderBuildingInterfaceScript;

    public delegate void CustomerDespawned();
    public static event CustomerDespawned OnCustomerDespawned;
    //public List<GameObject> waypoints;
    public float speed = 4;
    //public Transform target;
    public Vector3 target;
    public Vector3 target2;
    public GameObject OrderIndicator;
    public MeshRenderer PLS;
    //public int count;
    Rigidbody CustomerRB;
    public Transform CustomerPos;
    //public static float Range(float minInclusive, float maxInclusive);
    //public class Dictionary<>
    //CustomerController.GetComponent<OrderStationInterface>().CompleteMeal();
    //public void SetActive(bool value);
    //bool CompleteMeal();
    //Dictionary<Meal, int> order;
    //CustomerSpawnOne s1;
    public Dictionary<Ingredient, int> food;


    /*private int collisionCount = 0;
    
    public bool IsNotColliding
    {
        get { return collisionCount == 0; }
    }

    void OnCollisionEnter(Collision col)
    {
        collisionCount++;
    }

    void OnCollisionExit(Collision col)
    {
        collisionCount--;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //RANDOMIZE CUSTOMER ORDER
        /*string[] CustomerOrder = new string[] { "CompleteBurger", "CompleteFries", "CompleteDrink" };
        System.Random random = new System.Random();
        int useCustomerOrder = random.Next(CustomerOrder.Length);
        string pickCustomerOrder = CustomerOrder[useCustomerOrder];
        print(pickCustomerOrder);*/

        /*s1 = GetComponent<CustomerSpawnOne>();
        yield return new WaitForEndOfFrame();
        foreach (newCustomer s in s1.customers)
            print(s);*/

        //don't show exclamation mark
        PLS.enabled = false;
        // Set up the dictionary with entries for Complete ingredients
        food = new Dictionary<Ingredient, int>();
        //food.Add(Ingredient.CompleteFries, 1);
        foreach (Ingredient ingredient in food.Keys)
        {
            if (ingredient.ToString().Contains("Complete"))
                food.Add(ingredient, UnityEngine.Random.Range(0,3));
        }
        //meals = new List<Meal>();

        /*food = new Dictionary<Ingredient, int>();
        foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
        {
            if (ingredient.ToString().Contains("Complete"))
                food.Add(ingredient, 0);
        }*/

        //order = new Dictionary<Meal, int>();
        //order.Add(Meal, 1);
        //OrderBuildingInterfaceScript = GameObject.FindGameObjectWithTag("OrderBuildingInterface").GetComponent<OrderBuildingInterface>();
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target, step);
        //transform.position = target.position;
        //transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        //transform.position = transform.position + new Vector3(0, 0, 0);
        //setRigidbodyState(true);
        //setColliderState(false);
        //GetComponent<Animator>().enabled = true;
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target2, step);
    }

    /*public void despawn()
    {
        //setRigidbodyState(false);
        //setColliderState(true);

        if (gameObject != null)
        {
            Destroy(gameObject, 3f);
        }

        if (OnCustomerDespawned != null)
        {
            OnCustomerDespawned();
        }
    }*/

    /*void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }*/

    // Update is called once per frame
    /*public void CheckOrders()
    {
        foreach (Meal meal in StationMealStorage.meals)
        {
            bool flag = true;

            foreach (Ingredient ingredient in meal.food.Keys)
            {
                if (order.food[ingredient] != meal.food[ingredient]) flag = false;
                //quantify order
            }

            if (flag)
            {
                NewMeal();
                // Remove meal from meal storage and give it to the customer
            }
        }
    }*/

    void Update()
    {
        //these two functions must be together. Moves customer towards target in inspector.
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        CheckCompletion();
        //CheckOrders();
        //CustomerPos = GameObject.Find("Customer(clone)").transform;
        /*if (CompleteMeal() == true);
        {
            transform.position = Vector3.MoveTowards(transform.position, target2, step);
        }*/
        /*if (collisionCount == 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }*/
        //transform.position = Vector3.MoveTowards(transform.position, target2, step);
        /*int index = 0;
        Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
        transform.position = newPos;*/
    }

    /*void DestroyGameObject()
    {
        Destroy(gameObject);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Register")
        {
            PLS.enabled = true;
            //elapsedTime += Time.deltaTime;
            //count++;
            //Destroy(GameObject.FindGameObjectWithTag("OrderIndicator"));
            //Destroy(other.gameObject);
            //DestroyGameObject();
            //print("added order indicator");
        }
        else if (other.tag == "Player")
        {
            //print("collided with player");
            //elapsedTime += Time.deltaTime;
            float step = speed * Time.deltaTime;
            PLS.enabled = false;
            //transform.position = Vector3.MoveTowards(transform.position, target2, step);
        }     
        else if (other.tag == "Customer")
        {
            //print("customer in the way");
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, CustomerPos.position, step);
            //elapsedTime += Time.deltaTime;
            //CustomerRB.velocity = Vector3.zero;
            //RigidBody.IsKinematic = false;
            //GetComponenet<Rigidbody>().velocity = Vector3.zero;
            //return;
        }
        else if (other.tag == "LimitWall")
        {
            //print("despawned customer");
            //elapsedTime += Time.deltaTime;
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }
        /*else
        {
            print("move toward target");
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }*/
        //return;
    }

    /*public void ReferenceFunction()
    {

        bool flag = true;
        foreach (Ingredient ingredient1 in order.food.Keys)
        {
            if (food[ingredient1] < order.food[ingredient1])
            {
                flag = false;
            }
        }

        if (flag)
        {
            // Pass food
            foreach (Ingredient ingredient1 in order.food.Keys)
            {
                food[ingredient1] -= order.food[ingredient1];
            }
        }

        
        /*if (!FindObjectOfType<CustomerController>())
        {
            // Spawn customer
        }

    
        if (FindObjectsOfType<CustomerController>().Length < 8)
        {

        }*/
    //}

    /*IEnumerator Start()
    {
        s1 = GetComponent<CustomerSpawnOne>();
        yield return new WaitForEndOfFrame();
        foreach (string s in s1.testList)
            print(s);
    }*/

    public void CheckCompletion()
    {
        bool flag = true;
        foreach (Ingredient ingredient in food.Keys)
        {
            if (WorkStation.orderStation.gameObject.GetComponent<MealStorage>().food[ingredient] < food[ingredient])
                flag = false;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target2, step);
        }

        if (flag)
        {
            // Pass food
            foreach (Ingredient ingredient1 in food.Keys)
            {
                WorkStation.orderStation.gameObject.GetComponent<MealStorage>().food[ingredient1] -= food[ingredient1];
            }
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            // Use up food
            // FILL-IN
            GameManager.Instance.EarnMoney(5);
            //customers.Remove(newCustomer.GetComponent<CustomerController>());
        }
    }
}