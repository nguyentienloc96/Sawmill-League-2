using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeScene
{
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

    [Header("InfoPlayer")]
    public Text txtDollar;
    public Text txtGold;
    public Text txtRevenue;
    public GameObject panelDollar;
    public GameObject panelGold;
    public Text txtDollarVideoAds;
    public Text txtDollarRecive;
    public Button btnGoldToDollar;
    public Image txtUIDollarRecive;
    public GameObject panelLoadingIAP;


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
    public List<string> lsNameForest;

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
    public Sprite[] spBG;

    [Header("UIHome")]
    public Button btncontinue;
    public GameObject popupStart;
    public GameObject popupHome;
    public bool isClick;
    public GameObject WarningForest;

    [Header("SellJob")]
    public Button btnSell;
    public Button btnUpgradeJob;
    public Button btnUpgradeTrunk;
    public Button btnNoUpgradeJob;
    public Button btnNoUpgradeTrunk;

    [Header("UIInfoUpgradeJob")]
    public Text nameUpgradeJob;
    public Text levelCurrentJob;
    public Text levelNextJob;
    public Text CapacityCurrentJob;
    public Text CapacityNextJob;
    public Text CostUpgradeJob;

    [Header("UIInfoUpgradeTruck")]
    public Text nameUpgradeTrunkJob;
    public Text levelCurrentTruck;
    public Text levelNextTruck;
    public Text CapacityCurrentTruck;
    public Text CapacityNextTruck;
    public Text CostUpgradeTruck;

    [Header("UIInfoSellJob")]
    public Text nameJob;
    public Image iconJob;
    public Text CostJob;


    [Header("Tutorial")]
    public GameObject popupTutorial;
    public Transform handTutorial;
    public GameObject objTutorial;
    public Transform btnUpgradeTutorial;
    public Transform btnFellingTutorial;
    public Transform btnCloseFellingTutorial;
    public GameObject panelWaitGrow;
    public Text txtWait;

    public bool isClickHome;
    public bool isClickTrunk;
    public bool isOnClickTrunk;
    public float speedTrunkTutorial;
    public List<string> arrAlphabetNeed = new List<string>();

    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    public void Start()
    {
        scene = TypeScene.HOME;
        if (!PlayerPrefs.HasKey("isTutorial"))
        {
            PlayerPrefs.SetInt("isTutorial", 0);
        }
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < arrAlphabet.Length; j++)
            {
                arrAlphabetNeed.Add(arrAlphabet[i] + arrAlphabet[j]);
            }
        }
    }

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

        if (JobUpgrade.activeInHierarchy && !btnUpgradeJob.interactable)
        {
            int id = GameManager.Instance.IDLocation;
            int indexType = GameManager.Instance.lsLocation[id].indexType;
            if (!isJobX10)
            {
                if (GameManager.Instance.dollar >= GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgrade)
                {
                    btnUpgradeJob.interactable = true;
                }
            }
            else
            {
                int level = GameManager.Instance.lsLocation[id].lsWorking[indexType].level;
                double priceUpgradeTotal = GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgrade;

                for (int i = level + 1; i < (level + 10); i++)
                {
                    priceUpgradeTotal +=
                        (double)((double)GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeStart
                        * Mathf.Pow((1 + GameManager.Instance.lsLocation[id].lsWorking[indexType].UN2), (level - 1)));
                }
                if (GameManager.Instance.dollar >= priceUpgradeTotal)
                {
                    btnUpgradeJob.interactable = true;
                }
            }
        }

        if (TruckUpgrade.activeInHierarchy && !btnUpgradeTrunk.interactable)
        {
            int id = GameManager.Instance.IDLocation;
            int indexType = GameManager.Instance.lsLocation[id].indexType;
            if (!isTrunkX10)
            {
                if (GameManager.Instance.dollar >= GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruck)
                {
                    btnUpgradeTrunk.interactable = true;
                }
            }
            else
            {
                int levelTruck = GameManager.Instance.lsLocation[id].lsWorking[indexType].levelTruck;
                double priceUpgradeTruckTotal = GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruck;

                for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
                {
                    priceUpgradeTruckTotal +=
                        (double)((double)GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruckStart
                        * Mathf.Pow((1 + GameConfig.Instance.XN2), (levelTruck - 1)));
                }
                if (GameManager.Instance.dollar >= priceUpgradeTruckTotal)
                {
                    btnUpgradeTrunk.interactable = true;
                }
            }
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
            popupHome.SetActive(false);
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
            popupHome.SetActive(true);
            ScenesManager.Instance.isNextScene = false;
            GameManager.Instance.sumHomeAll = 0;
            GameManager.Instance.LoadDate();
            GameManager.Instance.dollar = GameConfig.Instance.dollarStart;
            if (GameManager.Instance.gold < GameConfig.Instance.goldStart)
            {
                GameManager.Instance.gold = GameConfig.Instance.goldStart;
            }
            GameManager.Instance.ClearLocation();
            GameManager.Instance.CreatLocation(lsLocationUI[0], true);
            handWorld.position = lsLocationUI[0].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f);
            contentWorld.anchoredPosition = Vector3.zero;
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                GameManager.Instance.lsLocation[0].GetComponent<ScrollRect>().vertical = false;
            }
            ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                isSaveJson = true;
                scene = TypeScene.WOLRD;
                isClick = false;
                if (PlayerPrefs.GetInt("isTutorial") == 0)
                {
                    panelWaitGrow.SetActive(true);
                    txtWait.text = "Tap to Africa";
                    Ads.Instance.HideBanner();
                }
            });

        }
    }
    public void BtnContinueOnclick()
    {
        if (!isClick)
        {
            if (PlayerPrefs.GetInt("Continue") != 0 && PlayerPrefs.GetInt("isTutorial") != 0)
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
            else
            {
                BtnYesPlayOnclick();
            }
        }
    }
    public void BtnBackToWorld()
    {
        if (scene != TypeScene.WOLRD)
        {
            CloseJob();
            txtRevenue.enabled = true;
            scene = TypeScene.WOLRD;
            AudioManager.Instance.Play("Click");
            locationManager.transform.SetAsFirstSibling();
            panelSetting.SetActive(false);
        }
        else
        {
            ClosePupopFull();
            scene = TypeScene.HOME;
            AudioManager.Instance.Play("Menu", true);
            AudioManager.Instance.Stop("GamePlay", true);
            AudioManager.Instance.Play("Click");
            DataPlayer.Instance.SaveExit();
            panelSetting.SetActive(false);
            ScenesManager.Instance.secenes[0].objects.SetActive(true);
        }
    }

    public void PushGiveGold(string str)
    {
        PopupGiveGold.SetActive(true);
        PopupGiveGold.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str;
    }
    public string ConvertNumber(double number)
    {
        string smoney = string.Format("{0:#,##0}", number);
        for (int i = 0; i < arrAlphabetNeed.Count; i++)
        {
            if (smoney.Length >= 5 + i * 4 && smoney.Length < 9 + i * 4)
            {
                if (smoney[smoney.Length - (3 + i * 4)] != '0')
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 3));
                    smoney = smoney + arrAlphabetNeed[i];
                    if (i < 4)
                    {
                        smoney = smoney.Remove(smoney.Length - 3, 1);
                        smoney = smoney.Insert(smoney.Length - 2, ".");
                    }
                    else
                    {
                        smoney = smoney.Remove(smoney.Length - 4, 1);
                        smoney = smoney.Insert(smoney.Length - 3, ".");
                    }
                }
                else
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 1));
                    smoney = smoney + arrAlphabetNeed[i];
                }
                return smoney;
            }
        }
        return smoney;
    }

    public void UpgradeJob()
    {
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        GameManager.Instance.lsLocation[id].CheckInfoTypeOfWorkST(indexType);
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (objTutorial != null)
            {
                Destroy(objTutorial);
            }
            ControlHandTutorial(btnUpgradeJob.transform);
            txtWait.text = "Upgrade the capacity of the workshop";
        }
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
            if (GameManager.Instance.dollar >= GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgrade)
            {
                btnUpgradeJob.interactable = true;
            }
            else
            {
                btnUpgradeJob.interactable = false;
            }
        }
        else
        {
            arrXJob[0].color = new Color32(255, 255, 255, 128);
            arrXJob[1].color = new Color32(255, 255, 255, 255);
            GameManager.Instance.lsLocation[id].CheckInfoTypeOfWorkSTX10(indexType);
            int level = GameManager.Instance.lsLocation[id].lsWorking[indexType].level;
            double priceUpgradeTotal = GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgrade;

            for (int i = level + 1; i < (level + 10); i++)
            {
                priceUpgradeTotal +=
                    (double)((double)GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeStart
                    * Mathf.Pow((1 + GameManager.Instance.lsLocation[id].lsWorking[indexType].UN2), (level - 1)));
            }
            if (GameManager.Instance.dollar >= priceUpgradeTotal)
            {
                btnUpgradeJob.interactable = true;
            }
            else
            {
                btnUpgradeJob.interactable = false;
            }
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
            if (GameManager.Instance.dollar >= GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruck)
            {
                btnUpgradeTrunk.interactable = true;
            }
            else
            {
                btnUpgradeTrunk.interactable = false;
            }
        }
        else
        {
            arrXTrunk[0].color = new Color32(255, 255, 255, 128);
            arrXTrunk[1].color = new Color32(255, 255, 255, 255);
            GameManager.Instance.lsLocation[id].CheckInfoTruckX10(indexType);
            int levelTruck = GameManager.Instance.lsLocation[id].lsWorking[indexType].levelTruck;
            double priceUpgradeTruckTotal = GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruck;

            for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
            {
                priceUpgradeTruckTotal +=
                    (double)((double)GameManager.Instance.lsLocation[id].lsWorking[indexType].priceUpgradeTruckStart
                    * Mathf.Pow((1 + GameConfig.Instance.XN2), (levelTruck - 1)));
            }
            if (GameManager.Instance.dollar >= priceUpgradeTruckTotal)
            {
                btnUpgradeTrunk.interactable = true;
            }
            else
            {
                btnUpgradeTrunk.interactable = false;
            }
        }
    }

    public void YesUpgradeJob()
    {
        AudioManager.Instance.Play("Click");
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isJobX10)
            GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkST(indexType);
        else
            GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkSTX10(indexType);

        if (id == GameManager.Instance.lsLocation.Count - 1)
        {
            if (indexType == GameManager.Instance.lsLocation[id].countType)
            {
                txtRevenue.text
                = "Revenue : " + ConvertNumber(
                    GameManager.Instance.lsLocation[id]
                    .lsWorking[indexType].maxOutputMade
                    * GameConfig.Instance.r
                    * GameConfig.Instance.productCost
                    ) + "$/day";
            }
        }

        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (objTutorial != null)
            {
                Destroy(objTutorial);
            }
            ControlHandTutorial(btnNoUpgradeJob.transform);
            txtWait.text = "Turn off the panel";
        }
    }
    public void NoUpgradeJob()
    {
        AudioManager.Instance.Play("Click");
        JobUpgrade.SetActive(false);
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (objTutorial != null)
            {
                Destroy(objTutorial);
            }
            ControlHandTutorial(btnFellingTutorial);
            btnFellingTutorial.gameObject.SetActive(false);
            objTutorial.GetComponent<Image>().raycastTarget = true;
            txtWait.text = "Tap to Work yourself to increase the capacity";
        }
    }


    public void YesUpgradeTruck()
    {
        AudioManager.Instance.Play("Click");
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isTrunkX10)
            GameManager.Instance.lsLocation[id].UpgradeInfoTruck(indexType);
        else
            GameManager.Instance.lsLocation[id].UpgradeInfoTruckX10(indexType);
        if (indexType == 0)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                if (objTutorial != null)
                {
                    Destroy(objTutorial);
                }
                ControlHandTutorial(btnNoUpgradeTrunk.transform);
                txtWait.text = "Turn off the panel";
            }
        }
    }
    public void NoUpgradeTruck()
    {
        AudioManager.Instance.Play("Click");
        TruckUpgrade.SetActive(false);
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (objTutorial != null)
            {
                Destroy(objTutorial);
            }
            if (UIManager.Instance.popupTutorial.activeInHierarchy)
            {
                UIManager.Instance.popupTutorial.SetActive(false);
            }
            GameManager.Instance.lsLocation[0].GetComponent<ScrollRect>().vertical = true;
            PlayerPrefs.SetInt("isTutorial", 1);
            txtWait.text = "It's your show now. Good luck!";
            Invoke("HidePanelWait", 3f);
        }
    }


    public void HidePanelWait()
    {
        panelWaitGrow.SetActive(false);
        Ads.Instance.ShowBanner();
    }

    public void YesSellJob()
    {
        int id = GameManager.Instance.IDLocation;
        GameManager.Instance.lsLocation[id].SellJob();
        JobSell.SetActive(false);
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (objTutorial != null)
            {
                Destroy(objTutorial);
            }
            popupTutorial.SetActive(false);
            txtWait.text = "Tap to plant trees";
        }
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
        AudioManager.Instance.Stop("Felling");
        AudioManager.Instance.Stop("Drill");
        AudioManager.Instance.Stop("Debarking");
        AudioManager.Instance.Stop("Saw");
        AudioManager.Instance.Stop("Painting");
        AudioManager.Instance.Stop("Water");
        AudioManager.Instance.Stop("Polish");
    }

    public void ShowPanelDollar()
    {
        if (!panelDollar.activeSelf)
        {
            panelDollar.SetActive(true);
            int locationEnd = GameManager.Instance.lsLocation.Count - 1;
            int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
            double dollarRecive = 0;
            if (GameManager.Instance.lsLocation.Count > 1 && jobEnd != -1)
            {
                if (jobEnd == -1)
                {
                    locationEnd--;
                    jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
                }
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
            }
            else
            {
                dollarRecive = 0;
            }
            txtDollarVideoAds.text = UIManager.Instance.ConvertNumber(dollarRecive / 5) + "$";
            txtDollarRecive.text = UIManager.Instance.ConvertNumber(dollarRecive / 5) + "$";
            if (GameManager.Instance.gold > 0)
            {
                btnGoldToDollar.interactable = true;
                txtUIDollarRecive.color = new Color(255, 255, 255);
            }
            else
            {
                btnGoldToDollar.interactable = false;
                txtUIDollarRecive.color = new Color(150, 150, 150);
            }
        }
        else
            panelDollar.SetActive(false);
    }

    public void ShowPanelGold()
    {
        if (!panelGold.activeSelf)
        {
            panelGold.SetActive(true);
        }
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
        ClosePupopFull();
        scene = TypeScene.HOME;
        txtRevenue.enabled = true;
        AudioManager.Instance.Play("Menu", true);
        AudioManager.Instance.Stop("GamePlay", true);
        AudioManager.Instance.Play("Click");
        DataPlayer.Instance.SaveExit();
        panelSetting.SetActive(false);
        ScenesManager.Instance.secenes[0].objects.SetActive(true);
        if (PlayerPrefs.GetInt("Continue") == 0)
        {
            btncontinue.interactable = false;
        }
        else
        {
            btncontinue.interactable = true;
        }
    }

    public void ControlHandTutorial(Transform obj)
    {
        Vector3 posObj = obj.position;
        Transform tfObj = Instantiate(obj, popupTutorial.transform.GetChild(0));
        objTutorial = tfObj.gameObject;
        tfObj.SetAsFirstSibling();
        tfObj.position = posObj;
        handTutorial.position = posObj;
    }

    public void BtnShare()
    {
        AudioManager.Instance.Play("Click");
        ShareManager.Instance.ShareScreenshotWithText(GameConfig.Instance.string_Share);
    }

    public void BtnRate()
    {
        AudioManager.Instance.Play("Click");
#if UNITY_ANDROID
        if (GameConfig.Instance.link_ios != null)
        {
            Application.OpenURL(GameConfig.Instance.link_ios);
        }
#elif UNITY_IOS
        if (GameConfig.Instance.link_ios != null)
        {
            Application.OpenURL(GameConfig.Instance.link_ios);
        }
#endif
    }

    public void UpdateInfoUpgradeJob(string name, int levelCurrent,
    int levelNext, double capacityCurrent,
    double capacityNext, double cost)
    {
        nameUpgradeJob.text = name;
        levelCurrentJob.text = levelCurrent.ToString();
        levelNextJob.text = levelNext.ToString();
        CapacityCurrentJob.text = ConvertNumber(capacityCurrent);
        CapacityNextJob.text = ConvertNumber(capacityNext);
        CostUpgradeJob.text = ConvertNumber(cost);
    }

    public void UpdateInfoUpgradeTruck(string name, int levelCurrent,
    int levelNext, double capacityCurrent,
    double capacityNext, double cost)
    {
        nameUpgradeTrunkJob.text = name;
        levelCurrentTruck.text = levelCurrent.ToString();
        levelNextTruck.text = levelNext.ToString();
        CapacityCurrentTruck.text = ConvertNumber(capacityCurrent);
        CapacityNextTruck.text = ConvertNumber(capacityNext);
        CostUpgradeTruck.text = ConvertNumber(cost);
    }

    public void UpdateInfoSellJob(string name, Sprite spIcon, double cost)
    {
        nameJob.text = name;
        iconJob.sprite = spIcon;
        CostJob.text = ConvertNumber(cost);
    }

    public void WarningForestOnClick()
    {
        WarningForest.SetActive(false);
        int id = GameManager.Instance.IDLocation;
        GameManager.Instance.lsLocation[id].transform.GetChild(0).GetChild(0).localPosition = Vector3.zero;
    }

    public void ClosePupopFull()
    {
        CloseJob();
        JobSell.SetActive(false);
        JobUpgrade.SetActive(false);
        TruckUpgrade.SetActive(false);
        PopupAutoPlant.SetActive(false);
        PopupGiveGold.SetActive(false);
        panelDollar.SetActive(false);
        panelGold.SetActive(false);
    }
}
