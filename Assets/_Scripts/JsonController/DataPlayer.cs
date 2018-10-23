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
    public long dollar;

    [HideInInspector]
    public long gold;

    [HideInInspector]
    public int sumHomeAll;

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
            GameManager.Instance.gold = objJson["gold"].AsLong;
            GameManager.Instance.dollar = objJson["dollar"].AsLong;
            GameManager.Instance.sumHomeAll = objJson["sumHomeAll"].AsInt;
            GameManager.Instance.dateStartPlay = DateTime.Parse(objJson["dateStartPlay"]);
            GameManager.Instance.dateGame = DateTime.Parse(objJson["dateGame"]);

            var lsData = objJson["lsLocation"].AsArray;
            lsLocation = new List<LocationJSON>();
            GameManager.Instance.ClearLocation();
            GameManager.Instance.lsLocation = new List<Location>();
            StartCoroutine(IELoadLocationJson(lsData));
            GameManager.Instance.locationManager.gameObject.SetActive(true);
            GameManager.Instance.locationManager.SetAsFirstSibling();

        }

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

            location.forest.tree = lsData[i]["forest"]["tree"].AsInt;
            location.forest.isOnBtnAutoPlant = lsData[i]["forest"]["isOnBtnAutoPlant"].AsBool;
            location.forest.isAutoPlant = lsData[i]["forest"]["isAutoPlant"].AsBool;
            location.forest.forestClass.LoadTree();

            if ((i + 1) != lsData.Count && location.countType >= 0)
            {
                if (!location.forest.isAutoPlant)
                {
                    location.forest.btnAutoPlant.interactable = true;
                }
            }

            var lsWorking = lsData[i]["lsWorking"].AsArray;
            for (int j = 0; j < lsWorking.Count; j++)
            {
                if (location.countType >= j) location.lsWorking[j].icon.color = Color.white;

                location.lsWorking[j].name = lsWorking[j]["name"];
                location.lsWorking[j].id = lsWorking[j]["id"].AsInt;
                location.lsWorking[j].level = lsWorking[j]["level"].AsInt;

                location.lsWorking[j].input = lsWorking[j]["input"].AsLong;
                location.lsWorking[j].output = lsWorking[j]["output"].AsLong;
                location.lsWorking[j].priceOutput = lsWorking[j]["priceOutput"].AsLong;

                location.lsWorking[j].maxOutputMade = lsWorking[j]["maxOutputMade"].AsLong;
                location.lsWorking[j].maxOutputMadeStart = lsWorking[j]["maxOutputMadeStart"].AsLong;

                location.lsWorking[j].levelTruck = lsWorking[j]["levelTruck"].AsInt;
                location.lsWorking[j].priceUpgradeTruck = lsWorking[j]["priceUpgradeTruck"].AsLong;
                location.lsWorking[j].priceUpgradeTruckStart = lsWorking[j]["priceUpgradeTruckStart"].AsLong;
                location.lsWorking[j].priceTruckSent = lsWorking[j]["priceTruckSent"].AsLong;
                location.lsWorking[j].priceTruckSentStart = lsWorking[j]["priceTruckSentStart"].AsLong;
                location.lsWorking[j].currentSent = lsWorking[j]["currentSent"].AsLong;
                location.lsWorking[j].maxSent = lsWorking[j]["maxSent"].AsLong;
                location.lsWorking[j].maxSentStart = lsWorking[j]["maxSentStart"].AsLong;

                location.lsWorking[j].priceUpgrade = lsWorking[j]["priceUpgrade"].AsLong;
                location.lsWorking[j].priceUpgradeStart = lsWorking[j]["priceUpgradeStart"].AsLong;
                location.lsWorking[j].price = lsWorking[j]["price"].AsLong;
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
                }
            }
            if (!isFirst)
            {
                if (UIManager.Instance.isContinue)
                {
                    totalTime = (long)((TimeSpan)(dateNowPlayer - DateTime.Parse(PlayerPrefs.GetString("DateTimeOutGame")))).TotalHours;
                    if (totalTime > 0)
                    {
                        if (totalTime * 10 > 100)
                            totalTime = 10;
                        long adddollar = totalTime * 10 * location.lsWorking[location.countType].priceOutput;
                        GameManager.Instance.dollar += adddollar;
                        GameManager.Instance.dollarGive += adddollar;

                    }
                }
                isFirst = true;
            }
            GameManager.Instance.lsLocation.Add(location);
            UIManager.Instance.lsBtnLocationUI[i].interactable = true;
        }

        yield return new WaitUntil(() => UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].interactable);
        UIManager.Instance.handWorld.position = UIManager.Instance.lsBtnLocationUI[lsData.Count - 1].transform.GetChild(0).position - new Vector3(0f, 0.25f, 0f); ;
        if (totalTime > 0)
        {
            string strGive = "You were offline for "
                + UIManager.Instance.ConvertNumber(totalTime)
                + " hours \n You have just recived "
                + UIManager.Instance.ConvertNumber(GameManager.Instance.dollarGive)
                + "$";
            UIManager.Instance.PushGiveGold(strGive);
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }

        ScenesManager.Instance.isNextScene = true;

    }

    private void OnDestroy()
    {
        if (UIManager.Instance.isSaveJson)
        {
            SaveDataPlayer();
            PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (UIManager.Instance.isSaveJson)
            {
                SaveDataPlayer();
                PlayerPrefs.SetString("DateTimeOutGame", DateTime.Now.ToString());
            }
        }
    }

}
