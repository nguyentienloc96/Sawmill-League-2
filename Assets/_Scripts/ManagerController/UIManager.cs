﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeScene
{
    LOADING,
    HOME,
    WOLRD,
    LOCATION,
    MINIGAME,
    SETTING,
}

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public void Start()
    {
        scene = TypeScene.HOME;
        if (!PlayerPrefs.HasKey("Continue"))
        {
            PlayerPrefs.SetInt("Continue", 0);
            btncontinue.interactable = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("Continue") == 0)
            {
                btncontinue.interactable = false;
            }
            else
            {
                btncontinue.interactable = true;
            }
        }
    }

    [Header("InfoPlayer")]
    public Text txtDollar;
    public Text txtGold;
    public GameObject panelDollar;
    public GameObject panelGold;

    [Header("Setting")]
    public GameObject panelSetting;

    [Header("ObjectMain")]
    public GameObject worldManager;
    public GameObject locationManager;
    public RectTransform contentWorld;
    public Transform handWorld;

    [Header("Location")]
    public List<LocationUI> lsLocationUI;
    public List<Button> lsBtnLocationUI;

    [Header("BuildWork")]
    public GameObject JobSell;
    public GameObject JobUpgrade;
    public GameObject TruckUpgrade;
    public Image[] arrXJob;
    public Image[] arrXTrunk;
    public bool isJobX10;
    public bool isTrunkX10;

    [Header("PopupOther")]
    public GameObject PopupGiveGold;
    public GameObject PopupAutoPlant;

    [Header("Json")]
    public bool isSaveJson;

    [Header("SceneManager")]
    public TypeScene scene;
    public bool isContinue;

    [Header("MiniGame")]
    public Sprite[] spTree;
    public Sprite[] spBG;

    [Header("UIHome")]
    public Button btncontinue;
    public GameObject popupStart;
    public bool isClick;

    [Header("SellJob")]
    public Button btnSell;
    public Button btnUpgradeJob;
    public Button btnUpgradeTrunk;


    public void Update()
    {
        txtDollar.text = ConvertNumber(GameManager.Instance.dollar);
        txtGold.text = ConvertNumber(GameManager.Instance.gold);
        if (scene == TypeScene.MINIGAME)
        {
            int id = GameManager.Instance.IDLocation;
            int indexType = GameManager.Instance.lsLocation[id].indexType;
            int indexTypeWork = GameManager.Instance.lsLocation[id].indexTypeWork;
            if (indexType != 0) GameManager.Instance.lsTypeMiniGame[indexTypeWork].lsMiniGame[indexType].inputMiniGame.text = ConvertNumber(GameManager.Instance.lsLocation[id].lsWorking[indexType].input);
            GameManager.Instance.lsTypeMiniGame[indexTypeWork].lsMiniGame[indexType].outputMiniGame.text = ConvertNumber(GameManager.Instance.lsLocation[id].lsWorking[indexType].output);

        }
    }


    public void BtnSettingOnclick()
    {
        if (!panelSetting.activeSelf)
            panelSetting.SetActive(true);
        else
            panelSetting.SetActive(false);
    }
    public void BtnPlayOnclick()
    {
        if (PlayerPrefs.GetInt("Continue") == 0)
        {
            BtnYesPlayOnclick();
        }
        else
        {
            popupStart.SetActive(true);
        }
    }
    public void BtnYesPlayOnclick()
    {
        if (!isClick)
        {
            isClick = true;
            isContinue = false;
            AudioManager.Instance.Play("Click");
            AudioManager.Instance.Stop("Menu", true);
            AudioManager.Instance.Play("GamePlay", true);
            popupStart.SetActive(false);
            ScenesManager.Instance.isNextScene = false;
			GameManager.Instance.sumHomeAll = 0;
            GameManager.Instance.dollar = GameConfig.Instance.dollarStart;
            GameManager.Instance.gold = GameConfig.Instance.goldStart;
            GameManager.Instance.ClearLocation();
            GameManager.Instance.CreatLocation(lsLocationUI[0],true);
            handWorld.position = lsLocationUI[0].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f);
            contentWorld.anchoredPosition = Vector3.zero;
            ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                isSaveJson = true;
                scene = TypeScene.WOLRD;
                isClick = false;
            });
        }
    }
    public void BtnContinueOnclick()
    {
        if (!isClick)
        {
            isClick = true;
            isContinue = true;

            AudioManager.Instance.Play("Click");
            AudioManager.Instance.Stop("Menu", true);
            AudioManager.Instance.Play("GamePlay", true);

            DataPlayer.Instance.LoadDataPlayer();
            contentWorld.anchoredPosition = Vector3.zero;

            ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                isSaveJson = true;
                scene = TypeScene.WOLRD;
                isClick = false;
            });
        }
    }
    public void BtnBackToWorld()
    {
        if (scene != TypeScene.WOLRD)
        {
            scene = TypeScene.WOLRD;
            AudioManager.Instance.Play("Click");
            locationManager.transform.SetAsFirstSibling();
            panelSetting.SetActive(false);
        }
        else
        {
            scene = TypeScene.HOME;
            AudioManager.Instance.Play("Menu", true);
            AudioManager.Instance.Stop("GamePlay", true);
            AudioManager.Instance.Play("Click");
            DataPlayer.Instance.SaveDataPlayer();
            panelSetting.SetActive(false);
            GameManager.Instance.ClearLocation();
            ScenesManager.Instance.secenes[0].objects.SetActive(true);
            ScenesManager.Instance.currentScenes = 0;
        }
    }

    public void PushGiveGold(string str)
    {
        PopupGiveGold.SetActive(true);
        PopupGiveGold.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
    }
    public string ConvertNumber(long number)
    {
        string smoney = string.Format("{0:n0}", number);

        if (smoney.Length >= 5 && smoney.Length < 9)
        {
            if (number % 1000 != 0)
            {
                smoney = smoney.Substring(0, smoney.Length - 2);
                smoney = smoney + "k";
                smoney = smoney.Remove(smoney.Length - 3, 1);
                smoney = smoney.Insert(smoney.Length - 2, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 4);
                smoney = smoney + "k";
            }
        }
        else if (smoney.Length >= 9 && smoney.Length < 13)
        {
            if (number % 1000000 != 0)
            {
                smoney = smoney.Substring(0, smoney.Length - 6);
                smoney = smoney + "M";
                smoney = smoney.Remove(smoney.Length - 3, 1);
                smoney = smoney.Insert(smoney.Length - 2, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 8);
                smoney = smoney + "M";
            }
        }
        else if (smoney.Length >= 13 && smoney.Length < 17)
        {
            if (number % 1000000000 != 0)
            {
                smoney = smoney.Substring(0, smoney.Length - 10);
                smoney = smoney + "B";
                smoney = smoney.Remove(smoney.Length - 3, 1);
                smoney = smoney.Insert(smoney.Length - 2, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 12);
                smoney = smoney + "B";
            }
        }
        else if (smoney.Length >= 17)
        {
            if (number % 1000000000000 != 0)
            {
                smoney = smoney.Substring(0, smoney.Length - 14);
                smoney = smoney + "kB";
                smoney = smoney.Remove(smoney.Length - 4, 1);
                smoney = smoney.Insert(smoney.Length - 3, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 16);
                smoney = smoney + "kB";
            }
        }
        return smoney;
    }

    public void UpgradeJobX10(bool x10)
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        isJobX10 = x10;
        if (!x10)
        {
            arrXJob[0].color = new Color32(255, 255, 255, 255);
            arrXJob[1].color = new Color32(255, 255, 255, 128);
            GameManager.Instance.lsLocation[id].CheckInfoTypeOfWorkST(indexType);
        }
        else
        {
            arrXJob[0].color = new Color32(255, 255, 255, 128);
            arrXJob[1].color = new Color32(255, 255, 255, 255);
            GameManager.Instance.lsLocation[id].CheckInfoTypeOfWorkSTX10(indexType);
        }
    }
    public void UpgradeTrunkX10(bool x10)
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        isTrunkX10 = x10;
        if (!x10)
        {
            arrXTrunk[0].color = new Color32(255, 255, 255, 255);
            arrXTrunk[1].color = new Color32(255, 255, 255, 128);
            GameManager.Instance.lsLocation[id].CheckInfoTruck(indexType);
        }
        else
        {
            arrXTrunk[0].color = new Color32(255, 255, 255, 128);
            arrXTrunk[1].color = new Color32(255, 255, 255, 255);
            GameManager.Instance.lsLocation[id].CheckInfoTruckX10(indexType);
        }
    }

    public void YesUpgradeJob()
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isJobX10)
            JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkST(indexType);
        else
            JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkSTX10(indexType);
    }
    public void NoUpgradeJob()
    {
        JobUpgrade.SetActive(false);
    }


    public void YesUpgradeTruck()
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isTrunkX10)
            TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTruck(indexType);
        else
            TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTruckX10(indexType);
    }
    public void NoUpgradeTruck()
    {
        TruckUpgrade.SetActive(false);
    }


    public void YesSellJob()
    {
        int id = GameManager.Instance.IDLocation;
        GameManager.Instance.lsLocation[id].SellJob();
        JobSell.SetActive(false);
    }
    public void NoSellJob()
    {
        JobSell.SetActive(false);
    }


    public void CloseJob()
    {
        scene = TypeScene.LOCATION;
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        GameManager.Instance.lsTypeMiniGame[GameManager.Instance.lsLocation[id].indexTypeWork].lsMiniGame[indexType].miniGame.SetActive(false);
        GameManager.Instance.lsLocation[id].lsWorking[indexType].isXJob = false;
    }

    public void ShowPanelDollar()
    {
        if (!panelDollar.activeSelf)
            panelDollar.SetActive(true);
        else
            panelDollar.SetActive(false);
    }

    public void ShowPanelGold()
    {
        if (!panelGold.activeSelf)
            panelGold.SetActive(true);
        else
            panelGold.SetActive(false);
    }

    public void ClosePanel(GameObject g)
    {
        if (g != null)
            g.SetActive(false);
    }

    public void SaveExit()
    {
        scene = TypeScene.HOME;
        AudioManager.Instance.Play("Menu", true);
        AudioManager.Instance.Stop("GamePlay", true);
        AudioManager.Instance.Play("Click");
        DataPlayer.Instance.SaveDataPlayer();
        panelSetting.SetActive(false);
        GameManager.Instance.ClearLocation();
        ScenesManager.Instance.secenes[0].objects.SetActive(true);
        ScenesManager.Instance.currentScenes = 0;
    }
}
