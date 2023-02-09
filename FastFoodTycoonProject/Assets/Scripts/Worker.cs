using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    // VARIABLES
    private NavMeshAgent agent;
    public WorkStation targetStation;

    // Worker Storage
    private Tuple<Ingredient, int> heldIngredient = null;

    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (gameObject.name.Contains("Player") && PlayerInputEvents.Instance.canOpenStation && CheckPlayerDistance())
        {
            PlayerInputEvents.Instance.canOpenStation = false;
            targetStation.LoadStationScene();
        }
    }

    public void SetDestination(WorkStation station)
    {
        targetStation = station;
        agent.destination = station.goToPoint.position;
    }

    public Vector3 GetDestination()
    {
        return agent.destination;
    }

    public void HoldIngredient(Ingredient ingredient, int amount)
    {
        if (heldIngredient != null)
        {
            Debug.Log("Already holding ingredient");
            return;
        }

        heldIngredient = new Tuple<Ingredient, int>(ingredient, amount);
        // To-Do: Turn on visuals for held item (UI or model needed)
    }

    public void ReleaseIngredient()
    {
        if (heldIngredient == null)
        {
            Debug.Log("Not holding ingredient");
            return;
        }

        heldIngredient = null;
        // To-Do: Turn on visuals for held item (UI or model needed)
    }


    private bool CheckPlayerDistance()
    {
        if (Vector3.Distance(GetDestination(), transform.position) <= 2.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
