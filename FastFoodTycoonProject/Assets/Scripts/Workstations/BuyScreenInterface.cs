using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyScreenInterface : WorkStationInterface
{
    // VARIABLES
    [SerializeField]
    private GameObject[] tabWindows;

    // Ingredient Tab
    [SerializeField]
    private IngredientPage ingredientPage;

    // Appliance Tab
    [SerializeField]
    private AppliancePage appliancePage;

    // Employee Tab
    [SerializeField]
    private EmployeePage employeePage;

    [SerializeField]
    private Button exitButton;

    // FUNCTIONS
    protected override void Awake()
    {
        //base.Awake();

        ChangeTab(0);
        ingredientPage.SetIngredientInfoPage(1);
        exitButton.onClick.AddListener(GameManager.Instance.CloseBuyMenu);
    }

    private void Update()
    {
        ingredientPage.UpdateAmountText();
        ingredientPage.UpdateStorageText();
    }

    public void ChangeTab(int index)
    {
        foreach (GameObject window in tabWindows)
        {
            window.SetActive(false);
        }

        tabWindows[index].SetActive(true);
    }

    public void SetIngredientInfoPage(int ingredientEnum)
    {
        ingredientPage.SetIngredientInfoPage(ingredientEnum);
    }

    // CLASSES
    [Serializable]
    public class IngredientPage
    {
        private Ingredient ingredient;
        [SerializeField]
        private Image ingredientImage;
        [SerializeField]
        private Text ingredientName;
        [SerializeField]
        private Text ingredientCostText;
        [SerializeField]
        private Text ingredientBulkText;
        [SerializeField]
        private Text ingredientStorageText;
        [SerializeField]
        private Text storageSpaceText;
        [SerializeField]
        private Text buyAmountText;
        [SerializeField]
        private Slider buyAmountSlider;
        [SerializeField]
        private Button buyButton;

        public void SetIngredientInfoPage(int ingredientEnum)
        {
            ingredient = (Ingredient)ingredientEnum;
            IngredientInfoManager.IngredientInfo ingredientInfo = IngredientInfoManager.Instance.GetInfo(ingredient);
            ingredientImage.sprite = ingredientInfo.ingredientSprite;
            ingredientName.text = ingredientInfo.ingredientName;
            ingredientCostText.text = "Cost: " + ingredientInfo.costPerUnit.ToString();
            ingredientBulkText.text = "Bulk: " + ingredientInfo.bulkAmount.ToString() + " for $" + ingredientInfo.bulkCost.ToString();
            SetAmountSlider(ingredientInfo);
            SetBuyButton(ingredientInfo);
        }

        private void SetAmountSlider(IngredientInfoManager.IngredientInfo ingredientInfo)
        {
            buyAmountSlider.minValue = 1;
            buyAmountSlider.maxValue = ingredientInfo.bulkAmount;
            buyAmountSlider.value = 1;
        }

        public void UpdateAmountText()
        {
            IngredientInfoManager.IngredientInfo ingredientInfo = IngredientInfoManager.Instance.GetInfo(ingredient);
            float cost = Mathf.Lerp(ingredientInfo.costPerUnit, ingredientInfo.bulkCost, Mathf.InverseLerp(1, ingredientInfo.bulkAmount, buyAmountSlider.value));
            buyAmountText.text = buyAmountSlider.value.ToString() + " " + ingredientInfo.ingredientName + "(s) for $" + cost.ToString();
        }

        public void UpdateStorageText()
        {
            ingredientStorageText.text = "You have " + (WorkStation.fridge1.ingredients[ingredient] + WorkStation.fridge2.ingredients[ingredient]).ToString() + " " + IngredientInfoManager.Instance.GetInfo(ingredient).ingredientName + "(s)";
        }

        private void SetBuyButton(IngredientInfoManager.IngredientInfo ingredientInfo)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(GetBuyAction(ingredientInfo));
        }

        private UnityAction GetBuyAction(IngredientInfoManager.IngredientInfo ingredientInfo)
        {
            return new UnityAction(() =>
            {
                // Run code here upon pressing the buy button
                float cost = Mathf.Lerp(ingredientInfo.costPerUnit, ingredientInfo.bulkCost, Mathf.InverseLerp(1, ingredientInfo.bulkAmount, buyAmountSlider.value));
                if (GameManager.Instance.SpendMoney(cost))
                {
                    if (WorkStation.fridge1.ingredients[ingredientInfo.ingredient] >= WorkStation.fridge1.ingredients[ingredientInfo.ingredient] && WorkStation.fridge1.DepositIngredient(ingredientInfo.ingredient, (int) buyAmountSlider.value))
                    {
                        Debug.Log("You spent $" + cost.ToString() + " to add " + buyAmountSlider.value.ToString() + " " + ingredientInfo.ingredientName + (buyAmountSlider.value > 1 ? "" : "s") + " to Fridge 1");
                    }
                    else if (WorkStation.fridge2.DepositIngredient(ingredientInfo.ingredient, (int)buyAmountSlider.value))
                    {
                        Debug.Log("You spent $" + cost.ToString() + " to add " + buyAmountSlider.value.ToString() + " " + ingredientInfo.ingredientName + (buyAmountSlider.value > 1 ? "" : "s") + " to Fridge 2");
                    }
                    else
                    {
                        Debug.LogError("Could not deposit ingredient in either fridge");
                    }
                }

            });
        }
    }

    [Serializable]
    public class AppliancePage
    {
        
    }

    [Serializable]
    public class EmployeePage
    {

    }
}
