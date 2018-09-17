using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDataJson : MonoBehaviour
{
    public static LoadDataJson Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private string gameConfig = "GameConfig";

    void Start () {
        LoadGameConfig();
    }

    public void LoadGameConfig()
    {
        var objJson = SimpleJSON.JSON.Parse(loadJson(gameConfig));
        //Debug.Log(objJson);
        //Debug.Log("<color=yellow>Done: </color>LoadGameConfig !");
        if (objJson != null)
        {
            GameConfig.Instance.goldToDollar = objJson["goldToDollar"].AsInt;
            GameConfig.Instance.dollarVideoAd = objJson["dollarVideoAd"].AsInt;
            GameConfig.Instance.timeInterAd = objJson["timeInterAd"].AsInt;
            GameConfig.Instance.fellingTime = objJson["fellingTime"].AsInt;
            GameConfig.Instance.p0 = objJson["p0"].AsInt;
            GameConfig.Instance.p0Time = objJson["p0Time"].AsFloat;
            GameConfig.Instance.c0 = objJson["c0"].AsFloat;
            GameConfig.Instance.productCost = objJson["productCost"].AsFloat;
            GameConfig.Instance.p0i = objJson["p0i"].AsFloat;
            GameConfig.Instance.c0i = objJson["c0i"].AsFloat;
            GameConfig.Instance.UN2 = objJson["UN2"].AsFloat;
            GameConfig.Instance.UN1i = objJson["UN1i"].AsFloat;
            GameConfig.Instance.x0 = objJson["x0"].AsInt;
            GameConfig.Instance.x0i = objJson["x0i"].AsFloat;
            GameConfig.Instance.XN2 = objJson["XN2"].AsFloat;
            GameConfig.Instance.XN1i = objJson["XN1i"].AsFloat;
            GameConfig.Instance.XT2 = objJson["XT2"].AsFloat;
            GameConfig.Instance.XT1i = objJson["XT1i"].AsFloat;
            GameConfig.Instance.captruckIndex = objJson["captruckIndex"].AsFloat;
            GameConfig.Instance.idInter_android = objJson["idInter_android"];
            GameConfig.Instance.idInter_ios = objJson["idInter_ios"];
        }
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }

}
