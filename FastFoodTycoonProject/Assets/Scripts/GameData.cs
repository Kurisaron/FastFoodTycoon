using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // VARIABLES
    public float money;
    public DayData dayData;

    // FUNCTIONS
    public void NewGameData()
    {
        dayData.day = 1;
        dayData.dayTime = 0.0f;
    }

    // CLASSES
    public class DayData
    {
        public int day;
        [Range(0.0f, 1.0f)]
        public float dayTime;
    }
}
