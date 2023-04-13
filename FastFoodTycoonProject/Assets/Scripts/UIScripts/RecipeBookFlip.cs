using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeBookFlip : MonoBehaviour
{
    [SerializeField] private List<Sprite> leftPageContent = new List<Sprite>();
    [SerializeField] private List<Sprite> rightPageContent = new List<Sprite>();

    [SerializeField] private GameObject leftPageContentObject;
    [SerializeField] private GameObject rightPageContentObject;

    private int currentListNumber = 0;

    private void Update()
    {
        leftPageContentObject.GetComponent<Image>().sprite = leftPageContent[currentListNumber];
        rightPageContentObject.GetComponent<Image>().sprite = rightPageContent[currentListNumber];
    }

    public void PageTurnLeft()
    {
        
        currentListNumber--;
        if(currentListNumber < 0)
        {
            currentListNumber = leftPageContent.Count - 1;
        }
        Debug.Log(currentListNumber);
    }

    public void PageTurnRight()
    {
        currentListNumber++;
        if(currentListNumber >= rightPageContent.Count)
        {
            currentListNumber = 0;
        }
    }

    public void TurnOffRecipeBook()
    {
        gameObject.SetActive(false);
    }
}
