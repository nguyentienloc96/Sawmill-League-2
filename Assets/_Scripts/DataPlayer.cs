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
                lc._LsWorking[j]._Material = GameManager.Instance.lsLocation[i]._LsWorking[j]._Material;
                lc._LsWorking[j]._MaterialReceive = GameManager.Instance.lsLocation[i]._LsWorking[j]._MaterialReceive;
                lc._LsWorking[j]._NumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._NumberOfMaterialsSent;
                lc._LsWorking[j]._MaxNumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._MaxNumberOfMaterialsSent;
                lc._LsWorking[j]._Price = GameManager.Instance.lsLocation[i]._LsWorking[j]._Price;
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
                Location ljson = obj.GetComponent<Location>();
                obj.name = lsData[i]["_Name"]; ;
                ljson._ID = lsData[i]["_ID"].AsInt;
                ljson._Name = lsData[i]["_Name"];
                ljson._CountType = lsData[i]["_CountType"].AsInt;
                ljson._Forest._Tree = lsData[i]["_Forest"]["_Tree"].AsInt;
                var lsWorking = lsData[i]["_LsWorking"].AsArray;
                for (int j = 0; j < lsWorking.Count; j++)
                {
                    ljson._LsWorking[j]._Material = lsWorking[j]["_Material"].AsLong;
                    ljson._LsWorking[j]._MaterialReceive = lsWorking[j]["_MaterialReceive"].AsLong;
                    ljson._LsWorking[j]._NumberOfMaterialsSent = lsWorking[j]["_NumberOfMaterialsSent"].AsLong;
                    ljson._LsWorking[j]._MaxNumberOfMaterialsSent = lsWorking[j]["_MaxNumberOfMaterialsSent"].AsLong;
                    ljson._LsWorking[j]._Price = lsWorking[j]["_Price"].AsLong;
                    ljson._LsWorking[j]._TransportType.ActionSented = ljson.SentedWood;
                    ljson._LsWorking[j]._TransportType.ActionReceived = ljson.ReceivedWood;
                    ljson._LsWorking[j]._TransportType.PauseRun();
                    if (ljson._LsWorking[j]._Material > 0)
                    {
                        ljson._LsWorking[j]._TransportType.ResetAll();
                    }
                }
                ljson._Forest._ForestCode.LoadTree();
                UIManager.Instance.MaskLocation.Add(ljson._MaskLocation);
                GameManager.Instance.lsLocation.Add(ljson);
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
