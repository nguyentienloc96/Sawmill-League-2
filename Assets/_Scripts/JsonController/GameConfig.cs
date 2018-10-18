using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SocialPlatforms.GameCenter;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance;
    public static string id = "";
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public long dollarStart;
    public long goldStart;
    public int goldToDollar;
    public int dollarVideoAd;
    public int timeInterAd;
    public int fellingTime;
    public int growTime;
    public int p0;
    public float p0Time;
    public float c0;
    public float productCost;
    public float p0i;
    public float c0i;
    public float r;
    public float UN2;
    public float UN1i;
    public float truckTime;
    public int x0;
    public float x0i;
    public float XN2;
    public float XN1i;
    public float XT2;
    public float XT1i;
    public float capIndex;
    public float captruckIndex;
    public float WYS;
    public float AutoPlant;
    public float TruckSpeed;
    public float TimeForest;
    public string idInter_android;
    public string idInter_ios;
    public string idBanner_ios;
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
    string app42_apiKey = "41b8289bb02efae4f37f1c9d891b09bb43f6f801bdbbf17a557bc4598ddf836b";
    string app42_secretKey = "35d9a321b8d4cfc3b375b5f212f15ffab98bb2b53e4b9da20d22881fc01a0efa";

    void Start()
    {
        if (id == "")
        {
            App42API.Initialize(app42_apiKey, app42_secretKey);
            //GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    id = Social.localUser.id;
                    //Debug.Log(id);
                    StorageService storageService = App42API.BuildStorageService();
                    storageService.FindDocumentByKeyValue("Db", "Data", "id", id, new UnityCallBack1());
                }
                else
                    Debug.Log("Failed to authenticate");
            });
        }
    }
}

public class SaveGold
{
    public string id;
    public int gold;
    public SaveGold(string id, int gold)
    {
        this.id = id;
        this.gold = gold;
    }
}
