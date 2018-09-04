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

    public long dollars;

    public long gold;

    public string dateStartPlay;

    public string dateGame;

    public List<LocationJSON> lsLocation = new List<LocationJSON>();

    public void Start()
    {
        LoadDataPlayer();
    }

    public void SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.dollars = GameManager.Instance.dollar;
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
            lc._Forest._PosCar = GameManager.Instance.lsLocation[i]._Forest._PosCar;
            lc._LsWorking = new TypeOfWorkST[GameManager.Instance.lsLocation[i]._LsWorking.Length];
            for (int j = 0; j < GameManager.Instance.lsLocation[i]._LsWorking.Length; j++)
            {
                lc._LsWorking[j]._Name = GameManager.Instance.lsLocation[i]._LsWorking[j]._Name;
                lc._LsWorking[j]._Material = GameManager.Instance.lsLocation[i]._LsWorking[j]._Material;
                lc._LsWorking[j]._NumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._NumberOfMaterialsSent;
                lc._LsWorking[j]._MaxNumberOfMaterialsSent = GameManager.Instance.lsLocation[i]._LsWorking[j]._MaxNumberOfMaterialsSent;
                lc._LsWorking[j]._PosCar = GameManager.Instance.lsLocation[i]._LsWorking[j]._PosCar;
                lc._LsWorking[j]._TimeWorking = GameManager.Instance.lsLocation[i]._LsWorking[j]._TimeWorking;
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
