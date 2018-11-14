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
    public double gold;
    public double dollar;
    public int sumHomeAll;
    public int indexSawmill;
    public int IDLocation;
    public Transform locationManager;
    public List<Location> lsLocation;
    public GameObject[] lsItemLocation;
    public GameObject[] arrPrefabOther;
    public GameObject[] arrPrefabsStreet;


    #endregion

    #region MiniGame
    public List<TypeMiniGame> lsTypeMiniGame;
    #endregion
    public bool isReset;
    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        LoadDate();
        if (isReset)
        {
            PlayerPrefs.DeleteAll();
        }
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
            time = 0;
        }
    }

    public void LoadLocation()
    {
        UIManager.Instance.txtRevenue.enabled = false;
        lsLocation[IDLocation].transform.localPosition = Vector3.zero;
        for (int animL = 0; animL < lsLocation[IDLocation].lsWorking.Length; animL++)
        {
            lsLocation[IDLocation].lsWorking[animL].anim.enabled = true;
        }
        for (int animR = 0; animR < lsLocation[IDLocation].rivers.arrAnim.Count; animR++)
        {
            lsLocation[IDLocation].rivers.arrAnim[animR].enabled = true;
        }
        for (int animO = 0; animO < lsLocation[IDLocation].others.arrAnim.Count; animO++)
        {
            lsLocation[IDLocation].others.arrAnim[animO].enabled = true;
        }
        for (int i = 0; i < lsLocation.Count; i++)
        {
            if (i != IDLocation)
            {
                lsLocation[i].transform.localPosition = new Vector3(3000f, 0f, 0f);
                for (int animLH = 0; animLH < lsLocation[i].lsWorking.Length; animLH++)
                {
                    lsLocation[i].lsWorking[animLH].anim.enabled = false;
                }
                for (int animRH = 0; animRH < lsLocation[i].rivers.arrAnim.Count; animRH++)
                {
                    lsLocation[i].rivers.arrAnim[animRH].enabled = false;
                }
                for (int animOH = 0; animOH < lsLocation[i].others.arrAnim.Count; animOH++)
                {
                    lsLocation[i].others.arrAnim[animOH].enabled = false;
                }
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

    public void CreatLocation(LocationUI locationUI, bool isStart = false)
    {
        GameObject objLocation = Instantiate(lsItemLocation[locationUI.indexTypeWork], locationManager);
        objLocation.name = locationUI.nameLocationUI;
        objLocation.transform.SetAsFirstSibling();
        objLocation.transform.localPosition = new Vector3(3000f, 0f, 0f);
        Location location = objLocation.GetComponent<Location>();
        location.id = locationUI.id;
        if (location.indexTypeWork == 0)
        {
            location.makerType = indexSawmill;
            indexSawmill++;
        }
        location.nameLocation = locationUI.nameLocationUI;
        UIManager.Instance.lsBtnLocationUI[location.id].interactable = true;
        location.LoadLocation();
        lsLocation.Add(location);
        if (isStart)
        {
            location.countType++;
            location.indexType = location.countType;
            location.lsWorking[location.countType].icon.color = Color.white;
            location.lsWorking[location.countType].info.SetActive(true);
            location.lsWorking[location.countType].textInput.text = UIManager.Instance.ConvertNumber(location.lsWorking[location.countType].input);
            location.lsWorking[location.countType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[location.countType].output);
            location.forest.btnAutoPlant.interactable = true;
            UIManager.Instance.txtRevenue.text
                       = "Revenue : " + UIManager.Instance.ConvertNumber(
                           GameConfig.Instance.c0
                           * GameConfig.Instance.r
                           * GameConfig.Instance.productCost
                           ) + "$/day";
            location.lsWorking[0].animLock.gameObject.SetActive(false);
            location.lsWorking[1].animLock.enabled = true;
        }
        else
        {
            location.lsWorking[0].animLock.enabled = true;
        }
    }

    public void BonusAds(double dollarBonus, double goldBonus)
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
