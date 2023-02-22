using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInfoManager : Singleton<IngredientInfoManager>
{
    // VARIABLES
    public List<IngredientInfo> priceInformation;

    // FUNCTIONS
    public override void Awake()
    {
        base.Awake();

    }

    public IngredientInfo GetInfo(Ingredient ingredient)
    {
        if (priceInformation.Exists(info => info.ingredient == ingredient))
        {
            return priceInformation.Find(info => info.ingredient == ingredient);
        }

        Debug.LogError("Ingredient " + ingredient.ToString() + " info did not match any in IngredientInfoManager");
        return priceInformation[0]; // return first information in list, to prevent null values
    }

    // CLASSES
    [Serializable]
    public class IngredientInfo
    {
        // VARIABLES
        public Ingredient ingredient;
        [Tooltip("Cost of buying 1 unit of this ingredient")]
        public float costPerUnit;
        [Tooltip("Units of ingredient received from buying in bulk")]
        public int bulkAmount;
        [Tooltip("Cost of ingredient when buying in bulk")]
        public float bulkCost;
        [Tooltip("Sprite for ingredient")]
        public Sprite ingredientSprite;
    }
}
