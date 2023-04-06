using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // VARIABLES ==================================================
    public bool gameActive; // false = game is between days or paused

    public GameObject openBuyMenuButton;
    public Text countdownTest;

    [HideInInspector]
    public GameData gameData;
    [HideInInspector]
    public DayManager dayManager;
    [HideInInspector]
    public SaveManager saveManager;

    [HideInInspector]
    public bool stationOpened;



    // UNITY FUNCTIONS ============================================
    public override void Awake()
    {
        base.Awake();

        gameData = gameObject.AddComponent<GameData>();
        dayManager = gameObject.AddComponent<DayManager>();
        saveManager = gameObject.AddComponent<SaveManager>();

        gameData.money = 10000.0f;
        gameData.NewGameData();
        stationOpened = false;

        gameActive = true;
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
        if (!gameActive) gameActive = !gameActive;
    }

    // SAVE/LOAD ==================================================
    private void DoSave()
    {
        saveManager.SaveData(gameData);
    }

    // DAYS ==================================================
    public void SetCountdownText(float secondsLeft)
    {
        countdownTest.text = secondsLeft.ToString("F1") + "s";
    }

    public void EndDay()
    {
        if (stationOpened) Worker.player.targetStation.UnloadStationScene();
        gameData.dayData.day += 1;
        StartCoroutine(EndDayRoutine());
    }

    private IEnumerator EndDayRoutine()
    {
        // TO-DO: Display end of day stats (money, day, etc)

        yield return new WaitForSeconds(3.0f);

        // TO-DO: Clear end of day stats
        
        OpenBuyMenu();
    }
}
