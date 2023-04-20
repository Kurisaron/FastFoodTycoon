using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : Singleton<DayManager>
{
    private float dayDuration; //in seconds
    private Slider daySlider;

    public override void Awake()
    {
        base.Awake();

    }

    public void Init(float duration, Slider slider)
    {
        dayDuration = duration;
        daySlider = slider;
        daySlider.value = daySlider.minValue;

        //StartCoroutine(DayRoutine());
    }

    private IEnumerator DayRoutine()
    {
        //Debug.Log("Day Routine started");
        while(true)
        {
            //Debug.Log("Day started");

            float dayTime = dayDuration;
            while (dayTime > 0.0f)
            {
                if (GameManager.Instance.gameActive)
                {
                    dayTime -= Time.deltaTime;

                    daySlider.value = (dayDuration - dayTime) / dayDuration;
                }

                yield return null;
            }

            // TO-DO: Insert "wait until" all orders have been filled

            //Debug.Log("Day over");

            GameManager.Instance.gameActive = false;

            // Display summary for the day and pull up the buy screen;
            GameManager.Instance.EndDay();

            yield return new WaitUntil(() => GameManager.Instance.gameActive);
        }
    }

}
