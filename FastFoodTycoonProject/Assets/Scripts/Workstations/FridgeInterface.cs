using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FridgeInterface : WorkStationInterface
{
    [SerializeField]
    private GameObject layoutGroup;
    [SerializeField]
    private GameObject ingredientButtonPrefab;
    [SerializeField]
    private Text ingredientName;
    [SerializeField]
    private Image ingredientImage;
    [SerializeField]
    private Text amountText;
    [SerializeField]
    private Slider amountSlider;
    [SerializeField]
    private Button withdrawButton;

    protected override void Awake()
    {
        base.Awake();

        SetupAmountSlider();
        SetupIngredientButtons();
    }

    private void Update()
    {
        amountText.text = amountSlider.value.ToString();
    }

    public UnityAction IngredientButtonPressed(Ingredient ingredient)
    {
        string name;
        
        switch (ingredient)
        {
            case Ingredient.TopBun:
                name = "Top Bun";
                break;
            case Ingredient.BottomBun:
                name = "Bottom Bun";
                break;
            case Ingredient.RawPatty:
                name = "Raw Patty";
                break;
            case Ingredient.Lettuce:
                name = "Lettuce";
                break;
            case Ingredient.RawFries:
                name = "Raw Fries";
                break;
            case Ingredient.Soda:
                name = "Soda";
                break;
            default:
                name = "Invalid Ingredient";
                break;
        }

        return new UnityAction(() =>
        {
            ingredientName.text = name;
            SetIngredientImage(ingredient);

            SetupAmountSlider(ingredient);

            SetWithdrawButtonListener(ingredient);
        });
    }

    private void SetupIngredientButtons()
    {
        foreach (KeyValuePair<Ingredient, int> kvp in workStation.ingredients)
        {
            if (workStation.ingredients[kvp.Key] > 0)
            {
                GameObject newButton = Instantiate(ingredientButtonPrefab, layoutGroup.transform);
                newButton.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = IngredientInfoManager.Instance.GetInfo(kvp.Key).ingredientSprite;
                newButton.GetComponent<Button>().onClick.AddListener(IngredientButtonPressed(kvp.Key));
            }

        }
    }

    private void SetIngredientImage(Ingredient ingredient)
    {
        ingredientImage.sprite = IngredientInfoManager.Instance.GetInfo(ingredient).ingredientSprite;
    }

    private void SetupAmountSlider()
    {
        amountSlider.minValue = 0;
        amountSlider.maxValue = 0;
        amountSlider.value = 0;
    }

    private void SetupAmountSlider(Ingredient ingredient)
    {
        amountSlider.minValue = 1;
        amountSlider.maxValue = workStation.ingredients[ingredient];
        amountSlider.value = 1;
    }

    private void SetWithdrawButtonListener(Ingredient ingredient)
    {
        withdrawButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.AddListener(new UnityAction(() =>
        {
            if (amountSlider.value <= 0)
            {
                Debug.Log("Cannot withdraw 0 ingredients");
                return;
            }
            
            if (workStation.WithdrawIngredient(ingredient, (int) amountSlider.value))
            {
                Worker.player.HoldIngredient(ingredient, (int) amountSlider.value);
                //SetupAmountSlider(ingredient);
                Worker.player.targetStation.UnloadStationScene();
            }
        }));
    }
}
