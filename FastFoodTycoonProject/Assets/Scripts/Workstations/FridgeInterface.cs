using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FridgeInterface : WorkStationInterface
{
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

    public void IngredientButtonPressed(int ingredientIndex)
    {
        Ingredient ingredient = (Ingredient)ingredientIndex;
        switch (ingredient)
        {
            case Ingredient.TopBun: // 0
                ingredientName.text = "Top Bun";
                break;
            case Ingredient.BottomBun: // 1
                ingredientName.text = "Bottom Bun";
                break;
            case Ingredient.RawPatty: // 2
                ingredientName.text = "Raw Patty";
                break;
            case Ingredient.Lettuce: // 4
                ingredientName.text = "Lettuce";
                break;
            case Ingredient.RawFries: // 5
                ingredientName.text = "Raw Fries";
                break;
            case Ingredient.Soda: // 7
                ingredientName.text = "Soda";
                break;
            default:
                break;
        }

        amountSlider.value = 0;
    }
}
