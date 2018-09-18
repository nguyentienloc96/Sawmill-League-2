using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
//using UnityEngine.Advertisements;

public class Ads : MonoBehaviour {
    [Header("Admob")]
    InterstitialAd interstitalAd;
    RewardBasedVideoAd rewardVideo;

    bool isLoadAds = false;
    //bool isShowAds = false;

    [Header("Time")]
    public float timeAds = 1;

    public static Ads Instance = new Ads();
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
        //PlayerPrefs.DeleteAll();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize("ca-app-pub-4738062221647171~1956094015");
        //RequestAd();
        MobileAds.SetiOSAppPauseOnBackground(true);
    }

    void Update()
    {
        timeAds += Time.deltaTime;
        ShowInterstitialAd();
    }

    #region ===ADMOB===


    public void RequestAd()
    {

#if UNITY_ANDROID
        GameConfig.Instance.idInter_android = "ca-app-pub-6285794272989840/5632501293"; //test
        if (!isLoadAds && GameConfig.Instance.idInter_android != null)
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_android);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#elif UNITY_IOS
            if (!isLoadAds && GameConfig.Instance.idInter_ios != null && GameConfig.Instance.idInter_ios != "")
            {
                interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_ios);
                AdRequest requestInterAd = new AdRequest.Builder().Build();
                interstitalAd.LoadAd(requestInterAd);
                isLoadAds = true;
                Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
            }
#else
            GameConfig.Instance.idInter_android = "ca-app-pub-6285794272989840/5632501293"; //test
            if (!isLoadAds && GameConfig.Instance.idInter_android != null)
            {
                interstitalAd = new InterstitialAd(GameConfig.Instance.idInter_android);
                AdRequest requestInterAd = new AdRequest.Builder().Build();
                interstitalAd.LoadAd(requestInterAd);
                isLoadAds = true;
                Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
            }
#endif

    }

    public void ShowInterstitialAd()
    {
        if (timeAds < GameConfig.Instance.timeInterAd)
        {
            RequestAd();
            return;
        }

        if (interstitalAd != null)
        {
            if (interstitalAd.IsLoaded())
            {
                interstitalAd.Show();
                isLoadAds = false;
                timeAds = 0;
                Debug.Log("Show Ads");
            }
        }
        else
        {
            Debug.Log("Null");
            RequestAd();
        }
    }
    #endregion

    #region ===UNITY ADS===
    //void HidePanelInfo()
    //{
    //    WorldManager.Instance.panelInfo.SetActive(false);
    //}

    public void SuccessAdsUnity()
    {
        Debug.Log("Cong tien : " + GameConfig.Instance.dollarVideoAd);
    }
    #endregion
}
