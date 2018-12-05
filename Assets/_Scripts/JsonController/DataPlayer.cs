using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        if (!PlayerPrefs.HasKey("DateTimeOutGame"))
        {
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    [HideInInspector]
    public double dollar;

    [HideInInspector]
    public double gold;

    [HideInInspector]
    public int sumHomeAll;

    [HideInInspector]
    public int indexSawmill;

    [HideInInspector]
    public string dateStartPlay;

    [HideInInspector]
    public string dateGame;

    [HideInInspector]
    public List<LocationJSON> lsLocation;
    private bool isFirst;
    private DateTime dateNowPlayer;

    public void SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.gold;
        data.dollar = GameManager.Instance.dollar;
        data.sumHomeAll = GameManager.Instance.sumHomeAll;
        data.indexSawmill = GameManager.Instance.indexSawmill;
        data.dateStartPlay = GameManager.Instance.dateStartPlay.ToString();
        data.dateGame = GameManager.Instance.dateGame.ToString();
        data.lsLocation = new List<LocationJSON>();
        for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
        {
            LocationJSON locationJson = new LocationJSON();
            locationJson.id = GameManager.Instance.lsLocation[i].id;
            locationJson.nameLocation = GameManager.Instance.lsLocation[i].nameLocation;
            locationJson.indexTypeWork = GameManager.Instance.lsLocation[i].indexTypeWork;
            locationJson.countType = GameManager.Instance.lsLocation[i].countType;
            locationJson.indexType = GameManager.Instance.lsLocation[i].indexType;
            locationJson.makerType = GameManager.Instance.lsLocation[i].makerType;

            locationJson.forest = GameManager.Instance.lsLocation[i].forest;
            locationJson.lsWorking = GameManager.Instance.lsLocation[i].lsWorking;

            locationJson.lsOther = GameManager.Instance.lsLocation[i].lsOther;
            locationJson.lsRiverLeft = GameManager.Instance.lsLocation[i].lsRiverLeft;
            locationJson.lsRiverRight = GameManager.Instance.lsLocation[i].lsRiverRight;
            locationJson.lsStreet = GameManager.Instance.lsLocation[i].lsStreet;

            data.lsLocation.Add(locationJson);
        }


        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
        PlayerPrefs.SetInt("Continue", 1);

        Debug.Log(SimpleJSON_DatDz.JSON.Parse(File.ReadAllText(_path)));

    }

    public void LoadDataPlayer()
    {
        dateNowPlayer = DateTime.Now;
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        var objJson = SimpleJSON_DatDz.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            StartCoroutine(IEClearLocation(objJson));
        }

    }

    public IEnumerator IEClearLocation(SimpleJSON_DatDz.JSONNode objJson)
    {
        GameManager.Instance.ClearLocation();
        yield return new WaitUntil(() => GameManager.Instance.lsLocation.Count == 0);
        GameManager.Instance.gold = objJson["gold"].AsDouble;
        GameManager.Instance.dollar = objJson["dollar"].AsDouble;
        GameManager.Instance.sumHomeAll = objJson["sumHomeAll"].AsInt;
        GameManager.Instance.indexSawmill = objJson["indexSawmill"].AsInt;
        GameManager.Instance.dateStartPlay = DateTime.Parse(objJson["dateStartPlay"]);
        GameManager.Instance.dateGame = DateTime.Parse(objJson["dateGame"]);

        var lsData = objJson["lsLocation"].AsArray;
        lsLocation = new List<LocationJSON>();
        GameManager.Instance.lsLocation = new List<Location>();
        StartCoroutine(IELoadLocationJson(lsData));
        GameManager.Instance.locationManager.gameObject.SetActive(true);
        GameManager.Instance.locationManager.SetAsFirstSibling();
    }

    public IEnumerator IELoadLocationJson(SimpleJSON_DatDz.JSONArray lsData)
    {
        long totalTime = 0;
        for (int i = 0; i < lsData.Count; i++)
        {
            int indexTypeWork = lsData[i]["indexTypeWork"].AsInt;
            GameObject obj = Instantiate(GameManager.Instance.lsItemLocation[indexTypeWork], GameManager.Instance.locationManager);
            obj.transform.SetAsFirstSibling();
            obj.name = lsData[i]["nameLocation"];
            Location location = obj.GetComponent<Location>();

            var lsOther = lsData[i]["lsOther"].AsArray;
            location.lsOther = new List<int>();
            for (int iOther = 0; iOther < lsOther.Count; iOther++)
            {
                location.lsOther.Add(lsOther[iOther].AsInt);
            }

            var lsRiverRight = lsData[i]["lsRiverRight"].AsArray;
            location.lsRiverRight = new List<int>();
            for (int iRiverRight = 0; iRiverRight < lsRiverRight.Count; iRiverRight++)
            {
                location.lsRiverRight.Add(lsRiverRight[iRiverRight].AsInt);
            }

            var lsRiverLeft = lsData[i]["lsRiverLeft"].AsArray;
            location.lsRiverLeft = new List<int>();
            for (int iRiverLeft = 0; iRiverLeft < lsRiverLeft.Count; iRiverLeft++)
            {
                location.lsRiverLeft.Add(lsRiverLeft[iRiverLeft].AsInt);
            }

            var lsStreet = lsData[i]["lsStreet"].AsArray;
            location.lsStreet = new List<int>();
            for (int iStreet = 0; iStreet < lsStreet.Count; iStreet++)
            {
                location.lsStreet.Add(lsStreet[iStreet].AsInt);
            }

            yield return new WaitUntil(() =>
            location.lsStreet.Count == lsStreet.Count
            && location.lsRiverLeft.Count == lsRiverLeft.Count
            && location.lsRiverRight.Count == lsRiverRight.Count
            && location.lsOther.Count == lsOther.Count);

            location.LoadLocationJson();

            yield return new WaitUntil(() => location.isLoadFull);

            location.id = lsData[i]["id"].AsInt;
            location.nameLocation = lsData[i]["nameLocation"];
            location.indexTypeWork = lsData[i]["indexTypeWork"].AsInt;
            location.countType = lsData[i]["countType"].AsInt;
            location.makerType = lsData[i]["makerType"].AsInt;

            location.forest.tree = lsData[i]["forest"]["tree"].AsInt;
            location.forest.isOnBtnAutoPlant = lsData[i]["forest"]["isOnBtnAutoPlant"].AsBool;
            location.forest.isAutoPlant = lsData[i]["forest"]["isAutoPlant"].AsBool;
            location.forest.forestClass.LoadTree();

            var lsWorking = lsData[i]["lsWorking"].AsArray;
            for (int j = 0; j < lsWorking.Count; j++)
            {
                if (location.countType >= j) location.lsWorking[j].icon.color = Color.white;

                location.lsWorking[j].name = lsWorking[j]["name"];
                location.lsWorking[j].id = lsWorking[j]["id"].AsInt;
                location.lsWorking[j].level = lsWorking[j]["level"].AsInt;

                location.lsWorking[j].input = lsWorking[j]["input"].AsDouble;
                location.lsWorking[j].output = lsWorking[j]["output"].AsDouble;
                location.lsWorking[j].priceOutput = lsWorking[j]["priceOutput"].AsDouble;

                location.lsWorking[j].maxOutputMade = lsWorking[j]["maxOutputMade"].AsDouble;
                location.lsWorking[j].maxOutputMadeStart = lsWorking[j]["maxOutputMadeStart"].AsDouble;

                location.lsWorking[j].levelTruck = lsWorking[j]["levelTruck"].AsInt;
                location.lsWorking[j].priceUpgradeTruck = lsWorking[j]["priceUpgradeTruck"].AsDouble;
                location.lsWorking[j].priceUpgradeTruckStart = lsWorking[j]["priceUpgradeTruckStart"].AsDouble;
                location.lsWorking[j].priceTruckSent = lsWorking[j]["priceTruckSent"].AsDouble;
                location.lsWorking[j].priceTruckSentStart = lsWorking[j]["priceTruckSentStart"].AsDouble;
                location.lsWorking[j].currentSent = lsWorking[j]["currentSent"].AsDouble;
                location.lsWorking[j].maxSent = lsWorking[j]["maxSent"].AsDouble;
                location.lsWorking[j].maxSentStart = lsWorking[j]["maxSentStart"].AsDouble;

                location.lsWorking[j].priceUpgrade = lsWorking[j]["priceUpgrade"].AsDouble;
                location.lsWorking[j].priceUpgradeStart = lsWorking[j]["priceUpgradeStart"].AsDouble;
                location.lsWorking[j].price = lsWorking[j]["price"].AsDouble;
                location.lsWorking[j].UN2 = lsWorking[j]["UN2"].AsFloat;
                if (location.lsWorking[j].id <= location.countType)
                {
                    location.lsWorking[j].info.SetActive(true);
                    if (location.lsWorking[j].textInput != null)
                    {
                        location.lsWorking[j].textInput.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].input);
                    }
                    location.lsWorking[j].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].output);
                    if (location.lsWorking[j].output > 0)
                    {
                        location.lsWorking[j].truckManager.LoadTruck();
                    }
                    location.lsWorking[j].textLevel.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].level);
                    location.lsWorking[j].truckManager.txtLevel.text = UIManager.Instance.ConvertNumber(location.lsWorking[j].levelTruck);
                }
                if (i != lsData.Count - 1)
                {
                    location.lsWorking[j].animLock.gameObject.SetActive(false);
                }
                else
                {
                    if (location.lsWorking[j].id <= location.countType)
                    {
                        location.lsWorking[j].animLock.gameObject.SetActive(false);
                    }
                    else if (location.lsWorking[j].id == location.countType + 1)
                    {
                        location.lsWorking[j].animLock.enabled = true;
                    }
                }
            }

            yield return new WaitUntil(() => location.lsWorking[0].price != 0);

            if (!location.forest.isAutoPlant)
            {
                if (location.forest.isOnBtnAutoPlant)
                {
                    location.forest.btnAutoPlant.gameObject.SetActive(true);
                    if (GameManager.Instance.dollar >= (double)(location.lsWorking[0].price * GameConfig.Instance.AutoPlant))
                    {
                        location.forest.btnAutoPlant.interactable = true;
                    }
                    else
                    {
                        location.forest.btnAutoPlant.interactable = false;
                    }
                }
            }
            GameManager.Instance.lsLocation.Add(location);
            UIManager.Instance.lsBtnLocationUI[i].interactable = true;
        }
        yield return new WaitUntil(() => UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].interactable);

        UIManager.Instance.handWorld.position = UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f); ;

        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        if (jobEnd == -1 && GameManager.Instance.lsLocation.Count > 1)
        {
            locationEnd--;
            jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        }
        double dollarRecive = 0;
        double maxOutputMadeRevenue = 0;
        if (GameManager.Instance.lsLocation.Count > 1)
        {
            dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
            maxOutputMadeRevenue = GameManager.Instance.lsLocation[locationEnd]
            .lsWorking[jobEnd].maxOutputMade
            * GameConfig.Instance.r
            * GameConfig.Instance.productCost;
        }
        else
        {
            if (jobEnd == -1)
            {
                dollarRecive = 0;
                maxOutputMadeRevenue = 0;
            }
            else
            {
                dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
                maxOutputMadeRevenue = GameManager.Instance.lsLocation[locationEnd]
                .lsWorking[jobEnd].maxOutputMade
                * GameConfig.Instance.r
                * GameConfig.Instance.productCost;
            }
        }
        UIManager.Instance.txtRevenue.text
        = "Revenue : " + UIManager.Instance.ConvertNumber(maxOutputMadeRevenue) + "$/day";
        ScenesManager.Instance.isNextScene = true;
        double adddollar = 0;
        if (!isFirst)
        {
            if (UIManager.Instance.isContinue)
            {
                totalTime = (long)((TimeSpan)(dateNowPlayer - DateTime.Parse(PlayerPrefs.GetString("DateTimeOutGame")))).TotalHours;
                if (totalTime > 0 && dollarRecive > 0)
                {
                    if (totalTime > 10)
                        totalTime = 10;
                    adddollar = (double)((double)totalTime * 0.5f * dollarRecive);
                    GameManager.Instance.AddDollar(+adddollar);
                    string strGive = "Offline Reward\n"
                    + UIManager.Instance.ConvertNumber(adddollar)
                    + "$";
                    UIManager.Instance.PushGiveGold(strGive);
                    PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
                }
            }
            isFirst = true;
        }
    }

    private void OnDestroy()
    {
        if (UIManager.Instance.isSaveJson && UIManager.Instance.scene != TypeScene.HOME)
        {
            SaveDataPlayer();
        }
        PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (UIManager.Instance.isSaveJson && UIManager.Instance.scene != TypeScene.HOME)
            {
                SaveDataPlayer();
            }
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }
    }

    public void SaveExit()
    {
        StartCoroutine(IESaveDataPlayer());
    }

    public IEnumerator IESaveDataPlayer()
    {
        int sumLocaton = 0;
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.gold;
        data.dollar = GameManager.Instance.dollar;
        data.sumHomeAll = GameManager.Instance.sumHomeAll;
        data.indexSawmill = GameManager.Instance.indexSawmill;
        data.dateStartPlay = GameManager.Instance.dateStartPlay.ToString();
        data.dateGame = GameManager.Instance.dateGame.ToString();
        data.lsLocation = new List<LocationJSON>();
        for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
        {
            LocationJSON locationJson = new LocationJSON();
            locationJson.id = GameManager.Instance.lsLocation[i].id;
            locationJson.nameLocation = GameManager.Instance.lsLocation[i].nameLocation;
            locationJson.indexTypeWork = GameManager.Instance.lsLocation[i].indexTypeWork;
            locationJson.countType = GameManager.Instance.lsLocation[i].countType;
            locationJson.indexType = GameManager.Instance.lsLocation[i].indexType;
            locationJson.makerType = GameManager.Instance.lsLocation[i].makerType;

            locationJson.forest = GameManager.Instance.lsLocation[i].forest;
            locationJson.lsWorking = GameManager.Instance.lsLocation[i].lsWorking;

            locationJson.lsOther = GameManager.Instance.lsLocation[i].lsOther;
            locationJson.lsRiverLeft = GameManager.Instance.lsLocation[i].lsRiverLeft;
            locationJson.lsRiverRight = GameManager.Instance.lsLocation[i].lsRiverRight;
            locationJson.lsStreet = GameManager.Instance.lsLocation[i].lsStreet;

            data.lsLocation.Add(locationJson);
            sumLocaton++;
        }

        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);

        yield return new WaitUntil(() => sumLocaton == GameManager.Instance.lsLocation.Count);

        PlayerPrefs.SetInt("Continue", 1);

        GameManager.Instance.ClearLocation();
    }

}
