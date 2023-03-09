using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // VARIABLES ==================================================
    public GameObject openBuyMenuButton;
    [HideInInspector]
    public GameData gameData;
    [HideInInspector]
    public SaveManager saveManager;
    [HideInInspector]
    public bool stationOpened;
    [HideInInspector]
    public bool restaurantOpen;



    // UNITY FUNCTIONS ============================================
    public override void Awake()
    {
        base.Awake();

        gameData = gameObject.AddComponent<GameData>();
        saveManager = gameObject.AddComponent<SaveManager>();

        gameData.money = 10000.0f;
        gameData.NewGameData();
        stationOpened = false;
        restaurantOpen = false;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) return;

        DoSave();
    }

    private void OnApplicationQuit()
    {
        DoSave();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;

        DoSave();
    }

    // MONEY =====================================================
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

    public void EarnMoney(float profit)
    {
        gameData.money += profit;
    }

    // BUY MENU ==================================================
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

    // SAVE/LOAD ==================================================
    private void DoSave()
    {
        saveManager.SaveData(gameData);
    }
}
