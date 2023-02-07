using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkStation : MonoBehaviour
{
    // VARIABLES
    [Tooltip("The point that workers stand at to work at the station (Pathfinding)")]
    public Transform goToPoint;

    // FUNCTIONS
    public void LoadStationScene()
    {
        SceneManager.LoadScene(gameObject.name + "Scene", LoadSceneMode.Additive);
    }
}
