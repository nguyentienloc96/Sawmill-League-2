using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public GameObject effectAddOutput;
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
        lsLocation.Add(location);
        location.LoadLocation();
        location.lsWorking[0].animLock.enabled = true;
        if (isStart)
        {
            UIManager.Instance.txtRevenue.text = "Revenue : 0$/day";
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
            GameManager.Instance.AddDollar(+dollarBonus);

        }
    }

    public void AddDollar(double dollarBonus)
    {
        dollar += dollarBonus;
        if(dollar < 0)
        {
            dollar = 0;
        }

        if (dollar >= lsLocation[lsLocation.Count -1].lsWorking[lsLocation[lsLocation.Count - 1].countType + 1].price)
        {
            lsLocation[lsLocation.Count - 1].lsWorking[lsLocation[lsLocation.Count - 1].countType + 1].animLock.enabled = true;
        }
        else
        {
            lsLocation[lsLocation.Count - 1].lsWorking[lsLocation[lsLocation.Count - 1].countType + 1].animLock.enabled = false;
        }
    }

    public void AddOutPut(double numberAddOutput,Sprite icon,Vector3 startMove,Vector3 endMove)
    {
        effectAddOutput.SetActive(true);
        effectAddOutput.transform.position = startMove;
        effectAddOutput.GetComponent<TextMesh>().text = "+" + numberAddOutput;
        effectAddOutput.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        effectAddOutput.transform.DOMove(endMove, 0.5f).OnComplete(() =>
        {
            effectAddOutput.SetActive(false);
        });
    }
}
