using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FryerInterface : WorkStationInterface
{
    // VARIABLES
    public Transform[] fryer_SpawnPoints;

    public GameObject fryerBasketPrefab;
    public GameObject completeFriesPrefab;
    public GameObject timerUIPrefab;
    public GameObject playAnim;
    public AudioSource completionAudio;
    public AudioSource fryerAudio;

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

    private void Start()
    {

    }

    private void OnEnable()
    {
        FryerCooker.startCooking += Fryer_StartCooking;
        FryerCooker.stillCooking += Fryer_StillCooking;
        FryerCooker.endCooking += Fryer_EndCooking;
    }

    private void OnDisable()
    {
        FryerCooker.startCooking -= Fryer_StartCooking;
        FryerCooker.stillCooking -= Fryer_StillCooking;
        FryerCooker.endCooking -= Fryer_EndCooking;
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
                    if (pressedIngredient.stepTime >= 2.0f)
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


    private void Fryer_StartCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        fryerAudio.Play();

        // Spawn and attach cooking timer UI to spawn point transform
        GameObject newTimer = Instantiate(timerUIPrefab, spawnPoint);
        newTimer.transform.localScale *= 0.01f;
        newTimer.transform.SetPositionAndRotation(newTimer.transform.position + (Vector3.up * 0.1f), Quaternion.Euler(45, 180, 0));
        newTimer.transform.Find("TimerUI").gameObject.GetComponent<Slider>().value = 0;
    }

    private void Fryer_StillCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        if (spawnPoint.Find("TimerUICanvas(Clone)") != null)
        {
            // Update the cooking timer UI
            Slider mySlider = spawnPoint.Find("TimerUICanvas(Clone)").Find("TimerUI").gameObject.GetComponent<Slider>();
            mySlider.value = Mathf.Lerp(mySlider.minValue, mySlider.maxValue, Mathf.InverseLerp(0, 2.0f, cookingIngredient.stepTime));
        }
    }

    private void Fryer_EndCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        // Clear cooking timer UI
        if (spawnPoint.Find("TimerUICanvas(Clone)") != null)
        {
            Destroy(spawnPoint.Find("TimerUICanvas(Clone)").gameObject);
        }
    }

    private void PlaceFries(int index, int step)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];

        RemoveFries(index);

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

    private void RemoveFries(int index)
    {
        Transform spawnPoint = fryer_SpawnPoints[index];

        if (!Array.Exists(spawnPoint.gameObject.GetComponentsInChildren<Transform>(), child => child.gameObject.name.Contains("Fry"))) return;

        foreach (Transform fryChild in Array.FindAll(spawnPoint.gameObject.GetComponentsInChildren<Transform>(), child => child.gameObject.name.Contains("Fry")))
        {
            if (fryChild == null) continue;

            Debug.Log("Removing child of spawn point " + index.ToString() + " with name containing Fry [" + fryChild.gameObject.name + "]");
            Destroy(fryChild.gameObject);
        }

        /*
        if (fryer_SpawnPoints[index].Find("Fryer Basket(Clone)") != null && FryerCooker.cookingIngredients[index] != null && FryerCooker.cookingIngredients[index].stepsComplete != 0)
        {
            Debug.Log("Removing Fryer Basket");
            Destroy(fryer_SpawnPoints[index].Find("Fryer Basket(Clone)").gameObject);
        }

        if (fryer_SpawnPoints[index].Find("FryBasketComplete(Clone)") != null && FryerCooker.cookingIngredients[index] != null && FryerCooker.cookingIngredients[index].stepsComplete != 0)
        {
            Debug.Log("Removing Complete Fries");
            Destroy(fryer_SpawnPoints[index].Find("FryBasketComplete(Clone)").gameObject);
        }

        Transform[] allChildren = spawnPoint.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            Debug.LogError(child.gameObject.name + " still active");
        }
        */

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

        GameObject anim;

        //Debug.Log("Pass Routine " + index.ToString() + " starting loop");

        completionAudio.Play();

        anim = Instantiate(playAnim, spawnPoint);
        anim.transform.position = spawnPoint.position + (spawnPoint.up * 0.8f);

        Destroy(anim, 1.1f);

        while (FriesVisible(fries))
        {
            //Debug.Log("Pass Routine " + index.ToString() + " loop iteration");
            fries.transform.position += spawnPoint.right * -5.0f * Time.deltaTime;
            yield return null;
        }

        //Debug.Log("Pass Routine " + index.ToString() + " ending loop");

        //Debug.Log("Pass Routine " + index.ToString() + " playing audio");

        //Debug.Log("Pass Routine " + index.ToString() + " almost complete, need to destroy current fries");

        Destroy(fries);

    }

    private bool FriesVisible(GameObject fries)
    {
        Vector3 friesRelativePos = Camera.main.WorldToViewportPoint(fries.transform.position, Camera.MonoOrStereoscopicEye.Mono);

        if ((0 <= friesRelativePos.x && friesRelativePos.x <= 1) && (0 <= friesRelativePos.y && friesRelativePos.y <= 1) && friesRelativePos.z > 0) return true;

        return false;
    }

    private Transform GetSpawnPoint(CookingStation.CookingIngredient cookingIngredient)
    {
        return fryer_SpawnPoints[Array.FindIndex(FryerCooker.cookingIngredients, ingredient => ingredient == cookingIngredient)];
        // returns the transform associated with the provided CookingIngredient
    }
}
