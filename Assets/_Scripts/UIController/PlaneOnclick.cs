using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlaneOnclick : MonoBehaviour
{
    public GameObject imgGive;
    public GameObject give;
    public Transform posIns;

    private bool isOnclick;

    public void BtnOnClick()
    {
        if (isOnclick)
            return;
        imgGive.SetActive(false);
        give.SetActive(true);
        give.transform.position = posIns.position;
        give.GetComponent<Rigidbody2D>().gravityScale = 0.15f;
        Invoke("OpenPopupAds", 0.5f);
        isOnclick = true;
    }

    public void OpenPopupAds()
    {
        Invoke("ActiveGive", 5f);
        give.GetComponent<Rigidbody2D>().gravityScale = 0f;
        give.SetActive(false);
        Ads.Instance.panelPlane.SetActive(true);
        int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        if (jobEnd == -1)
        {
            locationEnd--;
            jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        }
        double dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
        Ads.Instance.txtPlaneVideoAds.text = UIManager.Instance.ConvertNumber(dollarRecive * 5) + "$";
        Ads.Instance.txtPlaneReciveDollar.text = UIManager.Instance.ConvertNumber(dollarRecive / 10) + "$";
    }

    void ActiveGive()
    {
        imgGive.SetActive(true);
        isOnclick = false;
    }
}
