using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEvents : Singleton<PlayerInputEvents>
{
    // VARIABLES
    private Worker player;
    private Worker currentWorker;

    public WorkStation lastStationPressed;
    public bool canOpenStation;

    // FUNCTIONS
    public override void Awake()
    {
        base.Awake();
        
        player = GameObject.Find("Player").GetComponent<Worker>();
        currentWorker = player;
        canOpenStation = false;
    }


    public void TapEvent(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 tapPosition = context.action.ReadValue<Vector2>();
            // Debug.Log("(" + tapPosition.x.ToString() + ", " + tapPosition.y.ToString() + ")");

            if (Physics.Raycast(Camera.main.ScreenPointToRay(tapPosition), out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.GetComponent<WorkStation>() != null)
                {
                    lastStationPressed = hit.collider.gameObject.GetComponent<WorkStation>();
                    currentWorker.SetDestination(lastStationPressed);

                    if (currentWorker == player)
                    {
                        canOpenStation = true;
                    }

                    ResetWorker();
                    
                }
                else if (hit.collider.gameObject.GetComponent<Worker>() != null && !hit.collider.gameObject.name.Contains("Player"))
                {
                    currentWorker = hit.collider.gameObject.GetComponent<Worker>();

                    // TO-DO: Enable effect to show selected worker
                    currentWorker.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (hit.collider.gameObject.name.Contains("Exit"))
                {
                    player.targetStation.UnloadStationScene();
                }
                else
                {
                    ResetWorker();
                }
            }
        }
    }

    // Runs when a non-player worker is deselected (either moved to station or player pressed on non-interactable)
    private void ResetWorker()
    {
        if (currentWorker != player)
        {
            // TO-DO: Disable effect that shows worker is selected
            currentWorker.gameObject.GetComponent<Renderer>().material.color = Color.red;

            currentWorker = player;
        }
    }

}
