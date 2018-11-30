using System;
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
    public Text textLevel;
    public Image icon;
    public Animator anim;
    public Animator animLock;
    public Text textInput;
    public Text textOutput;
    public GameObject info;

    [Header("Parameters")]
    public double input;
    public double output;
    public double priceOutput;

    public double maxOutputMade; //C0
    public double maxOutputMadeStart; //C0

    [HideInInspector]
    public float timeWorking;
    [HideInInspector]
    public bool isXJob;

    [Header("Transport")]
    public int levelTruck;
    public double priceUpgradeTruck;
    public double priceUpgradeTruckStart;
    public double priceTruckSent; //X0
    public double priceTruckSentStart; //X0
    public double currentSent;
    public double maxSent; //CI0
    public double maxSentStart; //CI0
    public TruckManager truckManager;

    [Header("Price")]
    public double priceUpgrade;
    public double priceUpgradeStart;
    public double price; //P0
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
    public Text txtNameForest;
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
        streets.LoadStreetRandom();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        LoadInfoTypeOfWorkST();
        yield return new WaitUntil(() => isLoaded);
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
        txtNameForest.text = UIManager.Instance.lsNameForest[id];
    }
    public IEnumerator IELoadLocationJson()
    {
        isLoaded = false;
        others.LoadOtherJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        streets.LoadStreetJson();
        yield return new WaitUntil(() => isLoaded);
        isLoaded = false;
        isLoadFull = true;
        isLoaded = true;
        yield return new WaitUntil(() => isLoaded);
        if (indexTypeWork == 0)
        {
            lsWorking[lsWorking.Length - 1].anim.SetFloat("indexMaker", makerType);
        }
        txtNameForest.text = UIManager.Instance.lsNameForest[id];
    }
    public void LoadInfoTypeOfWorkST()
    {
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            for (int i = 0; i < lsWorking.Length; i++)
            {
                int LevelHome = GameManager.Instance.sumHomeAll + i;
                if (i == 0)
                {
                    Location l = GameManager.Instance.lsLocation[id - 1];
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * l.lsWorking[l.countType].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * l.lsWorking[l.countType].maxOutputMade));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade * (1 + LevelHome / GameConfig.Instance.captruckIndex));
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
                else
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * lsWorking[i - 1].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * lsWorking[i - 1].maxOutputMade));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade * (1 + LevelHome / GameConfig.Instance.captruckIndex));
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
            }
        }
        else
        {
            for (int i = 0; i < lsWorking.Length; i++)
            {
                if (i == 0)
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = (double)(GameConfig.Instance.p0);
                    lsWorking[i].maxOutputMade = (double)(GameConfig.Instance.c0);
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade * (1 + i / GameConfig.Instance.captruckIndex));
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
                else
                {
                    lsWorking[i].UN2 = GameConfig.Instance.UN2;
                    lsWorking[i].price = Math.Floor((double)(GameConfig.Instance.p0i * lsWorking[i - 1].price));
                    lsWorking[i].maxOutputMade = Math.Floor((double)(GameConfig.Instance.c0i * lsWorking[i - 1].maxOutputMade));
                    lsWorking[i].maxOutputMadeStart = lsWorking[i].maxOutputMade;
                    lsWorking[i].priceUpgrade = Math.Floor((double)(GameConfig.Instance.UN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeStart = lsWorking[i].priceUpgrade;
                    lsWorking[i].priceTruckSent = 0;
                    lsWorking[i].priceTruckSentStart = lsWorking[i].priceTruckSent;
                    lsWorking[i].priceOutput = (double)GameConfig.Instance.productCost;
                    lsWorking[i].maxSent = Math.Floor(GameConfig.Instance.MaxSentStartX5 * lsWorking[i].maxOutputMade * (1 + i / GameConfig.Instance.captruckIndex));
                    lsWorking[i].maxSentStart = lsWorking[i].maxSent;
                    lsWorking[i].priceUpgradeTruck = Math.Floor((double)(GameConfig.Instance.XN1i * lsWorking[i].price));
                    lsWorking[i].priceUpgradeTruckStart = lsWorking[i].priceUpgradeTruck;
                }
            }
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
        UIManager.Instance.UpdateInfoUpgradeJob(
            lsWorking[indexType].name,
            lsWorking[indexType].level,
            lsWorking[indexType].level + 1,
            (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
            (Math.Floor(lsWorking[indexType].maxOutputMadeStart * GameConfig.Instance.r
                    * (1 + (double)(lsWorking[indexType].level + 1) / GameConfig.Instance.capIndex))),
            lsWorking[indexType].priceUpgrade
        );
        UIManager.Instance.JobUpgrade.SetActive(true);
        UIManager.Instance.arrXJob[0].transform.GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.arrXJob[1].transform.GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXJob[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXJob[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isJobX10 = false;
    }
    public void UpgradeInfoTypeOfWorkST(int idType)
    {
        indexType = idType;
        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgrade)
        {
            GameManager.Instance.dollar -= lsWorking[indexType].priceUpgrade;
            // Update thông số
            lsWorking[indexType].level++;
            lsWorking[indexType].maxOutputMade = Math.Floor((double)(((double)lsWorking[indexType].maxOutputMadeStart * (1 + (double)lsWorking[indexType].level / GameConfig.Instance.capIndex))));
            lsWorking[indexType].priceUpgrade = Math.Floor((double)((double)lsWorking[indexType].priceUpgrade * (1 + GameConfig.Instance.UN2)));
            lsWorking[indexType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].level);

            UIManager.Instance.UpdateInfoUpgradeJob(
                lsWorking[indexType].name,
                lsWorking[indexType].level,
                lsWorking[indexType].level + 1,
                (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
                (Math.Floor(lsWorking[indexType].maxOutputMadeStart
                        * GameConfig.Instance.r
                        * (1 + (double)(lsWorking[indexType].level + 1) / GameConfig.Instance.capIndex))),
                lsWorking[indexType].priceUpgrade
            );
            if (GameManager.Instance.dollar < lsWorking[indexType].priceUpgrade)
            {
                UIManager.Instance.btnUpgradeJob.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeJob.interactable = false;
        }
    }
    public void CheckInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        int level = lsWorking[indexType].level;
        double priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
        double priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;
        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTotal += priceUpgradeCurrent;
        }

        UIManager.Instance.UpdateInfoUpgradeJob(
            lsWorking[indexType].name,
            lsWorking[indexType].level,
            lsWorking[indexType].level + 10,
            (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
            (Math.Floor((double)(((double)lsWorking[indexType].maxOutputMadeStart
            * GameConfig.Instance.r * (1 + (double)(level + 10) / GameConfig.Instance.capIndex))))),
            priceUpgradeTotal
        );
        UIManager.Instance.JobUpgrade.SetActive(true);
    }
    public void UpgradeInfoTypeOfWorkSTX10(int idType)
    {
        indexType = idType;
        int level = lsWorking[indexType].level;
        double priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
        double priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;
        for (int i = level + 1; i < (level + 10); i++)
        {
            priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTotal += priceUpgradeCurrent;
        }

        if (GameManager.Instance.dollar >= priceUpgradeTotal)
        {
            GameManager.Instance.dollar -= priceUpgradeTotal;
            // Update thông số
            lsWorking[indexType].level += 10;
            lsWorking[indexType].maxOutputMade = Math.Floor((double)((lsWorking[indexType].maxOutputMadeStart * (1 + (double)(lsWorking[indexType].level) / GameConfig.Instance.capIndex))));
            lsWorking[indexType].priceUpgrade = priceUpgradeCurrent;
            lsWorking[indexType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].level);

            level = lsWorking[indexType].level;
            priceUpgradeTotal = lsWorking[indexType].priceUpgrade;
            priceUpgradeCurrent = lsWorking[indexType].priceUpgrade;

            for (int i = level + 1; i < (level + 10); i++)
            {
                priceUpgradeCurrent = Math.Floor((double)(priceUpgradeCurrent * (1 + GameConfig.Instance.UN2)));
                priceUpgradeTotal += priceUpgradeCurrent;
            }

            UIManager.Instance.UpdateInfoUpgradeJob(
                lsWorking[indexType].name,
                lsWorking[indexType].level,
                lsWorking[indexType].level + 10,
                (Math.Floor(lsWorking[indexType].maxOutputMade * GameConfig.Instance.r)),
                (Math.Floor((double)(((double)lsWorking[indexType].maxOutputMadeStart
                * GameConfig.Instance.r * (1 + (double)(level + 10) / GameConfig.Instance.capIndex))))),
                priceUpgradeTotal
            );

            if (GameManager.Instance.dollar < priceUpgradeTotal)
            {
                UIManager.Instance.btnUpgradeJob.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeJob.interactable = false;
        }
    }


    public void CheckInfoTruck(int idType)
    {
        if (PlayerPrefs.GetInt("isTutorial") == 0 && !UIManager.Instance.isClickTrunk)
            return;
        indexType = idType;
        UIManager.Instance.UpdateInfoUpgradeTruck(
            lsWorking[indexType].name,
            lsWorking[indexType].levelTruck,
            lsWorking[indexType].levelTruck + 1,
            lsWorking[indexType].maxSent,
            (Math.Floor((double)(lsWorking[indexType].maxSentStart * (1f + (double)(lsWorking[indexType].levelTruck + 1) / (double)GameConfig.Instance.captruckIndex)))),
            lsWorking[indexType].priceUpgradeTruck
        );
        UIManager.Instance.TruckUpgrade.SetActive(true);
        UIManager.Instance.arrXTrunk[0].transform.GetChild(1).gameObject.SetActive(true);
        UIManager.Instance.arrXTrunk[1].transform.GetChild(1).gameObject.SetActive(false);
        UIManager.Instance.arrXTrunk[0].color = new Color32(255, 255, 255, 255);
        UIManager.Instance.arrXTrunk[1].color = new Color32(255, 255, 255, 128);
        UIManager.Instance.isTrunkX10 = false;
        if (idType == 0)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                GameConfig.Instance.TruckSpeed = UIManager.Instance.speedTrunkTutorial;
                if (UIManager.Instance.objTutorial != null)
                {
                    Destroy(UIManager.Instance.objTutorial);
                }
                UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnUpgradeTrunk.transform);
                UIManager.Instance.txtWait.text = "Upgrade the capacity of the truck";
            }
        }
    }
    public void UpgradeInfoTruck(int idType)
    {
        indexType = idType;
        if (GameManager.Instance.dollar >= lsWorking[indexType].priceUpgradeTruck)
        {
            GameManager.Instance.dollar -= lsWorking[indexType].priceUpgradeTruck;
            // Update thông số
            lsWorking[indexType].levelTruck++;
            lsWorking[indexType].maxSent = Math.Floor((double)(lsWorking[indexType].maxSentStart * (1f + (double)lsWorking[indexType].levelTruck / (double)GameConfig.Instance.captruckIndex)));
            lsWorking[indexType].priceUpgradeTruck = Math.Floor((double)(lsWorking[indexType].priceUpgradeTruck * (1 + GameConfig.Instance.UN2)));
            lsWorking[indexType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].levelTruck);
            UIManager.Instance.UpdateInfoUpgradeTruck(
                lsWorking[indexType].name,
                lsWorking[indexType].levelTruck,
                lsWorking[indexType].levelTruck + 1,
                lsWorking[indexType].maxSent,
                (Math.Floor((double)(lsWorking[indexType].maxSentStart * (1f + (double)(lsWorking[indexType].levelTruck + 1) / (double)GameConfig.Instance.captruckIndex)))),
                lsWorking[indexType].priceUpgradeTruck
            );
            if (GameManager.Instance.dollar < lsWorking[indexType].priceUpgradeTruck)
            {
                UIManager.Instance.btnUpgradeTrunk.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeTrunk.interactable = false;
        }
    }
    public void CheckInfoTruckX10(int idType)
    {
        indexType = idType;
        int levelTruck = lsWorking[indexType].levelTruck;
        double priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
        double priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;

        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
        }

        UIManager.Instance.UpdateInfoUpgradeTruck(
                lsWorking[indexType].name,
                lsWorking[indexType].levelTruck,
                lsWorking[indexType].levelTruck + 10,
                lsWorking[indexType].maxSent,
                (Math.Floor((double)(lsWorking[indexType].maxSentStart * (1f + (double)(lsWorking[indexType].levelTruck + 10) / (double)GameConfig.Instance.captruckIndex)))),
                priceUpgradeTruckTotal
            );
        UIManager.Instance.TruckUpgrade.SetActive(true);
    }
    public void UpgradeInfoTruckX10(int idType)
    {
        indexType = idType;
        int levelTruck = lsWorking[indexType].levelTruck;
        double priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
        double priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;

        for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
        {
            priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.UN2)));
            priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
        }
        if (GameManager.Instance.dollar >= priceUpgradeTruckTotal)
        {
            GameManager.Instance.dollar -= priceUpgradeTruckTotal;
            // Update thông số
            lsWorking[indexType].levelTruck += 10;
            lsWorking[indexType].maxSent = Math.Floor((double)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck) / (float)GameConfig.Instance.captruckIndex)));
            lsWorking[indexType].priceUpgradeTruck = priceUpgradeTruckCurrent;
            lsWorking[indexType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[indexType].levelTruck);

            levelTruck = lsWorking[indexType].levelTruck;
            priceUpgradeTruckTotal = lsWorking[indexType].priceUpgradeTruck;
            priceUpgradeTruckCurrent = lsWorking[indexType].priceUpgradeTruck;

            for (int i = levelTruck + 1; i < (levelTruck + 10); i++)
            {
                priceUpgradeTruckCurrent = Math.Floor((double)(priceUpgradeTruckCurrent * (1 + GameConfig.Instance.UN2)));
                priceUpgradeTruckTotal += priceUpgradeTruckCurrent;
            }

            UIManager.Instance.UpdateInfoUpgradeTruck(
               lsWorking[indexType].name,
               lsWorking[indexType].levelTruck,
               lsWorking[indexType].levelTruck + 10,
               lsWorking[indexType].maxSent,
               (Math.Floor((double)((float)lsWorking[indexType].maxSentStart * (1f + (float)(lsWorking[indexType].levelTruck + 10) / (float)GameConfig.Instance.captruckIndex)))),
               priceUpgradeTruckTotal
           );
            if (GameManager.Instance.dollar < priceUpgradeTruckTotal)
            {
                UIManager.Instance.btnUpgradeTrunk.interactable = false;
            }
        }
        else
        {
            UIManager.Instance.btnUpgradeTrunk.interactable = false;
        }
    }


    public void SellJob()
    {
        if (GameManager.Instance.dollar >= lsWorking[countType + 1].price)
        {
            GameManager.Instance.dollar -= lsWorking[countType + 1].price;
            countType++;
            indexType = countType;
            lsWorking[countType].icon.color = Color.white;
            lsWorking[countType].animLock.gameObject.SetActive(false);
            lsWorking[countType].info.SetActive(true);
            lsWorking[countType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].input);
            lsWorking[countType].textOutput.text = UIManager.Instance.ConvertNumber(lsWorking[countType].output);
            lsWorking[countType].textLevel.text = UIManager.Instance.ConvertNumber(lsWorking[countType].level);
            lsWorking[countType].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(lsWorking[countType].levelTruck);
            if (countType + 1 == lsWorking.Length)
            {
                if (id + 1 < UIManager.Instance.lsLocationUI.Count)
                {
                    int indexLsLocation = GameManager.Instance.lsLocation.Count;
                    GameManager.Instance.CreatLocation(UIManager.Instance.lsLocationUI[indexLsLocation]);
                    UIManager.Instance.handWorld.position = UIManager.Instance.lsLocationUI[indexLsLocation].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f);
                    GameManager.Instance.sumHomeAll += lsWorking.Length;
                }
            }
            else
            {
                lsWorking[countType + 1].animLock.enabled = true;
                if (id == GameManager.Instance.lsLocation.Count - 1)
                {
                    UIManager.Instance.txtRevenue.text
                        = "Revenue : " + UIManager.Instance.ConvertNumber(
                            lsWorking[countType].maxOutputMade
                            * GameConfig.Instance.r
                            * GameConfig.Instance.productCost
                            ) + "$/day";
                }
            }
            if (id > 0 && countType == 0)
            {
                GameManager.Instance.lsLocation[id - 1].forest.btnAutoPlant.gameObject.SetActive(true);
                GameManager.Instance.lsLocation[id - 1].forest.isOnBtnAutoPlant = true;
            }
            if(countType == 0)
            {
                UIManager.Instance.WarningForest.SetActive(true);
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
                    if (id == GameManager.Instance.IDLocation && !forest.isAutoPlant)
                    {
                        if (forest.tree == 0 && !UIManager.Instance.WarningForest.activeInHierarchy)
                        {
                            UIManager.Instance.WarningForest.SetActive(true);
                        }
                    }
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

        lsWorking[0].output += Math.Floor((double)(GameConfig.Instance.r * lsWorking[0].maxOutputMade));
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
        double materialCurrent = 0;
        if (lsWorking[idType].input >= lsWorking[idType].maxOutputMade)
        {
            materialCurrent = lsWorking[idType].maxOutputMade;
        }
        else
        {
            materialCurrent = lsWorking[idType].input;
        }
        lsWorking[idType].input -= Math.Floor(materialCurrent);
        lsWorking[idType].textInput.text = UIManager.Instance.ConvertNumber(lsWorking[idType].input);
        lsWorking[idType].output += Math.Floor((double)(GameConfig.Instance.r * materialCurrent));
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
        if (countType >= 0)
        {
            FellingForest();
            Felling();
        }
        for (int i = 1; i <= countType; i++)
        {
            Job(i);
        }

        if (!forest.isAutoPlant && forest.isOnBtnAutoPlant)
        {
            if (GameManager.Instance.dollar >= Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant)))
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
            forest.btnAutoPlant.gameObject.SetActive(false);
        }
    }


    public void AutoPlant()
    {
        string str = "Do you want to spend " + UIManager.Instance.ConvertNumber(Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant))) + "$ On Auto Reforestation in " + nameLocation;
        UIManager.Instance.PopupAutoPlant.SetActive(true);
        UIManager.Instance.PopupAutoPlant.GetComponent<AutoPlant>().AutoPlant_Onclick(str, () =>
        {
            if (GameManager.Instance.dollar >= Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant)))
            {
                GameManager.Instance.dollar -= Math.Floor((double)(lsWorking[0].price * GameConfig.Instance.AutoPlant));
                forest.isAutoPlant = true;
                forest.btnAutoPlant.interactable = false;
                UIManager.Instance.PopupAutoPlant.SetActive(false);
                forest.btnAutoPlant.gameObject.SetActive(false);
                if (forest.tree <= 0)
                {
                    forest.forestClass.RunCarGrow();
                    UIManager.Instance.WarningForest.SetActive(false);
                }
            }
        });
    }


    public void WordYourSelf(int idType)
    {
        UIManager.Instance.scene = TypeScene.MINIGAME;
        indexType = idType;
        GameManager.Instance.lsTypeMiniGame[indexTypeWork].lsMiniGame[indexType].miniGame.SetActive(true);
        lsWorking[indexType].isXJob = true;
        if (PlayerPrefs.GetInt("isTutorial") == 0 && UIManager.Instance.isClickHome)
        {
            if (UIManager.Instance.objTutorial != null)
            {
                Destroy(UIManager.Instance.objTutorial);
            }
            UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnUpgradeTutorial);
            UIManager.Instance.txtWait.text = "Tap to Upgrade";
            UIManager.Instance.isClickHome = false;
        }
    }
    public void HomeOnclick(int idType)
    {
        indexType = idType;
        if (countType >= idType)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0 && !UIManager.Instance.isClickHome)
                return;
            WordYourSelf(idType);
        }
        else if (countType + 1 == idType)
        {
            if (PlayerPrefs.GetInt("isTutorial") == 0 && idType != 0)
                return;
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                if (!UIManager.Instance.popupTutorial.activeInHierarchy)
                {
                    UIManager.Instance.popupTutorial.SetActive(true);
                }
                if (UIManager.Instance.objTutorial != null)
                {
                    Destroy(UIManager.Instance.objTutorial);
                }
                UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnSell.transform);
                UIManager.Instance.txtWait.text = "Buy Felling workshop";
            }
            UIManager.Instance.isJobX10 = false;
            UIManager.Instance.UpdateInfoSellJob(lsWorking[idType].name,
            lsWorking[idType].icon.sprite,
            lsWorking[idType].price);
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
