using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationInterface : MonoBehaviour
{
    // VARIABLES
    public WorkStation workStation;
    private bool workStationAttached;

    private void Awake()
    {
        workStationAttached = false;
    }

    private void Update()
    {
        if (workStation != null && !workStationAttached)
        {
            Debug.Log("Work station attached");
        }

        if (workStation == null)
        {
            Debug.Log("Work station not yet attached");
        }
    }
}
