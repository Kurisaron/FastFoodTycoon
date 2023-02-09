using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientPrices : Singleton<IngredientPrices>
{
    // VARIABLES
    public List<PriceInfo> priceInformation;

    // FUNCTIONS
    public override void Awake()
    {
        base.Awake();

    }

    // CLASSES
    [Serializable]
    public class PriceInfo
    {
        // VARIABLES
        public Ingredient ingredient;
        [Tooltip("Cost of buying 1 unit of this ingredient")]
        public float costPerUnit;
        [Tooltip("Units of ingredient received from buying in bulk")]
        public int bulkAmount;
        [Tooltip("Cost of ingredient when buying in bulk")]
        public float bulkCost;
    }
}
