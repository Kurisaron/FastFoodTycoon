using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerSpeechBubble : MonoBehaviour
{
    private Worker worker;
    [SerializeField]
    private Image ingredientImage;

    public Worker Worker
    {
        get
        {
            return worker;
        }
        set
        {
            worker = value;
        }
    }
    
    private void Update()
    {
        if (gameObject.activeInHierarchy && worker != null)
        {
            GetComponent<RectTransform>().position = worker.gameObject.transform.position + (Vector3.up * 1.5f) + (Vector3.back * 1.5f);
        }
    }

    public void SetImage(Ingredient ingredient)
    {
        ingredientImage.sprite = IngredientInfoManager.Instance.GetInfo(ingredient).ingredientSprite;
    }
}
