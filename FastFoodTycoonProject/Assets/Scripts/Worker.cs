using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    // VARIABLES
    private NavMeshAgent agent;

    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Transform transform)
    {
        agent.destination = transform.position;
    }

    public Vector3 GetDestination()
    {
        return agent.destination;
    }
}
