using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEvents : MonoBehaviour
{
    // VARIABLES
    //private Worker currentWorker;

    // FUNCTIONS
    private void Awake()
    {
        //currentWorker = null;
    }

    public void TapEvent(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 tapPosition = context.action.ReadValue<Vector2>();
            // Debug.Log("(" + tapPosition.x.ToString() + ", " + tapPosition.y.ToString() + ")");

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(tapPosition), out hit, Mathf.Infinity))
            {
                // FINISH
            }
        }
    }
}
