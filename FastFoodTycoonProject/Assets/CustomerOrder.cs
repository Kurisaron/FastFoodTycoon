using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrder : Singleton<CustomerOrder>
{
    private List<CustomerController> Scripts;

    //CustomerOrder.Instance.AddCustomer(someController);

    // Start is called before the first frame update
    void Start()
    {
        //Scripts.Add(GameObject.FindGameObjectsWithTag("Customer").GetComponent<CustomerController>);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCustomer(CustomerController controller)
    {
        Scripts.Add(controller);
    }
}
