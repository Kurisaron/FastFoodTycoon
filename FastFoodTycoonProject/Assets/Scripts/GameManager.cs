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

    //Courtney
    
    //money counter
    public Text counterText;
    int counter = 0;
    //day counter
    public Text dayCounterText;
    int dayCounter = 0;
    //

    [HideInInspector]
    public GameData gameData;
    [HideInInspector]
    public DayManager dayManager;
    [HideInInspector]
    public SaveManager saveManager;

    [HideInInspector]
    public bool stationOpened;

    public float dayDuration;
    public Slider daySlider;

    [Header("End Day Screen")]
    public GameObject endDayScreen;
    public Text endDayTitle;
    public Text endDayDetails;

    // UNITY FUNCTIONS ============================================
    public override void Awake()
    {
        base.Awake();

        if (endDayScreen.activeInHierarchy) endDayScreen.SetActive(false);

        gameData = gameObject.AddComponent<GameData>();

        gameData.money = 10000.0f;
        gameData.moneyAtDayStart = gameData.money;
        gameData.NewGameData();

        dayManager = gameObject.AddComponent<DayManager>();
        dayManager.Init(dayDuration, daySlider);
        saveManager = gameObject.AddComponent<SaveManager>();

        stationOpened = false;

        gameActive = true;

        //courtney
        counterText.text = "Money: " + GameManager.Instance.gameData.money.ToString();
        dayCounterText.text = "Day: " + gameData.dayData.day.ToString();
        /*GameObject[] Customers = GameObject.FindGameObjectsWithTag("Customer");
        foreach (GameObject Customer in Customers)
            GameObject.Destroy(Customer);
        CustomerSpawnOne.Instance.gameObject.SetActive(true);*/
        //

    }

    private void Update()
    {
        counterText.text = "Money: " + GameManager.Instance.gameData.money.ToString();
        dayCounterText.text = "Day: " + gameData.dayData.day.ToString();
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
        //CustomerSpawnOne.Instance.gameObject.SetActive(true);
        if (!gameActive) gameActive = !gameActive;
        CustomerSpawnOne.Instance.SpawnNewCustomer();
        //Courtney
        //CustomerSpawnOne.Instance.gameObject.SetActive(true);
        //print("spawn customer");
        //
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
        StartCoroutine(EndDayRoutine());
        //Courtney
        CustomerController[] customers = FindObjectsOfType<CustomerController>();
        foreach(CustomerController customer in customers)
        {
            CustomerSpawnOne.Instance.customers.Remove(customer);
        }
        Destroy(GameObject.FindWithTag("Customer"));
        /*GameObject[] Customers = GameObject.FindGameObjectsWithTag("Customer");
        foreach (GameObject Customer in Customers)
        GameObject.Destroy(Customer);*/
        //CustomerSpawnOne.Instance.gameObject.SetActive(false);
        //
    }

    private IEnumerator EndDayRoutine()
    {
        // Display end of day stats (money, day, etc)
        endDayScreen.SetActive(true);
        if (gameData != null && gameData.dayData != null)
        {
            endDayTitle.text = "Day " + gameData.dayData.day.ToString() + " Ended";
            float profitAmount = gameData.money - gameData.moneyAtDayStart;
            endDayDetails.text = "Profit: " + (profitAmount >= 0 ? "" : "-") + "$" + Mathf.Abs(profitAmount).ToString("0.00");
        }
        else Debug.LogError("No game data or day data");

        yield return new WaitForSeconds(3.0f);

        // Clear end of day stats
        endDayScreen.SetActive(false);

        gameData.dayData.day += 1;
        gameData.moneyAtDayStart = gameData.money;
        OpenBuyMenu();
    }

    /*void update()
    {
        //counterText.text = counter.ToString();
    }*/
}
