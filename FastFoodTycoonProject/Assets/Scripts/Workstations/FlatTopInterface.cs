using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FlatTopInterface : WorkStationInterface
{
    // VARIABLES
    public GameObject uncookedPattyPrefab;
    public GameObject halfCookedPattyPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject timerUIPrefab;
    public GameObject playAnim;

    public Transform[] flatTop_SpawnPoints;

    public AudioSource grillAudio;
    public AudioSource completionAudio;

    // PROPERTIES
    private CookingStation FlatTopCooker
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

        PlayerInputEvents.Instance.workstationTapAction = FlatTopTapEvent;

        for(int i = 0; i < FlatTopCooker.cookingIngredients.Length; i++)
        {
            if (FlatTopCooker.cookingIngredients[i] != null)
            {
                PlacePatty(i, FlatTopCooker.cookingIngredients[i].stepsComplete);
            }
        }
    }

    private void OnEnable()
    {
        FlatTopCooker.startCooking += FlatTop_StartCooking;
        FlatTopCooker.stillCooking += FlatTop_StillCooking;
        FlatTopCooker.endCooking += FlatTop_EndCooking;
    }

    private void OnDisable()
    {
        FlatTopCooker.startCooking -= FlatTop_StartCooking;
        FlatTopCooker.stillCooking -= FlatTop_StillCooking;
        FlatTopCooker.endCooking -= FlatTop_EndCooking;
    }

    private void Update()
    {
        for (int i = 0; i < FlatTopCooker.cookingIngredients.Length; i++)
        {
            CookingStation.CookingIngredient ingredient = FlatTopCooker.cookingIngredients[i];
            if (ingredient != null && SwitchPatty(i))
            {
                PlacePatty(i, ingredient.stepsComplete);
            }
        }
    }

    private void FlatTopTapEvent(RaycastHit hit)
    {
        if (hit.collider.gameObject.name.Contains("CookSelection"))
        {
            if(FlatTopCooker.TryCookingIngredient(out int index))
            {

                grillAudio.Play();

                PlacePatty(index, 0);
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

                CookingStation.CookingIngredient pressedIngredient = FlatTopCooker.cookingIngredients[val];
                if (pressedIngredient != null)
                {
                    if (pressedIngredient.stepTime >= 1.0f)
                    {
                        if (pressedIngredient.stepsComplete < pressedIngredient.steps)
                        {
                            FlatTopCooker.CookingIngredient_NextStep(pressedIngredient);
                        }
                        else
                        {
                            FlatTopCooker.PassIngredient(pressedIngredient);
                            RemovePatty(val);
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

    private void FlatTop_StartCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        // Spawn and attach cooking timer UI to spawn point transform
        GameObject newTimer = Instantiate(timerUIPrefab, spawnPoint);
        newTimer.transform.localScale *= 0.25f;
        newTimer.transform.SetPositionAndRotation(spawnPoint.position + (Vector3.up * 0.2f), Quaternion.Euler(45, 180, 0));
        newTimer.transform.Find("TimerUI").gameObject.GetComponent<Slider>().value = 0;
    }

    private void FlatTop_StillCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        if (spawnPoint.Find("TimerUICanvas(Clone)") != null)
        {
            // Update the cooking timer UI
            Slider mySlider = spawnPoint.Find("TimerUICanvas(Clone)").Find("TimerUI").gameObject.GetComponent<Slider>();
            mySlider.value = Mathf.Lerp(mySlider.minValue, mySlider.maxValue, Mathf.InverseLerp(0, 2.0f, cookingIngredient.stepTime));
        }
    }

    private void FlatTop_EndCooking(CookingStation.CookingIngredient cookingIngredient)
    {
        Transform spawnPoint = GetSpawnPoint(cookingIngredient);

        // Clear cooking timer UI
        if (spawnPoint.Find("TimerUICanvas(Clone)") != null)
        {
            Destroy(spawnPoint.Find("TimerUICanvas(Clone)").gameObject);
        }
    }

    private void PlacePatty(int index, int step)
    {
        GameObject patty = Instantiate(step == 0 ? uncookedPattyPrefab : (step == 1 ? halfCookedPattyPrefab : cookedPattyPrefab), flatTop_SpawnPoints[index]);
        patty.transform.localScale = step == 0 ? new Vector3(20, 20, 20) : new Vector3(5, 5, 5);
    }

    private bool SwitchPatty(int index)
    {
        if (flatTop_SpawnPoints[index].Find("Uncooked Patty(Clone)") != null && FlatTopCooker.cookingIngredients[index] != null && FlatTopCooker.cookingIngredients[index].stepsComplete != 0)
        {
            Destroy(flatTop_SpawnPoints[index].Find("Uncooked Patty(Clone)").gameObject);
            return true;
        }

        if (flatTop_SpawnPoints[index].Find("Halfcooked Patty(Clone)") != null && FlatTopCooker.cookingIngredients[index] != null && FlatTopCooker.cookingIngredients[index].stepsComplete != 1)
        {
            Destroy(flatTop_SpawnPoints[index].Find("Halfcooked Patty(Clone)").gameObject);
            return true;
        }

        if (flatTop_SpawnPoints[index].Find("Patty(Clone)") != null && FlatTopCooker.cookingIngredients[index] != null && FlatTopCooker.cookingIngredients[index].stepsComplete != 2)
        {
            Destroy(flatTop_SpawnPoints[index].Find("Patty(Clone)").gameObject);
            return true;
        }

        return false;
    }

    private void RemovePatty(int index)
    {
        GameObject anim;

        completionAudio.Play();

        anim = Instantiate(playAnim, flatTop_SpawnPoints[index]);
        anim.transform.localScale = new Vector3(12f, 12f, 12f);
        anim.transform.SetPositionAndRotation(flatTop_SpawnPoints[index].position + (Vector3.up * 0.2f), Quaternion.Euler(45, 180, 0));

        Destroy(flatTop_SpawnPoints[index].Find("Patty(Clone)").gameObject);

        Destroy(anim, 1.1f);

    }

    private Transform GetSpawnPoint(CookingStation.CookingIngredient cookingIngredient)
    {
        return flatTop_SpawnPoints[Array.FindIndex(FlatTopCooker.cookingIngredients, ingredient => ingredient == cookingIngredient)];
        // returns the transform associated with the provided CookingIngredient
    }
}
