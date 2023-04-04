using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkStationInterface : WorkStationInterface
{
    // VARIABLES
    public GameObject emptyCupPrefab;
    public Transform[] drink_SpawnPoints;

    // PROPERTIES
    private CookingStation DrinkCooker
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

        PlayerInputEvents.Instance.workstationTapAction = DrinkStationTapEvent;

        for (int i = 0; i < DrinkCooker.cookingIngredients.Length; i++)
        {
            if (DrinkCooker.cookingIngredients[i] != null)
            {
                DrinkStep(i, DrinkCooker.cookingIngredients[i].stepsComplete);
            }
        }
    }

    private void Update()
    {
        
    }

    private void DrinkStationTapEvent(RaycastHit hit)
    {
        if (hit.collider.gameObject.name.Contains("CookSelection"))
        {
            if (DrinkCooker.TryCookingIngredient(out int index))
            {
                DrinkStep(index, 0);
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

                CookingStation.CookingIngredient pressedIngredient = DrinkCooker.cookingIngredients[val];
                if (pressedIngredient != null)
                {
                    if (pressedIngredient.stepTime >= 1.0f)
                    {
                        if (pressedIngredient.stepsComplete < pressedIngredient.steps)
                        {
                            DrinkCooker.CookingIngredient_NextStep(pressedIngredient);
                            DrinkStep(val, 1);
                        }
                        else
                        {
                            DrinkCooker.PassIngredient(pressedIngredient);
                            DrinkStep(val, 2);
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

    private void DrinkStep(int index, int step)
    {
        Transform spawnPoint = drink_SpawnPoints[index];

        switch (step)
        {
            case 0:
                // First step (none complete): add a drink cup and 'fill' it
                GameObject drinkCup = Instantiate(emptyCupPrefab, spawnPoint);
                drinkCup.transform.localPosition = drinkCup.transform.up * -0.088f;
                drinkCup.transform.localScale = new Vector3(0.02943999f, 0.04252196f, 0.07642627f);

                // TO-DO: Add pour stream
                break;
            case 1:
                // Second step (one complete): put lid on drink cup
                break;
            case 2:
                // Final step (two complete): pass to order building
                break;
        }
    }
}
