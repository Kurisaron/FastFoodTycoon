using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // VARIABLES
    [SerializeField]
    private GameObject openBuyMenuButton;
    [HideInInspector]
    public GameData gameData;

    // FUNCTIONS
    public override void Awake()
    {
        base.Awake();

        gameData = gameObject.AddComponent<GameData>();
        gameData.money = 10000.0f;
    }

    public bool SpendMoney(float cost)
    {
        if (cost > GameManager.Instance.gameData.money)
        {
            Debug.Log("You don't have enough money!");
            return false;
        }
        else
        {
            gameData.money -= cost;
            return true;
        }
    }

    public void OpenBuyMenu()
    {
        PlayerInputEvents.Instance.canOpenStation = false;
        SceneManager.LoadScene("BuyScreenScene", LoadSceneMode.Additive);
        openBuyMenuButton.SetActive(false);
    }

    public void CloseBuyMenu()
    {
        SceneManager.UnloadSceneAsync("BuyScreenScene");
        openBuyMenuButton.SetActive(true);
    }
}
