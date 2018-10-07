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
    public GameObject panelDollar;
    public GameObject panelGold;

    [Header("Setting")]
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

    [Header("Notifications")]
    public GameObject PopupNotification;

    [Header("Json")]
    public bool isSaveJson;

    [Header("SceneManager")]
    public TypeScene scene;
    public bool isContinue;

    [Header("MiniGame")]
    public Sprite[] spHand;

    [Header("UIHome")]
    public bool isClick;

    public void Update()
    {
        txtDollar.text = ConvertNumber(GameManager.Instance.dollar);
        txtGold.text = ConvertNumber(GameManager.Instance.gold);
        if (scene == TypeScene.MINIGAME)
        {
            int id = GameManager.Instance.IDLocation;
            int indexType = GameManager.Instance.lsLocation[id].indexType;
            if (indexType != 0) GameManager.Instance.lsTypeMiniGame[id].lsMiniGame[indexType].inputMiniGame.text = ConvertNumber(GameManager.Instance.lsLocation[id].lsWorking[indexType].input);
            GameManager.Instance.lsTypeMiniGame[id].lsMiniGame[indexType].outputMiniGame.text = ConvertNumber(GameManager.Instance.lsLocation[id].lsWorking[indexType].output);

        }
    }


    public void BtnSettingOnclick()
    {
        panelSetting.SetActive(true);
    }
    public void BtnPlayOnclick()
    {
        if (!isClick)
        {
            isClick = true;
            isContinue = false;
            AudioManager.Instance.Play("Click");
            AudioManager.Instance.Stop("Menu",true);
            AudioManager.Instance.Play("GamePlay",true);

            ScenesManager.Instance.isNextScene = false;

            GameManager.Instance.dollar = GameConfig.Instance.dollarStart;
            GameManager.Instance.gold = GameConfig.Instance.goldStart;
            GameManager.Instance.CreatLocation(lsLocationUI[0]);
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
            AudioManager.Instance.Stop("Menu",true);
            AudioManager.Instance.Play("GamePlay",true);

            DataPlayer.Instance.LoadDataPlayer();
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
        DataPlayer.Instance.SaveDataPlayer();
        Application.Quit();
    }
}
