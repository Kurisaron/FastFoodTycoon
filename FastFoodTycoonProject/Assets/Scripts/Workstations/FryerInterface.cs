using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FryerInterface : WorkStationInterface
{
    // VARIABLES
    public Transform[] fryer_SpawnPoints;

    public GameObject fryerBasketPrefab;
    public GameObject completeFriesPrefab;

    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // PROPERTIES
    private CookingStation FryerCooker
    {
        get
        {
            return workStation.gameObject.GetComponent<CookingStation>();
        }
    }

    // FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

        PlayerInputEvents.Instance.workstationTapAction = FryerTapEvent;

        for (int i = 0; i < FryerCooker.cookingIngredients.Length; i++)
        {
            if (FryerCooker.cookingIngredients[i] != null)
            {
                PlaceFries(i, FryerCooker.cookingIngredients[i].stepsComplete);
            }
        }
    }

    /*
    private void Update()
    {
        for (int i = 0; i < FryerCooker.cookingIngredients.Length; i++)
        {
            CookingStation.CookingIngredient ingredient = FryerCooker.cookingIngredients[i];
            if (ingredient != null  && (check if old stuff from past step needs to be removed))
            {
                 TO-DO: Add in new visuals for the current step
            }
        }
    }
    */

    private void FryerTapEvent(RaycastHit hit)
    {
        
        if (hit.collider.gameObject.name.Contains("CookSelection"))
        {
            if (FryerCooker.TryCookingIngredient(out int index))
            {
                PlaceFries(index, 0);
            }
        }


        if (hit.collider.gameObject.name.Contains("SpawnPoint"))
        {
            string str = string.Empty;
            for (int i = 0; i < hit.collider.gameObject.name.Length; i++)
            {
                if (Char.IsDigit(hit.collider.gameObject.name[i]))
                {
                    str += hit.collider.gameObject.name[i];
                }
            }

            if (str.Length > 0)
            {
                int val = int.Parse(str);
                Debug.Log("Spawn point " + str + " pressed");

                CookingStation.CookingIngredient pressedIngredient = FryerCooker.cookingIngredients[val];
                if (pressedIngredient != null)
                {
                    if (pressedIngredient.stepTime >= 1.0f)
                    {
                        if (pressedIngredient.stepsComplete < pressedIngredient.steps)
                        {
                            FryerCooker.CookingIngredient_NextStep(pressedIngredient);
                            PlaceFries(val, 1);
                        }
                        else
                        {
                            FryerCooker.PassIngredient(pressedIngredient);
                            PlaceFries(val, 2);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No digits found");
            }
        }
    }


    private void PlaceFries(int index, int step)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];

        if (spawnPoint.childCount > 0)
        {
            Destroy(spawnPoint.GetChild(0).gameObject);
        }
        
        GameObject fries = Instantiate(step == 2 ? completeFriesPrefab : fryerBasketPrefab, spawnPoint);

        if (step == 0)
        {
            StartCoroutine(DropInRoutine(fries, index));
        }

        if (step == 1)
        {
            StartCoroutine(PullUpRoutine(fries, index));
        }

        if (step == 2)
        {
            StartCoroutine(PassRoutine(fries, index));
        }
    }

    private IEnumerator DropInRoutine(GameObject basket, int index)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];

        basket.transform.position = spawnPoint.position + (spawnPoint.up * 0.5f);
        Vector3 startPos = basket.transform.position;

        float timeElapsed = 0.0f;
        while (timeElapsed < 1.0f)
        {
            basket.transform.position = Vector3.Lerp(startPos, spawnPoint.position, timeElapsed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator PullUpRoutine(GameObject basket, int index)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];
        Vector3 endPos = spawnPoint.position + (spawnPoint.up * 0.5f);

        float timeElapsed = 0.0f;
        while (timeElapsed < 1.0f)
        {
            basket.transform.position = Vector3.Lerp(spawnPoint.position, endPos, timeElapsed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator PassRoutine(GameObject fries, int index)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];

        while(fries.GetComponentInChildren<Renderer>().isVisible)
        {
            fries.transform.position += -spawnPoint.right * 5.0f * Time.deltaTime;
            yield return null;
        }

        audioData.Play();

        Destroy(fries);
    }
}
