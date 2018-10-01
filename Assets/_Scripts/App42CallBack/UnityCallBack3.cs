using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityCallBack3 : App42CallBack
{
    public void OnSuccess(object response)
    {
        Storage storage = (Storage)response;
        IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();
        SaveGold saveGold = JsonUtility.FromJson<SaveGold>(jsonDocList[0].GetJsonDoc());
        GameManager.Instance.gold = saveGold.gold;
        PlayerPrefs.SetInt("GoldPre", PlayerPrefs.GetInt("Gold", 10));
        Debug.Log(GameManager.Instance.gold);
        //Mng.mng.ui.loading.SetActive(false);
    }
    public void OnException(Exception e)
    {
        //Mng.mng.ui.loading.SetActive(false);
    }
}
