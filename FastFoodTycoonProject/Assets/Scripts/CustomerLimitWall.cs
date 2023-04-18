using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLimitWall : MonoBehaviour
{
    bool IsColliding = false;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsColliding)
        {  
            timer += Time.deltaTime;
        }
        if (timer > 2)
        {
            DestroyGameObject();
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
    }
}
