using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Worker : MonoBehaviour
{
    // VARIABLES
    public static Worker player;
    private NavMeshAgent agent;
    public WorkStation targetStation;

    // Worker Storage
    private Tuple<Ingredient, int> heldIngredient = null;
    [SerializeField]
    private GameObject heldItemCanvas;
    [SerializeField]
    private GameObject heldItemCanvas_Prefab;

    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (gameObject.name.Contains("Player"))
        {
            player = this;

            heldItemCanvas = Instantiate(heldItemCanvas_Prefab, null);
            heldItemCanvas.GetComponent<WorkerSpeechBubble>().Worker = this;

            heldItemCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameObject.name.Contains("Player") && PlayerInputEvents.Instance.canOpenStation && CheckPlayerDistance())
        {
            PlayerInputEvents.Instance.canOpenStation = false;

            // Deposit held items
            if ((heldIngredient != null && targetStation.DepositIngredient(heldIngredient.Item1, heldIngredient.Item2)) || heldIngredient == null)
            {
                ReleaseIngredient();
                targetStation.LoadStationScene();
            }
            else
            {
                Debug.Log("Workstation cannot accept held item");
            }

            if (heldItemCanvas.activeInHierarchy) heldItemCanvas.GetComponent<RectTransform>().position = new Vector3(gameObject.transform.position.x, heldItemCanvas.GetComponent<RectTransform>().position.y, gameObject.transform.position.z);
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

        // Turn on visuals for held item (UI or model needed)
        heldItemCanvas.SetActive(true);
        heldItemCanvas.GetComponent<WorkerSpeechBubble>().SetImage(ingredient);
    }

    public void ReleaseIngredient()
    {
        if (heldIngredient == null)
        {
            Debug.Log("Not holding ingredient");
            return;
        }

        heldIngredient = null;
        // Turn off visuals for held item (UI or model needed)
        heldItemCanvas.SetActive(false);
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
