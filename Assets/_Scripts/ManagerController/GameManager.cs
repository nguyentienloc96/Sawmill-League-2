using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MiniGame
{
    public string name;
    public Text inputMiniGame;
    public Text outputMiniGame;
    public GameObject miniGame;
}

[System.Serializable]
public struct TypeMiniGame
{
    public string name;
    public List<MiniGame> lsMiniGame;
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    #region DateTime
    [Header("DateTime")]
    public DateTime dateGame;
    public DateTime dateStartPlay;
    public List<Text> listDate;
    private float time;
    #endregion

    #region GamePlay
    [Header("GamePlay")]
    public long gold = 0;
    public long dollar = 0;
    public long dollarGive = 0;


    public int IDLocation;
    public Transform locationManager;
    public List<Location> lsLocation;
    public GameObject[] lsItemLocation;
    #endregion

    #region MiniGame
    public List<TypeMiniGame> lsTypeMiniGame;
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
        if (time >= GameConfig.Instance.p0Time)
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
                lsLocation[i].transform.localPosition = new Vector3(3000f, 0f, 0f);
            }
        }
    }

    public void ClearLocation()
    {
        for (int i = 0; i < lsLocation.Count; i++)
        {
            UIManager.Instance.lsBtnLocationUI[lsLocation[i].id].interactable = false;
            Destroy(lsLocation[i].gameObject);
        }
        lsLocation.Clear();
    }

    public void CreatLocation(LocationUI locationUI)
    {
        GameObject objLocation = Instantiate(lsItemLocation[locationUI.indexTypeWork], locationManager);
        objLocation.name = locationUI.nameLocationUI;
        objLocation.transform.SetAsFirstSibling();
        objLocation.transform.localPosition = new Vector3(3000f, 0f, 0f);
        Location location = objLocation.GetComponent<Location>();
        location.id = locationUI.id;
        location.nameLocation = locationUI.nameLocationUI;
        UIManager.Instance.lsBtnLocationUI[location.id].interactable = true;
        location.LoadLocation();
        lsLocation.Add(location);
    }

    public void BonusAds(long dollarBonus, long goldBonus)
    {
        if (goldBonus > 0)
        {
            gold += goldBonus;
        }
        if (dollarBonus > 0)
        {
            dollar += dollarBonus;
        }
    }
}
