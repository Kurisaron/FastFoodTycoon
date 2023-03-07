using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingTimer : MonoBehaviour
{
    [SerializeField] private Slider slider;


    public void SetCookingTime() //this sets the slider values
    {
        slider.value = 0;
        //slider.maxValue = however long it takes to cook something
    }

    public void SetTimerProgress(float timerProgress)
    {
        slider.value = timerProgress;
    }
}
