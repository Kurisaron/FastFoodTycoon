using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //public int count;
    Rigidbody CustomerRB;
    public Transform CustomerPos;
    //CustomerController.GetComponent<OrderStationInterface>().CompleteMeal();
    //public void SetActive(bool value);
    //bool CompleteMeal();
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
        PLS.enabled = false;
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
    void Update()
    {
        //these two functions must be together. Moves customer towards target in inspector.
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
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
            print("added order indicator");
        }
        else if (other.tag == "Player")
        {
            print("collided with player");
            //elapsedTime += Time.deltaTime;
            float step = speed * Time.deltaTime;
            PLS.enabled = false;
            //transform.position = Vector3.MoveTowards(transform.position, target2, step);
        }     
        else if (other.tag == "Customer")
        {
            print("customer in the way");
            //float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, CustomerPos.position, step);
            //elapsedTime += Time.deltaTime;
            //CustomerRB.velocity = Vector3.zero;
            //RigidBody.IsKinematic = false;
            //GetComponenet<Rigidbody>().velocity = Vector3.zero;
            //return;
        }
        else if (other.tag == "LimitWall")
        {
            print("despawned customer");
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
}