using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : MonoBehaviour
{
    private string acceptedIngredientType;

    public void PrepCooking(string ingredientType)
    {
        acceptedIngredientType = ingredientType;
    }
}
