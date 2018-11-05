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
    }

    [Header("InfoPlayer")]
    public Text txtDollar;
    public Text txtGold;
    public Text txtRevenue;
    public GameObject panelDollar;
    public GameObject panelGold;
    public Text txtDollarVideoAds;
    public Text txtDollarRecive;

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
    public bool isClick;

    [Header("SellJob")]
    public Button btnSell;
    public Button btnUpgradeJob;
    public Button btnUpgradeTrunk;
    public Button btnNoUpgradeJob;
    public Button btnNoUpgradeTrunk;


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

    public string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };


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
                }
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
    public string ConvertNumber(double number)
    {
        number = System.Math.Floor(number);
        string smoney = string.Format("{0:0}", number);
        if (smoney.Length >= 4 && smoney.Length < 8)
        {
            smoney = smoney.Substring(0, smoney.Length - 3);
            smoney = smoney + "k";
        }
        else if (smoney.Length >= 8 && smoney.Length < 12)
        {
            smoney = smoney.Substring(0, smoney.Length - 7);
            smoney = smoney + "M";

        }
        else if (smoney.Length >= 12 && smoney.Length < 16)
        {
            smoney = smoney.Substring(0, smoney.Length - 11);
            smoney = smoney + "B";
        }
        else if (smoney.Length >= 16 && smoney.Length < 20)
        {
            smoney = smoney.Substring(0, smoney.Length - 15);
            smoney = smoney + "T";
        }
        else if (smoney.Length >= 20)
        {
            int len = 20;
            for (int i = 0; i < arrAlphabet.Length / 2; i++)
            {
                for (int j = 0; j < arrAlphabet.Length; j++)
                {
                    if (smoney.Length >= (len + i * 26 * 4 + j * 4) && smoney.Length < (len + i * 26 * 4 + (j + 1) * 4))
                    {
                        smoney = smoney.Substring(0, smoney.Length - (len + i * 26 * 4 + j * 4 - 1));
                        smoney = smoney + arrAlphabet[i] + arrAlphabet[j];
                    }
                }
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
            txtWait.text = "Tap YES to Upgrade the capacity of the workshop";
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
        AudioManager.Instance.Play("Click");
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isJobX10)
            JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkST(indexType);
        else
            JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTypeOfWorkSTX10(indexType);

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
            txtWait.text = "Tap NO to turn off the panel";
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
            txtWait.text = "Tap to work yourself";
        }
    }


    public void YesUpgradeTruck()
    {
        AudioManager.Instance.Play("Click");
        int id = GameManager.Instance.IDLocation;
        int indexType = GameManager.Instance.lsLocation[id].indexType;
        if (!isTrunkX10)
            TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTruck(indexType);
        else
            TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.lsLocation[id].UpgradeInfoTruckX10(indexType);
        if (indexType == 0)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                if (objTutorial != null)
                {
                    Destroy(objTutorial);
                }
                ControlHandTutorial(btnNoUpgradeTrunk.transform);
                txtWait.text = "Tap NO to turn off the panel";
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
            txtWait.text = "It's your show now, good luck";
            Invoke("HidePanelWait", 3f);
        }
    }


    public void HidePanelWait()
    {
        panelWaitGrow.SetActive(false);
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
            if (jobEnd == -1)
            {
                locationEnd--;
                jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
            }
            double dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
            txtDollarVideoAds.text = UIManager.Instance.ConvertNumber(dollarRecive * 5) + "$";
            txtDollarRecive.text = UIManager.Instance.ConvertNumber(dollarRecive * 5) + "$";
        }
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
}
