using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forest : MonoBehaviour
{
    public Location location;

    public int index = 0;

    public bool isRun;

    public GameObject car;

    public GameObject[] lsTree;

    private float speedCar = 0.5f;

    private bool isGrow;

    public bool isGrowed;

    public bool isAutoPlant;

    private GameObject popupAutoPlant;

    public void LoadTree()
    {
        if (location.forest.tree > 0)
        {
            car.SetActive(false);
            for (int i = 0; i < lsTree.Length; i++)
            {
                lsTree[i].transform.localScale = new Vector3(1f, 1f, 1f);
                if (i < lsTree.Length - location.forest.tree)
                {
                    lsTree[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    lsTree[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            isGrowed = true;
        }
    }

    public void ResetForest()
    {
        index = 0;
        car.SetActive(true);
        car.transform.GetChild(1).gameObject.SetActive(true);
        car.transform.position = lsTree[index].transform.position;
        car.transform.right = lsTree[index + 1].transform.position - lsTree[index].transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            lsTree[i].transform.localScale = new Vector3(0f, 0f, 0f);
            lsTree[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        isGrowed = false;
        isAutoPlant = true;

        if (location.forest.isAutoPlant)
        {
            RunCarGrow();
        }
    }

    public void BtnRunCarGrow()
    {
        if (!location.forest.isAutoPlant)
        {
            RunCarGrow();
        }
    }

    public void RunCarGrow()
    {
        isRun = true;
        car.transform.GetChild(1).gameObject.SetActive(false);
        lsTree[index].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        Destroy(popupAutoPlant);
        isAutoPlant = false;
    }

    public void Update()
    {
        if (isRun)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, lsTree[index + 1].transform.position, speedCar * Time.deltaTime);
            if (index + 1 < lsTree.Length && car.transform.position == lsTree[index + 1].transform.position)
            {
                index++;
                lsTree[index].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                if (index + 1 < lsTree.Length)
                {
                    car.transform.right = lsTree[index + 1].transform.position - lsTree[index].transform.position;
                }
                else
                {
                    isRun = false;
                    car.SetActive(false);
                    isGrow = true;
                }
            }
        }
        else
        {
            if (isGrow)
            {
                GrowTrees();
            }
        }
        if (isAutoPlant)
        {
            long money = (long)(GameConfig.Instance.AutoPlant * (float)location.lsWorking[0].price);
            if (location.id != GameManager.Instance.IDLocation && GameManager.Instance.dollar >= money)
            {
                string str = "Do you want to spend  " + UIManager.Instance.ConvertNumber(money) + "$ Auto Reforestation in " + location.name;

                isAutoPlant = false;
            }
        }
    }

    public void GrowTrees()
    {
        for (int i = 0; i < lsTree.Length; i++)
        {
            if (i == 0)
            {
                lsTree[i].transform.DOScale(new Vector3(1f, 1f, 1f), GameConfig.Instance.growTime)
                    .OnComplete(() =>
                    {
                        isGrowed = true;
                    });
            }
            else
            {
                lsTree[i].transform.DOScale(new Vector3(1f, 1f, 1f), GameConfig.Instance.growTime);
            }
        }
        location.forest.tree = lsTree.Length;
        isGrow = false;
    }
}
