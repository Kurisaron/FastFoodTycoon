using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnOne : Singleton<CustomerSpawnOne>
{
    
    public List<CustomerController> customers = new List<CustomerController>();

    public Transform m_SpawnPoints;
    public GameObject m_CustomerPrefab;
    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;

    private void Awake()
    {
        SpawnNewCustomer();
    }

    public void OnEnable()
    {
        CustomerController.OnCustomerDespawned += SpawnNewCustomer;
    }

    public void SpawnNewCustomer()
    {
        GameObject newCustomer = Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
        customers.Add(newCustomer.GetComponent<CustomerController>());
    }

    private void CheckForCustomers()
    {

    }

    void Update()
    {
        elapsedTime = elapsedTime + Time.deltaTime;
    }
}
