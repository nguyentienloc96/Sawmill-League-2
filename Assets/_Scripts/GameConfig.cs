using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance = new GameConfig();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public int goldToDollar;
    public int dollarVideoAd;
    public int timeInterAd;
    public List<string> lstMapWorld = new List<string>();
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
}
