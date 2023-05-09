using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // VARIABLES
    public float money;
    [HideInInspector]
    public float moneyAtDayStart;
    public DayData dayData;

    // FUNCTIONS
    public void NewGameData()
    {
        if (dayData == null) dayData = new DayData(1, 0.0f);
        else
        {
            dayData.day = 1;
            dayData.dayTime = 0.0f;
        }
        
    }

    // CLASSES
    public class DayData
    {
        public int day;
        [Range(0.0f, 1.0f)]
        public float dayTime;

        public DayData(int d, float dt)
        {
            day = d;
            dayTime = dt;
        }
    }
}
