using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public delegate void CustomerDespawned();
    public static event CustomerDespawned OnCustomerDespawned;
    //public List<GameObject> waypoints;
    public float speed = 4;
    //public Transform target;
    public Vector3 target;
    public Vector3 target2;
    public GameObject OrderIndicator;
    public MeshRenderer PLS;

    // Start is called before the first frame update
    void Start()
    {
        PLS.enabled = false;
        //transform.position = target.position;
        //transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        //transform.position = transform.position + new Vector3(0, 0, 0);
        //setRigidbodyState(true);
        //setColliderState(false);
        //GetComponent<Animator>().enabled = true;
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target2, step);
    }

    public void despawn()
    {
        //setRigidbodyState(false);
        //setColliderState(true);

        if (gameObject != null)
        {
            Destroy(gameObject, 3f);
        }

        if(OnCustomerDespawned != null)
        {
            OnCustomerDespawned();
        }
    }

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
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
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
            //Destroy(GameObject.FindGameObjectWithTag("OrderIndicator"));
            //Destroy(other.gameObject);
            //DestroyGameObject();
            print("destroyed order indicator");
        }
        else if (other.tag == "Player")
        {
            PLS.enabled = false;
        }
    }
}