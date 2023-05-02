using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnOne : MonoBehaviour
{

    //public GameObject[] CustomerArr;
    //public Quaternion rotation;
    //Transform target;

    public List<CustomerController> customers = new List<CustomerController>();

    //Attempt #3
    /*public GameObject CustomerPrefab;
    public void Spawn()
    {
        GameObject Customer = (GameObject)Instantiate(CustomerPrefab, transform.position, transform.rotation);
    }*/

    //Attempt #2, spawns a customer, when customer despawns a new one spawns
    public Transform m_SpawnPoints;
    //PROBLEM?
    public GameObject m_CustomerPrefab;
    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;
    //bool customerExists = (GameObject.Find("Customer(Clone)") != null);
    //var Customer : GameObject;
    //var Customer : GameObject;
    //[SerializeField]
    //public GameObject Customer;

    private void Start()
    {
        //CheckForCustomers();
        /* foreach(customers customers in customers.keys)
         {
             if(customers)
             {
                 SpawnNewCustomer();
             }
         }*/
        /*if (customers)
        {
            SpawnNewCustomer();
        }*/
        SpawnNewCustomer();
        //elapsedTime = 0f;
        //CustomerArr = m_CustomerPrefab[numSelectors];
    }

    private void OnEnable()
    {
        CustomerController.OnCustomerDespawned += SpawnNewCustomer;
        /*elapsedTime -= Time.deltaTime;
        if (elapsedTime <= 0)
        {
            SpawnNewCustomer();
        }*/
    }

    void SpawnNewCustomer()
    {
        //elapsedTime += Time.deltaTime;
        /*if(elapsedTime <= 2)
        {*/
        //print("spawn new customer");
        //elapsedTime = 0.0f;
        //}
        GameObject newCustomer = Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
        customers.Add(newCustomer.GetComponent<CustomerController>());
        //transform.Rotate(new Vector3(axis2, axis1, 0), Space.World);
    }


    //Attempt #1
    /*[SerializeField]
    private GameObject CustomerPrefab;

    [SerializeField]
    private float CustomerInterval = 3.5f;*/

    // Start is called before the first frame update
    /*void Start()
    {
        StartCoroutine(SpawnNewCustomer(CustomerInterval, CustomerPrefab));
    }*/

    private void CheckForCustomers()
    {
        /*if (CustomerArr = 1)
        {
            print("no customers here");
            return;
        }
        else
        {
            print("spawning customer");
            SpawnNewCustomer();
        }*/
        /*if (GameObject.Find("Customer1(Clone)")) //!= null)
        {
            //Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            return;
            //print("no customer found");
        }
        else
        {
            SpawnNewCustomer();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = elapsedTime + Time.deltaTime;
        //CheckForCustomers();
        //SpawnNewCustomer();
        /*else
        {
            print("customer found");
            return;
        }*/
        /*if (elapsedTime >= secondsBetweenSpawn)
        {
            print("spawn new customer");
            SpawnNewCustomer();
            //Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            elapsedTime = 0.0f;
        }*/
        /*if (SpawnNewCustomer(););
        {
            elapsedTime = 0f
        }*/
        //if (Input.GetKey("a")) backup = Instantiate(Customer, transform.position, transform.rotation);
        /*elapsedTime += Time.deltaTime;

        if (elapsedTime > secondsBetweenSpawn) 
        {
            elapsedTime = 0;
            Debug.Log(true);
            SpawnNewCustomer();
            //Vector3 spawnPosition = new Vector3(4.71f, 0.72f, 0f);
            //GameObject newEnemy = (GameObject)Instantiate(enemyObject, spawnPosition, Quaternion.Euler(0, 0, 0));
        }*/
    }

    /*private IEnumerable SpawnCustomer(float interval, GameObject Customer)
    {
        yield return new WaitForSeconds(interval);
        GameObject newCustomer = Instantiate(Customer, new Vector3(5, 0, 0), Quaternion.identity);
        StartCoroutine(SpawnCustomer(interval, Customer));
    }*/
}
