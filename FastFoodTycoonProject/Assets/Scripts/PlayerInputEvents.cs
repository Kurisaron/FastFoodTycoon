using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEvents : MonoBehaviour
{
    // VARIABLES
    public Worker player;

    private WorkStation lastStationPressed;
    private bool canOpenStation;
    //private Worker currentWorker;

    // FUNCTIONS
    private void Awake()
    {
        //player = GameObject.Find("Player").GetComponent<Worker>();
        canOpenStation = false;
        //currentWorker = null;
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
                    canOpenStation = true;

                    lastStationPressed = hit.collider.gameObject.GetComponent<WorkStation>();
                    player.SetDestination(lastStationPressed.goToPoint);
                }


            }
        }
    }
}
