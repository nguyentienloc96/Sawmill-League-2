using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public Text txtDollar;
    public Text txtGold;

    public GameObject worldUI;
    public GameObject locationUI;
    public GameObject fellingUI;
    public Transform contentWorld;
    public GameObject itemLocationUI;

    public GameObject setting;
    public bool turnOnOff;

    public GameObject BuildDetail;
    public GameObject BuildSell;
    public GameObject CarDetail;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < DataUpdate.Instance.lstData_NameCountry.Count; i++)
        {
            GameObject item = Instantiate(itemLocationUI, contentWorld) as GameObject;
            item.GetComponent<LocationUI>()._ID = i;
            item.name = DataUpdate.Instance.lstData_NameCountry[i].name;
            item.GetComponent<LocationUI>()._Name = DataUpdate.Instance.lstData_NameCountry[i].name;
            item.transform.GetChild(2).GetComponent<Text>().text = string.Format("{0:000}", i);
            item.transform.GetChild(1).GetComponent<Text>().text = DataUpdate.Instance.lstData_NameCountry[i].name;
            if(i != 0)
            {
                item.transform.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Update()
    {
        txtDollar.text = ConvertMoney(GameManager.Instance.dollar);
        txtGold.text = ConvertMoney(GameManager.Instance.gold);
    }

    public void Setting()
    {
        turnOnOff = !turnOnOff;
        setting.SetActive(turnOnOff);
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
        if (GameManager.Instance.gold 
            >= GameManager.Instance.lsLocation[ID]
            ._LsWorking[GameManager.Instance.lsLocation[ID]._IndexType]._Price)
        {
            GameManager.Instance.gold -= GameManager.Instance.lsLocation[ID]
            ._LsWorking[GameManager.Instance.lsLocation[ID]._IndexType]._Price;
            GameManager.Instance.lsLocation[ID]._CountType++;
            GameManager.Instance.lsLocation[ID]._LsWorking[GameManager.Instance.lsLocation[ID]._CountType]._Icon.color = new Color32(255, 255, 255, 255);
        }
        BuildSell.SetActive(false);
    }

}
