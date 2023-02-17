using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    // VARIABLES
    public static Worker player;
    private NavMeshAgent agent;
    public WorkStation targetStation;

    // Worker Storage
    private Tuple<Ingredient, int> heldIngredient = null;

    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (gameObject.name.Contains("Player"))
        {
            player = this;
        }
    }

    private void Update()
    {
        if (gameObject.name.Contains("Player") && PlayerInputEvents.Instance.canOpenStation && CheckPlayerDistance())
        {
            PlayerInputEvents.Instance.canOpenStation = false;

            // TO-DO: Deposit held items

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

        if (gameObject.name.Contains("Player")) Debug.Log("Now holding ingredient " + heldIngredient.Item1.ToString() + ", amount: " + heldIngredient.Item2.ToString());
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
