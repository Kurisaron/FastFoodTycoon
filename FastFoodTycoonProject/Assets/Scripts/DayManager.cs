using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    private float dayDuration /*in seconds*/ = 600.0f;

    public override void Awake()
    {
        base.Awake();

        StartCoroutine(DayRoutine());
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
                    GameManager.Instance.SetCountdownText(dayTime);
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
