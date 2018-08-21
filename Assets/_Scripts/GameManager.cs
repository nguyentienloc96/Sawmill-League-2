using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("DateTime")]
    public DateTime dateGame;
    private float time;

    public float DeltaTimeGame = 4;

    public List<Text> listDate;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        LoadDate();
    }

    public void LoadDate()
    {
        dateGame = DateTime.Now;
        SetDate();
    }

    public void SetDate()
    {
        string daystring = dateGame.Day.ToString("00");
        listDate[0].text = daystring;

        string monthstring = dateGame.Month.ToString("00");
        listDate[1].text = monthstring;

        string yearstring = dateGame.Year.ToString("0000");
        listDate[2].text = yearstring;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= DeltaTimeGame)
        {
            int month = dateGame.Month;
            int year = dateGame.Year;
            dateGame = dateGame.AddDays(1f);
            SetDate();
            UpdateGame();
            time = 0;
        }
    }

    void UpdateGame()
    {

    }
}
