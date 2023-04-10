using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    //Attempt #3

    /*public GameObject CustomerPrefab;

    public void Spawn()
    {
        GameObject Customer = (GameObject)Instantiate(CustomerPrefab, transform.position, transform.rotation);
    }*/


    //Attempt #2, spawns a customer, when customer despawns a new one spawns
    public Transform m_SpawnPoints;
    public GameObject m_CustomerPrefab;

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
        Instantiate(m_CustomerPrefab, m_SpawnPoints.transform.position, Quaternion.identity);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerable SpawnCustomer(float interval, GameObject Customer)
    {
        yield return new WaitForSeconds(interval);
        GameObject newCustomer = Instantiate(Customer, new Vector3(5, 0, 0), Quaternion.identity);
        StartCoroutine(SpawnCustomer(interval, Customer));
    }*/
}
