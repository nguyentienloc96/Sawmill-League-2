using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;


    [Header("InforPlayer")]
    public Text txtDollar;
    public Text txtGold;
    public Text myTxtWood;

    [Header("ObjectMain")]
    public GameObject worldUI;
    public GameObject locationUI;
    public GameObject fellingUI;
    public Transform contentWorld;
    public GameObject itemLocationUI;
    public List<Button> lsLocationUI;

    [Header("Setting")]
    public GameObject mySetting;
    public bool isOnSetting;

    [Header("BuildWork")]
    public GameObject BuildDetail;
    public GameObject BuildSell;
    public GameObject CarDetail;
    public GameObject BuildUpdate;
    public GameObject CarUpdate;
    public string StrBuildUpdate;
    public string StrCarUpdate;

    [Header("MaskUI")]
    public Mask MaskWorld;
    public List<Mask> MaskLocation;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < DataUpdate.Instance.lstData_NameCountry.Count; i++)
        {
            GameObject item = Instantiate(itemLocationUI, contentWorld) as GameObject;
            item.GetComponent<LocationUI>()._ID = i;
            item.name = DataUpdate.Instance.lstData_NameCountry[i].name;
            item.GetComponent<LocationUI>()._Name = DataUpdate.Instance.lstData_NameCountry[i].name;
            item.transform.GetChild(2).GetComponent<Text>().text = string.Format("{0:000}", i);
            item.transform.GetChild(1).GetComponent<Text>().text = DataUpdate.Instance.lstData_NameCountry[i].name;
            if (i != 0)
            {
                item.transform.GetComponent<Button>().interactable = false;
            }
            lsLocationUI.Add(item.transform.GetComponent<Button>());
        }
    }

    public void Update()
    {
        if (txtDollar.gameObject.activeInHierarchy)
            txtDollar.text = ConvertMoney(GameManager.Instance.dollar);
        if (txtGold.gameObject.activeInHierarchy)
            txtGold.text = ConvertMoney(GameManager.Instance.gold);
        if (myTxtWood.gameObject.activeInHierarchy)
            myTxtWood.text = ConvertMoney(GameManager.Instance
                .lsLocation[GameManager.Instance.IDLocation]._LsWorking[0]._Material);
    }

    public void Setting()
    {
        isOnSetting = !isOnSetting;
        mySetting.SetActive(isOnSetting);
    }

    public void BackWorld()
    {
        locationUI.transform.SetAsFirstSibling();
        Setting();
    }

    public string ConvertMoney(long number)
    {
        string smoney = string.Format("{0:n0}", number);

        if (smoney.Length >= 5 && smoney.Length < 9)
        {
            if (number % 1000 != 0)
            {
                smoney = smoney.Substring(0, smoney.Length - 1);
                smoney = smoney + "k";
                smoney = smoney.Remove(smoney.Length - 4, 1);
                smoney = smoney.Insert(smoney.Length - 3, ".");
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
                smoney = smoney.Substring(0, smoney.Length - 5);
                smoney = smoney + "M";
                smoney = smoney.Remove(smoney.Length - 4, 1);
                smoney = smoney.Insert(smoney.Length - 3, ".");
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
                smoney = smoney.Substring(0, smoney.Length - 9);
                smoney = smoney + "B";
                smoney = smoney.Remove(smoney.Length - 4, 1);
                smoney = smoney.Insert(smoney.Length - 3, ".");
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
                smoney = smoney.Substring(0, smoney.Length - 13);
                smoney = smoney + "KB";
                smoney = smoney.Remove(smoney.Length - 5, 1);
                smoney = smoney.Insert(smoney.Length - 4, ".");
            }
            else
            {
                smoney = smoney.Substring(0, smoney.Length - 16);
                smoney = smoney + "KB";
            }
        }
        return smoney;
    }

    public void BuyWork()
    {
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        if (GameManager.Instance.gold
            >= GameManager.Instance.lsLocation[ID]
            ._LsWorking[IndexType]._Price)
        {
            GameManager.Instance.gold -= GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._Price;
            GameManager.Instance.lsLocation[ID]._CountType++;
            GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._Icon.color = new Color32(255, 255, 255, 255);
            if (GameManager.Instance.lsLocation[ID]._CountType + 1 >= GameManager.Instance.lsLocation[ID]._LsWorking.Length)
            {
                GameManager.Instance.CreatLocation();
            }
        }
        BuildSell.SetActive(false);
    }

    public void WorkYourSelf()
    {
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        GameManager.Instance.lsMiniGame[IndexType].SetActive(true);
        GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._isCanAuto = true;
        BuildDetail.SetActive(false);
        ConTrollMask(false);
    }

    public void CloseWorkYourSelf()
    {
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        GameManager.Instance.lsMiniGame[IndexType].SetActive(false);
        GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._isCanAuto = false;
        ConTrollMask(true);
        BuildUpdate.SetActive(false);
        CarUpdate.SetActive(false);
    }

    public void LoadBuildWork(int type)
    {
        if (type == 0)
        {
            BuildDetail.SetActive(true);
            BuildSell.SetActive(false);
            CarDetail.SetActive(false);
        }
        else if (type == 1)
        {
            BuildDetail.SetActive(false);
            BuildSell.SetActive(true);
            CarDetail.SetActive(false);
        }
        else
        {
            BuildDetail.SetActive(false);
            BuildSell.SetActive(false);
            CarDetail.SetActive(true);
        }
        BuildUpdate.SetActive(false);
        CarUpdate.SetActive(false);
    }

    public void ConTrollMask(bool isOn)
    {
        MaskWorld.enabled = isOn;
        for (int i = 0; i < MaskLocation.Count; i++)
        {
            MaskLocation[i].enabled = isOn;
        }
    }

    public void BuildUpdateOnClick()
    {
        BuildUpdate.transform.GetChild(0).GetComponent<Text>().text = StrBuildUpdate;
        BuildUpdate.SetActive(true);
    }

    public void CarUpdateOnClick()
    {
        CarUpdate.transform.GetChild(0).GetComponent<Text>().text = StrCarUpdate;
        CarUpdate.SetActive(true);
    }

    public void YesBuildUpdateOnClick()
    {

    }

    public void NoBuildUpdateOnClick()
    {
        BuildUpdate.SetActive(false);
    }

    public void YesCarUpdateOnClick()
    {

    }

    public void NoCarUpdateOnClick()
    {
        CarUpdate.SetActive(false);
    }

    public void BtnPlayOnclick()
    {
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main);
    }

    public void BtnContinueOnclick()
    {
        DataPlayer.Instance.LoadDataPlayer();
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main);
    }

    public void SellProduct()
    {
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        long material = GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._Material;
        GameManager.Instance.gold += material * GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._PriceMaterial;
        GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._Material -= material;
        GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._TextNumberOfMaterial.text
            = ConvertMoney(GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._Material);

    }
}
