using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ForestST
{
    public int tree;
    public Forest forestClass;
    public Button btnAutoPlant;
    public bool isOnBtnAutoPlant;
    public bool isAutoPlant;
    [HideInInspector]
    public float timeFelling;
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
    public Animator anim;
    public Text textInput;
    public Text textOutput;
    public GameObject info;

    [Header("Parameters")]
    public long input;
    public long output;
    public long priceOutput;

    public long maxOutputMade; //C0
    public long maxOutputMadeStart; //C0

    [HideInInspector]
    public float timeWorking;
    [HideInInspector]
    public bool isXJob;

    [Header("Transport")]
    public int levelTruck;
    public long priceUpgradeTruck;
    public long priceUpgradeTruckStart;
    public long priceTruckSent; //X0
    public long priceTruckSentStart; //X0
    public long currentSent;
    public long maxSent; //CI0
    public long maxSentStart; //CI0
    public TruckManager truckManager;

    [Header("Price")]
    public long priceUpgrade;
    public long priceUpgradeStart;
    public long price; //P0
    [HideInInspector]
    public float UN2;
}

public class Location : MonoBehaviour
{
    public int id;
    public string nameLocation;
    public int indexTypeWork;
    public int countType;
    [HideInInspector]
    public int makerType;
    public ForestST forest;
    public TypeOfWorkST[] lsWorking;

    public Streets streets;
    public Others others;
    public Rivers rivers;

    #region HideInInspector
    [HideInInspector]
    public int indexType;
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
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
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
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
    }
    public void LoadInfoTypeOfWorkST()
    {
        for (int i = 0; i < lsWorking.Length; i++)
        {
            int LevelHome = GameManager.Instance.sumHomeAll + i;
            lsWorking[i].UN2 = GameConfig.Instance.UN2;
            lsWorking[i].price = (long)(GameConfig.Instance.p0 * Mathf.Pow(GameConfig.Instance.p0i, LevelHome));
            lsWorking[i].maxOutputMade = (long)(GameConfig.Instance.c0 * Mathf.Pow(GameConfig.Instance.c0i, LevelHome));
            lsWorking[i].maxOutputMadeStart = (long)(GameConfig.Instance.c0 * Mathf.Pow(GameConfig.Instance.c0i, LevelHome));
            lsWorking[i].priceUpgrade = (long)(lsWorking[i].price * GameConfig.Instance.UN1i);
            lsWorking[i].priceUpgradeStart = (long)(lsWorking[i].price * GameConfig.Instance.UN1i);
            lsWorking[i].priceTruckSent = (long)(GameConfig.Instance.x0 * Mathf.Pow(GameConfig.Instance.x0i, LevelHome));
            lsWorking[i].priceTruckSentStart = (long)(GameConfig.Instance.x0 * Mathf.Pow(GameConfig.Instance.x0i, LevelHome));
            lsWorking[i].priceOutput = (long)GameConfig.Instance.productCost;
            lsWorking[i].maxSent = lsWorking[i].maxOutputMade * GameConfig.Instance.MaxSentStartX5;
            lsWorking[i].maxSentStart = lsWorking[i].maxOutputMade * GameConfig.Instance.MaxSentStartX5;
            lsWorking[i].priceUpgradeTruck = (long)(lsWorking[i].price * GameConfig.Instance.XN1i);
            lsWorking[i].priceUpgradeTruckStart = (long)(lsWorking[i].price * GameConfig.Instance.XN1i);
        }
        isLoaded = true;
        isLoadFull = true;
        if (!UIManager.Instance.isContinue)
        {
            ScenesManager.Instance.isNextScene = true;
            GameManager.Instance.sumHomeAll += lsWorking.Length;
        }
    }


    public void CheckInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        string stInfo = "";
        stInfo = lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].level + 1) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)(((float)lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r * (1 + (float)(lsWorking[indexType].level + 1) / GameConfig.Instance.capIndex)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgrade);
        UIManager.Instance.JobUpgrade.SetActive(true);
        UIManager.Instance.JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
        UIManager.Instance.JobUpgrade.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.JobUpgrade.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXJob[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXJob[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isJobX10 = false;
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
            lsWorking[indexType].maxOutputMade = (long)(((float)lsWorking[indexType].maxOutputMadeStart * (1 + (float)lsWorking[indexType].level / GameConfig.Instance.capIndex)));
            lsWorking[indexType].priceUpgrade = (long)((float)lsWorking[indexType].priceUpgradeStart * Mathf.Pow((1 + lsWorking[indexType].UN2), (lsWorking[indexType].level - 1)));

            stInfo = lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].level + 1) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)(((float)lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r * (1 + (float)(lsWorking[indexType].level + 1) / GameConfig.Instance.capIndex)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgrade);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }
    public void CheckInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        int level = lsWorking[indexType].level;
        long priceUpgradeTotal = lsWorking[indexType].priceUpgrade;

        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeTotal += (long)((float)lsWorking[indexType].priceUpgradeStart * Mathf.Pow((1 + lsWorking[indexType].UN2), (level - 1)));
        }
        string stInfo = "";
        stInfo = lsWorking[indexType].name + "\n"
                + "Level : " + (level + 10) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)(((float)lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r * (1 + (float)(level + 10) / GameConfig.Instance.capIndex)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(priceUpgradeTotal);
        UIManager.Instance.JobUpgrade.SetActive(true);
        UIManager.Instance.JobUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
    }
    public string UpgradeInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        int level = lsWorking[indexType].level;
        long priceUpgradeTotal = lsWorking[indexType].priceUpgrade;

        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeTotal += (long)((float)lsWorking[indexType].priceUpgradeStart * Mathf.Pow((1 + lsWorking[indexType].UN2), (level - 1)));
        }
        string stInfo = "";
        if (GameManager.Instance.dollar >= priceUpgradeTotal)
        {
            GameManager.Instance.dollar -= priceUpgradeTotal;
            // Update thông số
            lsWorking[indexType].level += 10;
            lsWorking[indexType].maxOutputMade = (long)(((float)lsWorking[indexType].maxOutputMadeStart * (1 + (float)(lsWorking[indexType].level + 10) / GameConfig.Instance.capIndex)));
            lsWorking[indexType].priceUpgrade = (long)((float)lsWorking[indexType].priceUpgradeStart * Mathf.Pow((1 + lsWorking[indexType].UN2), (lsWorking[indexType].level + 10 - 1)));

            level = lsWorking[indexType].level;
            priceUpgradeTotal = lsWorking[indexType].priceUpgrade;

            for (int i = level + 1; i < (level + 10); i++)
            {
                priceUpgradeTotal += (long)((float)lsWorking[indexType].priceUpgradeStart * Mathf.Pow((1 + lsWorking[indexType].UN2), (level - 1)));
            }

            stInfo = lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].level + 10) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)(((float)lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r * (1 + (float)(level + 10) / GameConfig.Instance.capIndex)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(priceUpgradeTotal);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }


    public void CheckInfoTruck(int idType)
    {
        indexType = idType;
        string stInfo = "";
        stInfo = "Truck " + lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].levelTruck + 1) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 1) / (float)GameConfig.Instance.captruckIndex))) + "\n"
                + "Transportation Fee : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].priceTruckSentStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgradeTruck);
        UIManager.Instance.TruckUpgrade.SetActive(true);
        UIManager.Instance.TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
        UIManager.Instance.TruckUpgrade.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.TruckUpgrade.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXTrunk[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXTrunk[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isTrunkX10 = false;
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
            lsWorking[indexType].maxSent = (long)((float)lsWorking[indexType].maxSentStart * (1f + (float)lsWorking[indexType].levelTruck / (float)GameConfig.Instance.captruckIndex));
            lsWorking[indexType].priceTruckSent = (long)((float)lsWorking[indexType].priceTruckSentStart * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck - 1)));
            lsWorking[indexType].priceUpgradeTruck = (long)((float)lsWorking[indexType].priceUpgradeTruckStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XN2), (lsWorking[indexType].levelTruck - 1)));

            stInfo = "Truck " + lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].levelTruck + 1) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 1) / (float)GameConfig.Instance.captruckIndex))) + "\n"
                + "Transportation Fee : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].priceTruckSentStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(lsWorking[indexType].priceUpgradeTruck);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }
    public void CheckInfoTruckX10(int idType)
    {
        indexType = idType;
        int levelTruck = lsWorking[indexType].levelTruck;
        long priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;

        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckTotal += (long)((float)lsWorking[indexType].priceUpgradeTruckStart * Mathf.Pow((1 + GameConfig.Instance.XN2), (levelTruck - 1)));
        }
        string stInfo = "";
        stInfo = "Truck " + lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].levelTruck + 10) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 10) / (float)GameConfig.Instance.captruckIndex))) + "\n"
            + "Transportation Fee : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].priceTruckSentStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck + 10 - 1)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(priceUpgradeTruckTotal);
        UIManager.Instance.TruckUpgrade.SetActive(true);
        UIManager.Instance.TruckUpgrade.transform.GetChild(0).GetComponent<Text>().text = stInfo;
    }
    public string UpgradeInfoTruckX10(int idType)
    {
        indexType = idType;
        int levelTruck = lsWorking[indexType].levelTruck;
        long priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;

        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckTotal += (long)((float)lsWorking[indexType].priceUpgradeTruckStart * Mathf.Pow((1 + GameConfig.Instance.XN2), (levelTruck - 1)));
        }
        string stInfo = "";
        if (GameManager.Instance.dollar >= priceUpgradeTruckTotal)
        {
            GameManager.Instance.dollar -= priceUpgradeTruckTotal;
            // Update thông số
            lsWorking[indexType].levelTruck += 10;
            lsWorking[indexType].maxSent = (long)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 10) / (float)GameConfig.Instance.captruckIndex));
            lsWorking[indexType].priceTruckSent = (long)((float)lsWorking[indexType].priceTruckSentStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck + 10 - 1)));
            lsWorking[indexType].priceUpgradeTruck = (long)((float)lsWorking[indexType].priceUpgradeTruckStart * Mathf.Pow((1 + GameConfig.Instance.XN2), (lsWorking[indexType].levelTruck + 10 - 1)));

            levelTruck = lsWorking[indexType].levelTruck;
            priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;

            for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
            {
                priceUpgradeTruckTotal += (long)((float)lsWorking[indexType].priceUpgradeTruckStart * Mathf.Pow((1 + GameConfig.Instance.XN2), (levelTruck - 1)));
            }
            stInfo = "Truck " + lsWorking[indexType].name + "\n"
                + "Level : " + (lsWorking[indexType].levelTruck + 10) + "\n"
                + "Capacity : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 10) / (float)GameConfig.Instance.captruckIndex))) + "\n"
                + "Transportation Fee : " + UIManager.Instance.ConvertNumber((long)((float)lsWorking[indexType].priceTruckSentStart * GameConfig.Instance.XT1i * Mathf.Pow((1 + GameConfig.Instance.XT2), (lsWorking[indexType].levelTruck + 10 - 1)))) + "\n"
                + "Price Upgrade : " + UIManager.Instance.ConvertNumber(priceUpgradeTruckTotal);
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }


    public void SellJob()
    {
        if (GameManager.Instance.dollar >= lsWorking[countType + 1].price)
        {
            GameManager.Instance.dollar -= lsWorking[countType + 1].price;
            countType++;
            indexType = countType;
            lsWorking[countType].icon.color = Color.white;
            lsWorking[countType].info.SetActive(true);
            lsWorking[countType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].input);
            lsWorking[countType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].output);
            if (countType + 1 == lsWorking.Length)
            {
                int indexLsLocation = GameManager.Instance.lsLocation.Count;
                GameManager.Instance.CreatLocation(UIManager.Instance.lsLocationUI[indexLsLocation]);
                UIManager.Instance.handWorld.position = UIManager.Instance.lsLocationUI[indexLsLocation].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f);
                forest.isOnBtnAutoPlant = true;
            }
        }
    }


    #region Felling
    public void FellingForest()
    {
        if (forest.tree > 0 && forest.forestClass.isGrowed)
        {
            if (lsWorking[0].isXJob)
            {
                forest.timeFelling += Time.deltaTime * (1f + GameConfig.Instance.WYS);
            }
            else
            {
                forest.timeFelling += Time.deltaTime;
            }
            if (forest.timeFelling >= (GameConfig.Instance.TimeForest / (float)forest.forestClass.lsTree.Length))
            {
                if (forest.tree > 0)
                {
                    forest.forestClass.lsTree[forest.forestClass.lsTree.Length - forest.tree].transform.GetChild(0).gameObject.SetActive(false);
                    forest.tree--;
                }
                if (forest.tree <= 0)
                {
                    forest.forestClass.ResetForest();
                }
                forest.timeFelling = 0;
            }
        }
    }
    public void Felling()
    {
        if (forest.tree > 0 && forest.forestClass.isGrowed)
        {
            if (lsWorking[0].isXJob)
            {
                lsWorking[0].timeWorking += Time.deltaTime * (1f + GameConfig.Instance.WYS);
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

        lsWorking[0].output += (long)(GameConfig.Instance.r * lsWorking[0].maxOutputMade);
        lsWorking[0].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[0].output);

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
                lsWorking[idType].timeWorking += Time.deltaTime * (1f + GameConfig.Instance.WYS);
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
        lsWorking[idType].output += (long)(GameConfig.Instance.r * materialCurrent);
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
        FellingForest();
        Felling();
        for (int i = 1; i <= countType; i++)
        {
            Job(i);
        }

        if (!forest.isAutoPlant && forest.isOnBtnAutoPlant)
        {
            if (id + 1 < GameManager.Instance.lsLocation.Count)
            {
                if (GameManager.Instance.lsLocation[id + 1].countType >= 0)
                {
                    if (GameManager.Instance.dollar >= (long)(lsWorking[0].price * GameConfig.Instance.AutoPlant))
                    {
                        forest.btnAutoPlant.interactable = true;
                    }
                    else
                    {
                        forest.btnAutoPlant.interactable = false;
                    }
                }
                else
                {
                    forest.btnAutoPlant.interactable = false;
                }
            }
            else
            {
                forest.btnAutoPlant.interactable = false;
            }
        }
        else
        {
            forest.btnAutoPlant.interactable = false;
        }
    }


    public void AutoPlant()
    {
        string str = "Do you want to spend " + UIManager.Instance.ConvertNumber((long)(lsWorking[0].price * GameConfig.Instance.AutoPlant)) + "$ On Auto Reforestation in " + nameLocation;
        UIManager.Instance.PopupAutoPlant.GetComponent<AutoPlant>().AutoPlant_Onclick(str, () =>
        {
            if (GameManager.Instance.dollar >= (long)(lsWorking[0].price * GameConfig.Instance.AutoPlant))
            {
                GameManager.Instance.dollar -= (long)(lsWorking[0].price * GameConfig.Instance.AutoPlant);
                forest.isAutoPlant = true;
                forest.btnAutoPlant.interactable = false;
                UIManager.Instance.PopupAutoPlant.SetActive(false);
            }
        });
    }


    public void WordYourSelf(int idType)
    {
        UIManager.Instance.scene = TypeScene.MINIGAME;
        indexType = idType;
        GameManager.Instance.lsTypeMiniGame[indexTypeWork].lsMiniGame[indexType].miniGame.SetActive(true);
        lsWorking[indexType].isXJob = true;
    }
    public void HomeOnclick(int idType)
    {
        indexType = idType;
        if (countType >= idType)
        {
            WordYourSelf(idType);
        }
        else if (countType + 1 == idType)
        {
            UIManager.Instance.isJobX10 = false;
            string str = lsWorking[idType].name + " \nPrice : " + UIManager.Instance.ConvertNumber(lsWorking[idType].price) + "$ ?";
            UIManager.Instance.JobSell.transform.GetChild(0).GetComponent<Text>().text = str;
            UIManager.Instance.JobSell.SetActive(true);
            if (GameManager.Instance.dollar >= lsWorking[idType].price)
            {
                UIManager.Instance.btnSell.interactable = true;
            }
            else
            {
                UIManager.Instance.btnSell.interactable = false;
            }
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
