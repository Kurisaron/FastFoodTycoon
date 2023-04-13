using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour

{
    //public Quaternion rotation;
    //Transform target;

    //Attempt #3
    /*public GameObject CustomerPrefab;
    public void Spawn()
    {
        GameObject Customer = (GameObject)Instantiate(CustomerPrefab, transform.position, transform.rotation);
    }*/


    //Attempt #2, spawns a customer, when customer despawns a new one spawns
    public Transform m_SpawnPoints;
    public GameObject m_CustomerPrefab;
    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;

    private void Start()
    {
        SpawnNewCustomer();
    }

    private void OnEnable()
    {
        CustomerController.OnCustomerDespawned += SpawnNewCustomer;
    }

    void SpawnNewCustomer()
    {
        Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, transform.rotation);
        //transform.Rotate(new Vector3(axis2, axis1, 0), Space.World);
    }


    //Attempt #1
    /*[SerializeField]
    private GameObject CustomerPrefab;

    [SerializeField]
    private float CustomerInterval = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCustomer(CustomerInterval, CustomerPrefab));
    }*/

    // Update is called once per frame
    void Update()
    {
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


