using System.Collections.Generic;
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
    }

    [Header("InfoPlayer")]
    public Text txtDollar;
    public Text txtGold;

    [Header("Setting")]
    private bool isOnSetting;
    public GameObject panelSetting;

    [Header("ObjectMain")]
    public GameObject worldManager;
    public GameObject locationManager;

    [Header("Location")]
    public List<LocationUI> lsLocationUI;
    public List<Button> lsBtnLocationUI;

    [Header("BuildWork")]
    public GameObject JobSell;
    public GameObject JobUpgrade;
    public GameObject TruckUpgrade;

    [Header("MaskUI")]
    public Mask MaskWorld;
    public List<Mask> lsMaskLocation;

    [Header("Notifications")]
    public GameObject PopupNotification;

    [Header("Json")]
    public bool isSaveJson;

    [Header("SceneManager")]
    public TypeScene scene;
    public bool isContinue;

    public void Update()
    {
        txtDollar.text = ConvertNumber(GameManager.Instance.dollar);
        txtGold.text = ConvertNumber(GameManager.Instance.gold);
    }


    public void BtnSettingOnclick()
    {
        isOnSetting = !isOnSetting;
        panelSetting.SetActive(isOnSetting);
    }
    public void BtnPlayOnclick()
    {
        isContinue = false;

        AudioManager.Instance.Play("Click");
        AudioManager.Instance.Stop("Menu");
        AudioManager.Instance.Play("GamePlay");

        ScenesManager.Instance.isNextScene = false;

        GameManager.Instance.gold = 50000;
        GameManager.Instance.dollar = 10;
        GameManager.Instance.CreatLocation(lsLocationUI[0]);
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
        {
            isSaveJson = true;
            scene = TypeScene.WOLRD;
        });
    }
    public void BtnContinueOnclick()
    {
        isContinue = true;

        AudioManager.Instance.Play("Click");
        AudioManager.Instance.Stop("Menu");
        AudioManager.Instance.Play("GamePlay");

        DataPlayer.Instance.LoadDataPlayer();
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
        {
            isSaveJson = true;
            scene = TypeScene.WOLRD;
        });
    }
    public void BtnBackToWorld()
    {
        scene = TypeScene.WOLRD;
        AudioManager.Instance.Play("Click");
        locationManager.transform.SetAsFirstSibling();
        panelSetting.SetActive(false);
    }


    public void ConTrollMask(bool isOn)
    {
        MaskWorld.enabled = isOn;
        for (int i = 0; i < lsMaskLocation.Count; i++)
        {
            lsMaskLocation[i].enabled = isOn;
        }
    }
    public void PushNotification(string str)
    {
        PopupNotification.SetActive(true);
        PopupNotification.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
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
                smoney = smoney + "KB";
                smoney = smoney.Remove(smoney.Length - 4, 1);
                smoney = smoney.Insert(smoney.Length - 3, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 16);
                smoney = smoney + "KB";
            }
        }
        return smoney;
    }


    public void YesUpgradeJob()
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkST(indexType);
    }
    public void NoUpgradeJob()
    {
        JobUpgrade.SetActive(false);
    }


    public void YesUpgradeTruck()
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTruck(indexType);
    }
    public void NoUpgradeTruck()
    {
        TruckUpgrade.SetActive(false);
    }


    public void YesSellJob()
    {
        int id = GameManager.Instance.IDLocation;
        GameManager.Instance.lsLocation[id].SellJob();
    }
    public void NoSellJob()
    {
        JobSell.SetActive(false);
    }


    public void CloseJob()
    {
        scene = TypeScene.LOCATION;
        ConTrollMask(true);
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        GameManager.Instance.lsLocation[id].lsWorking[indexType].isNotAuto = true;
        GameManager.Instance.lsMiniGame[indexType].SetActive(false);
    }
}
