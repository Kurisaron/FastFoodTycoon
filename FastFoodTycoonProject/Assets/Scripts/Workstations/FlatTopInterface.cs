using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlatTopInterface : WorkStationInterface
{
    // VARIABLES
    public GameObject uncookedPattyPrefab;
    public GameObject halfCookedPattyPrefab;
    public GameObject cookedPattyPrefab;
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

        completionAudio.Play();

        Destroy(flatTop_SpawnPoints[index].Find("Patty(Clone)").gameObject);
    }
}
