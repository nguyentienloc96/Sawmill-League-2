using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance = new DataPlayer();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public long dollar;

    public long gold;

    public string dateStartPlay;

    public string dateGame;

    public List<LocationJSON> lsLocation = new List<LocationJSON>();

    public void SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.dollar = GameManager.Instance.dollar;
        data.gold = GameManager.Instance.gold;
        data.dateStartPlay = GameManager.Instance.dateStartPlay.ToString();
        data.dateGame = GameManager.Instance.dateGame.ToString();
        data.lsLocation = new List<LocationJSON>();
        for (int i = 0; i < GameManager.Instance.lsLocation.Count; i++)
        {
            LocationJSON lc = new LocationJSON();
            lc._ID = GameManager.Instance.lsLocation[i]._ID;
            lc._Name = GameManager.Instance.lsLocation[i]._Name;
            lc._CountType = GameManager.Instance.lsLocation[i]._CountType;
            lc._Forest._Tree = GameManager.Instance.lsLocation[i]._Forest._Tree;
            lc._LsWorking = new TypeOfWorkST[GameManager.Instance.lsLocation[i]._LsWorking.Length];
            for (int j = 0; j < GameManager.Instance.lsLocation[i]._LsWorking.Length; j++)
            {
                lc._LsWorking[j]._Name = GameManager.Instance.lsLocation[i]._LsWorking[j]._Name;
                lc._LsWorking[j]._ID = GameManager.Instance.lsLocation[i]._LsWorking[j]._ID;
                lc._LsWorking[j]._Level = GameManager.Instance.lsLocation[i]._LsWorking[j]._Level;

                lc._LsWorking[j]._Material = GameManager.Instance.lsLocation[i]._LsWorking[j]._Material;
                lc._LsWorking[j]._MaterialReceive = GameManager.Instance.lsLocation[i]._LsWorking[j]._MaterialReceive;
                lc._LsWorking[j]._PriceMaterial = GameManager.Instance.lsLocation[i]._LsWorking[j]._PriceMaterial;

                lc._LsWorking[j]._NumberMaterialIsMade = GameManager.Instance.lsLocation[i]._LsWorking[j]._NumberMaterialIsMade;

                lc._LsWorking[j]._LevelTransport = GameManager.Instance.lsLocation[i]._LsWorking[j]._LevelTransport;
                lc._LsWorking[j]._PriceUpgradeCar = GameManager.Instance.lsLocation[i]._LsWorking[j]._PriceUpgradeCar;
                lc._LsWorking[j]._PriceTransportSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._PriceTransportSent;
                lc._LsWorking[j]._NumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._NumberOfMaterialsSent;
                lc._LsWorking[j]._MaxNumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._MaxNumberOfMaterialsSent;

                lc._LsWorking[j]._PriceUpgrade = GameManager.Instance.lsLocation[i]._LsWorking[j]._PriceUpgrade;
                lc._LsWorking[j]._Price = GameManager.Instance.lsLocation[i]._LsWorking[j]._Price;
                lc._LsWorking[j]._UN2 = GameManager.Instance.lsLocation[i]._LsWorking[j]._UN2;
            }
            data.lsLocation.Add(lc);
        }


        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
    }

    public void LoadDataPlayer()
    {
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        var objJson = SimpleJSON.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            GameManager.Instance.dollar = objJson["dollar"].AsLong;
            GameManager.Instance.gold = objJson["gold"].AsLong;
            GameManager.Instance.dateStartPlay = DateTime.Parse(objJson["dateStartPlay"]);
            GameManager.Instance.dateGame = DateTime.Parse(objJson["dateGame"]);

            var lsData = objJson["lsLocation"].AsArray;
            lsLocation = new List<LocationJSON>();
            GameManager.Instance.ClearLocation();
            GameManager.Instance.lsLocation = new List<Location>();
            UIManager.Instance.MaskLocation.Clear();
            for (int i = 0; i < lsData.Count; i++)
            {
                GameObject obj = Instantiate(GameManager.Instance.itemLocation, GameManager.Instance.locationManager);
                obj.transform.SetAsFirstSibling();
                Location ljson = obj.GetComponent<Location>();
                ljson.isCanStart = true;
                obj.name = lsData[i]["_Name"];

                ljson._Name = lsData[i]["_Name"];
                ljson._ID = lsData[i]["_ID"].AsInt;
                ljson._CountType = lsData[i]["_CountType"].AsInt;

                ljson._Forest._Tree = lsData[i]["_Forest"]["_Tree"].AsInt;

                ljson.GetActionTransports();
                var lsWorking = lsData[i]["_LsWorking"].AsArray;
                for (int j = 0; j < lsWorking.Count; j++)
                {
                    if (ljson._CountType >= j) ljson._LsWorking[j]._Icon.color = new Color32(255, 255, 255, 255);

                    ljson._LsWorking[j]._Name = lsWorking[j]["_Name"];
                    ljson._LsWorking[j]._ID = lsWorking[j]["_ID"].AsInt;
                    ljson._LsWorking[j]._Level = lsWorking[j]["_Level"].AsInt;

                    ljson._LsWorking[j]._Material = lsWorking[j]["_Material"].AsLong;
                    ljson._LsWorking[j]._MaterialReceive = lsWorking[j]["_MaterialReceive"].AsLong;
                    ljson._LsWorking[j]._PriceMaterial = lsWorking[j]["_PriceMaterial"].AsLong;

                    ljson._LsWorking[j]._NumberMaterialIsMade = lsWorking[j]["_NumberMaterialIsMade"].AsLong;

                    ljson._LsWorking[j]._LevelTransport = lsWorking[j]["_LevelTransport"].AsInt;
                    ljson._LsWorking[j]._PriceUpgradeCar = lsWorking[j]["_PriceUpgradeCar"].AsLong;
                    ljson._LsWorking[j]._PriceTransportSent = lsWorking[j]["_PriceTransportSent"].AsLong;
                    ljson._LsWorking[j]._NumberOfMaterialsSent = lsWorking[j]["_NumberOfMaterialsSent"].AsLong;
                    ljson._LsWorking[j]._MaxNumberOfMaterialsSent = lsWorking[j]["_MaxNumberOfMaterialsSent"].AsLong;
                    ljson._LsWorking[j]._PriceUpgrade = lsWorking[j]["_PriceUpgrade"].AsLong;
                    ljson._LsWorking[j]._Price = lsWorking[j]["_Price"].AsLong;
                    ljson._LsWorking[j]._UN2 = lsWorking[j]["_UN2"].AsLong;

                    ljson._LsWorking[j]._TransportType.isRun = false;
                    if (ljson._LsWorking[j]._ID != ljson._LsWorking.Length && ljson._LsWorking[j]._Material > 0) ljson._LsWorking[j]._TransportType.ResetAll();
                }
                ljson._Forest._ForestCode.LoadTree();
                UIManager.Instance.MaskLocation.Add(ljson._MaskLocation);
                GameManager.Instance.lsLocation.Add(ljson);
                UIManager.Instance.lsLocationUI[i].interactable = true;
 
            }
            GameManager.Instance.locationManager.gameObject.SetActive(true);
            GameManager.Instance.locationManager.SetAsFirstSibling();
        }

    }

    private void OnDestroy()
    {
        SaveDataPlayer();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            SaveDataPlayer();
        }
    }

}
