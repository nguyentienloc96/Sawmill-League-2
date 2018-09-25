using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ForestST
{
    public int tree;
    public Forest forestClass;
}

[System.Serializable]
public struct TypeOfWorkST
{
    [Header("Information")]
    public string name;
    public int id;
    public int level;

    [Header("UI")]
    public Image icon;
    public Text textInput;
    public Text textOutput;
    public GameObject info;
    public GameObject build;

    [Header("Parameters")]
    public long input;
    public long output;
    public long priceOutput;

    public long maxOutputMade; //C0

    [HideInInspector]
    public float timeWorking;
    [HideInInspector]
    public bool isXJob;

    [Header("Transport")]
    public long levelTruck;
    public long priceUpgradeTruck;
    public long priceTruckSent; //X0
    public long currentSent;
    public long maxSent; //CI0
    public TruckManager truckManager;

    [Header("Price")]
    public long priceUpgrade;
    public long price; //P0
    [HideInInspector]
    public float UN2;
}

public class Location : MonoBehaviour
{
    public int id;
    public string nameLocation;
    public int countType = 0;

    #region HideInInspector
    [HideInInspector]
    public int indexType = 0;
    [HideInInspector]
    public int capIndex = 4;
    [HideInInspector]
    public int captruckIndex = 4;
    #endregion

    public ForestST forest;
    public TypeOfWorkST[] lsWorking;

    public Streets streets;
    public Others others;
    public Rivers rivers;

    #region HideInInspector
    [HideInInspector]
    public bool isLoaded;
    public bool isLoadFull;

    [HideInInspector]
    public List<int> lsOther;
    [HideInInspector]
    public List<int> lsRiverRight;
    [HideInInspector]
    public List<int> lsRiverLeft;
    [HideInInspector]
    public List<int> lsStreet;
    #endregion

    public void LoadLocation()
    {
        StartCoroutine(IELoadLocation());
    }
    public void LoadLocationJson()
    {
        StartCoroutine(IELoadLocationJson());
    }
    public IEnumerator IELoadLocation()
    {
        isLoaded = false;
        others.LoadOtherRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        rivers.LoadRiverRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        streets.LoadStreetRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        LoadInfoTypeOfWorkST();
        yield return new WaitUntil(() => isLoaded);
    }
    public IEnumerator IELoadLocationJson()
    {
        isLoaded = false;
        others.LoadOtherJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        rivers.LoadRiverJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        streets.LoadStreetJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        LoadInfoTypeOfWorkST();
        yield return new WaitUntil(() => isLoaded);
    }
    public void LoadInfoTypeOfWorkST()
    {
        for (int i = 0; i < lsWorking.Length; i++)
        {
            lsWorking[i].UN2 = GameConfig.Instance.UN2;
            lsWorking[i].price = (long)(GameConfig.Instance.p0 * Mathf.Pow(GameConfig.Instance.p0i, i));
            lsWorking[i].maxOutputMade = (long)(GameConfig.Instance.c0 * Mathf.Pow(GameConfig.Instance.c0i, i));
            lsWorking[i].priceUpgrade = (long)(lsWorking[i].price * GameConfig.Instance.UN1i);
            lsWorking[i].priceTruckSent = (long)(GameConfig.Instance.x0 * Mathf.Pow(GameConfig.Instance.x0i, i));
            lsWorking[i].priceOutput = lsWorking[i].priceTruckSent * 3;
            lsWorking[i].maxSent = lsWorking[i].maxOutputMade;
            lsWorking[i].priceUpgradeTruck = (long)(lsWorking[i].price * GameConfig.Instance.XN1i);
        }
        isLoaded = true;
        isLoadFull = true;
        if (!UIManager.Instance.isContinue) ScenesManager.Instance.isNextScene = true;
    }



    public string UpgradeInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        string stInfo = "";
        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgrade)
        {
            GameManager.Instance.dollar -= lsWorking[indexType].priceUpgrade;
            // Update thông số
            lsWorking[indexType].level++;
            lsWorking[indexType].maxOutputMade = (long)((GameConfig.Instance.c0 * Mathf.Pow(2, lsWorking[indexType].id)) * (1 + (float)lsWorking[indexType].level / (float)capIndex));
            lsWorking[indexType].priceUpgrade = (long)((float)lsWorking[indexType].priceUpgrade * Mathf.Pow((1 + lsWorking[indexType].UN2), (lsWorking[indexType].level - 1)));
            lsWorking[indexType].priceOutput = (long)(lsWorking[indexType].priceOutput * Mathf.Pow((1 + lsWorking[indexType].UN2), (lsWorking[indexType].level - 1)));
            stInfo = lsWorking[indexType].name + " " + nameLocation + "\n"
                + "Level : " + lsWorking[indexType].level + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].maxOutputMade) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgrade);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }
    public void CheckInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        string stInfo = "";
        stInfo = lsWorking[indexType].name + " " + nameLocation + "\n"
                + "Level : " + lsWorking[indexType].level + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].maxOutputMade) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgrade);
        UIManager.Instance.JobUpgrade.SetActive(true);
        UIManager.Instance.JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
    }



    public void CheckInfoTruck(int idType)
    {
        indexType = idType;
        string stInfo = "";
        stInfo = "Truck " + lsWorking[indexType].name + " " + nameLocation + "\n"
                + "Level : " + lsWorking[indexType].levelTruck + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].maxSent) + "\n"
                + "Transportation Fee : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceTruckSent) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgradeTruck);
        UIManager.Instance.TruckUpgrade.SetActive(true);
        UIManager.Instance.TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
    }
    public string UpgradeInfoTruck(int idType)
    {
        indexType = idType;
        string stInfo = "";
        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgradeTruck)
        {
            GameManager.Instance.dollar -= lsWorking[indexType].priceUpgradeTruck;
            // Update thông số
            lsWorking[indexType].levelTruck++;
            lsWorking[indexType].maxSent = (long)(lsWorking[indexType].maxSent * (1 + (float)lsWorking[indexType].levelTruck / (float)captruckIndex));
            lsWorking[indexType].priceTruckSent = (long)(lsWorking[indexType].priceTruckSent * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck - 1)));
            lsWorking[indexType].priceUpgradeTruck = (long)(lsWorking[indexType].priceUpgradeTruck * Mathf.Pow((1 + GameConfig.Instance.XN2), (lsWorking[indexType].levelTruck - 1)));
            stInfo = "Truck " + lsWorking[indexType].name + " " + nameLocation + "\n"
                + "Level : " + lsWorking[indexType].levelTruck + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].maxSent) + "\n"
                + "Transportation Fee : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceTruckSent) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgradeTruck);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }



    public void SellJob()
    {
        countType++;
        indexType = countType;
        lsWorking[countType].icon.color = Color.white;
        lsWorking[countType].info.SetActive(true);
        lsWorking[countType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].input);
        lsWorking[countType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].output);
        if (countType + 1 == lsWorking.Length)
        {
            UIManager.Instance.lsBtnLocationUI[GameManager.Instance.lsLocation.Count].interactable = true;
            GameManager.Instance.CreatLocation(UIManager.Instance.lsLocationUI[GameManager.Instance.lsLocation.Count]);
        }
    }



    #region Felling
    public void Felling()
    {
        if (forest.tree > 0 && forest.forestClass.isGrowed)
        {
            if (lsWorking[0].isXJob)
            {
                lsWorking[0].timeWorking += Time.deltaTime * 1.3f;
            }
            else
            {
                lsWorking[0].timeWorking += Time.deltaTime;
            }
            if (lsWorking[0].timeWorking >= GameConfig.Instance.p0Time)
            {
                FellingComplete();
                lsWorking[0].timeWorking = 0;
            }
        }
    }

    public void FellingComplete()
    {
        if (forest.tree > 0)
        {
            forest.forestClass.lsTree[forest.forestClass.lsTree.Length - forest.tree].transform.GetChild(0).gameObject.SetActive(false);
            forest.tree--;
        }
        lsWorking[0].output += (long)(10000 / forest.forestClass.transform.childCount);
        lsWorking[0].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[0].output);
        if (forest.tree <= 0)
        {
            forest.forestClass.ResetForest();
        }
        if (lsWorking[0].id < lsWorking.Length)
        {
            if (lsWorking[0].output > 0)
            {
                lsWorking[0].truckManager.LoadTruck();
            }
        }
    }
    #endregion

    #region Job! Felling
    public void Job(int idType)
    {
        if (lsWorking[idType].input > 0)
        {
            if (lsWorking[idType].isXJob)
            {
                lsWorking[idType].timeWorking += Time.deltaTime * 1.3f;
            }
            else
            {
                lsWorking[idType].timeWorking += Time.deltaTime;
            }
            if (lsWorking[idType].timeWorking >= GameConfig.Instance.p0Time)
            {
                JobComplete(idType);
                lsWorking[idType].timeWorking = 0;
            }
        }
    }

    public void JobComplete(int idType)
    {
        long materialCurrent = 0;
        if (lsWorking[idType].input >= lsWorking[idType].maxOutputMade)
        {
            materialCurrent = lsWorking[idType].maxOutputMade;
        }
        else
        {
            materialCurrent = lsWorking[idType].input;
        }
        lsWorking[idType].input -= materialCurrent;
        lsWorking[idType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].input);
        lsWorking[idType].output += (long)(0.9f * materialCurrent);
        lsWorking[idType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].output);
        if (lsWorking[idType].id < lsWorking.Length)
        {
            if (lsWorking[idType].output > 0)
            {
                lsWorking[idType].truckManager.LoadTruck();
            }
        }
    }
    #endregion



    public void Update()
    {
        Felling();
        for (int i = 1; i < countType; i++)
        {
            Job(i);
        }
    }


    public void WordYourSelf(int idType)
    {
        UIManager.Instance.scene = TypeScene.MINIGAME;
        indexType = idType;
        GameManager.Instance.lsMiniGame[indexType].SetActive(true);
        lsWorking[indexType].isXJob = true;
    }


    public void HomeOnclick(int idType)
    {
        indexType = idType;
        if (countType >= idType)
        {
            lsWorking[idType].build.SetActive(true);
            HideObject(lsWorking[idType].build, 10f);
        }
        else if (countType + 1 == idType)
        {
            string str = "You want to buy " + lsWorking[idType].name + " " + UIManager.Instance.ConvertNumber(lsWorking[idType].price) + "$ ?";
            UIManager.Instance.JobSell.transform.GetChild(1).GetComponent<Text>().text = str;
            UIManager.Instance.JobSell.SetActive(true);
        }
    }


    public void HideObject(GameObject obj, float time)
    {
        StartCoroutine(IEHideObject(obj, time));
    }
    public IEnumerator IEHideObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
