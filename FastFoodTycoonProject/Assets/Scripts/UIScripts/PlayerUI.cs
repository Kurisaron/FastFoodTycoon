using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject recipeBookUI;
    
    //add functions that have to do with displaying player data here
    
    public void ViewRecipeBook() //when you click the recipe book button, set it active
    {
        Debug.Log("this is running");
        GameManager.Instance.stationOpened = true;
        recipeBookUI.SetActive(true);
    }
}
