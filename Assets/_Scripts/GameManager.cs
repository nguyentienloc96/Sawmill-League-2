using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    #region DateTime
    [Header("DateTime")]
    public DateTime dateGame;
    public DateTime dateStartPlay;
    public float deltaTimeGame = 4;
    public List<Text> listDate;
    private float time;
    #endregion

    #region GamePlay
    [Header("GamePlay")]
    public long dollar = 0;
    public long gold = 0;
    public int IDLocation;
    public List<Location> lsLocation;
    #endregion

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
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

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= deltaTimeGame)
        {
            int month = dateGame.Month;
            int year = dateGame.Year;
            dateGame = dateGame.AddDays(1f);
            SetDate();
            UpdateGame();
            time = 0;
        }
    }

    public void UpdateGame()
    {

    }

    public void LoadLocation()
    {
        lsLocation[IDLocation].transform.localPosition = Vector3.zero;
        for (int i = 0; i < lsLocation.Count; i++)
        {
            if (i != IDLocation)
            {
                lsLocation[i].transform.localPosition = new Vector3(720f, 0f, 0f);
            }
        }
    }
}
