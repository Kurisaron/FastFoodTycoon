using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationInterface : MonoBehaviour
{
    // VARIABLES
    public WorkStation workStation;
    protected bool workStationAttached;

    protected virtual void Awake()
    {
        workStation = Worker.player.targetStation;

        Debug.Log(workStation.gameObject.name + " opened");
        foreach(KeyValuePair<Ingredient, int> kvp in workStation.ingredients)
        {
            Debug.Log(kvp.Key.ToString() + ": " + kvp.Value.ToString());
        }
    }

    public void ExitButton()
    {
        Worker.player.targetStation.UnloadStationScene();
    }
}
