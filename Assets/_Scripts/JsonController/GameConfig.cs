using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
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

    void Start()
    {
        if (id == "")
        {
            App42API.Initialize("70d071010820cea793a6fecd4a573a7551c630da18878944b24d09b4a50c1212", "48faebb001dc22c3d0b453bcc351fe45120e3f9660ef458f939c9f2055ee352e");
            GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    id = Social.localUser.id;
                    StorageService storageService = App42API.BuildStorageService();
                    storageService.FindDocumentByKeyValue("Db", "Data", "id", id, new UnityCallBack1());
                }
                else
                    Debug.Log("Failed to authenticate");
            });
        }
    }

    public int goldToDollar;
    public int dollarVideoAd;
    public int timeInterAd;
    public int fellingTime;
    public int p0;
    public float p0Time;
    public float c0;
    public float productCost;
    public float p0i;
    public float c0i;
    public float UN2;
    public float UN1i;
    public int x0;
    public float x0i;
    public float XN2;
    public float XN1i;
    public float XT2;
    public float XT1i;
    public float captruckIndex;
    public string idInter_android;
    public string idInter_ios;
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
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
