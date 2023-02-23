using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssemblyInterface : WorkStationInterface
{
    // VARIABLES
    [SerializeField]
    private GameObject topBunPrefab;
    [SerializeField]
    private GameObject bottomBunPrefab;
    [SerializeField]
    private GameObject lettucePrefab;
    [SerializeField]
    private GameObject cookedPattyPrefab;

    // FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

    }

    public void Drop(int ingredientEnum)
    {
        GameObject droppingIngredient
    }
}
