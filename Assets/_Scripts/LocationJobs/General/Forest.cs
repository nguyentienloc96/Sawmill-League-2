﻿using DG.Tweening;
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
        car.transform.GetChild(1).GetComponent<Image>().enabled = true;
        car.transform.position = lsTree[index].transform.position;
        car.transform.right = lsTree[index + 1].transform.position - lsTree[index].transform.position;

        for (int i = 0; i < transform.childCount; i++)
        {
            lsTree[i].transform.localScale = new Vector3(0f, 0f, 0f);
            lsTree[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        isGrowed = false;

        if (location.forest.isAutoPlant)
        {
            RunCarGrow();
        }
    }

    public void BtnRunCarGrow()
    {
        if (!location.forest.isAutoPlant && location.countType >= 0)
        {
            RunCarGrow();
        }
    }

    public void RunCarGrow()
    {
        car.transform.GetChild(1).GetComponent<Image>().enabled = false;
        lsTree[index].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        isRun = true;
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
                    Vector3 angleCar = car.transform.localEulerAngles;
                    if (angleCar.z != 0)
                    {
                        if (index % 2 == 0)
                        {
                            angleCar.y = 180;
                        }
                        else
                        {
                            angleCar.y = 0;
                        }
                    }
                    angleCar.z = 0;

                    car.transform.localEulerAngles = angleCar;
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

        if (location.countType < 0)
        {
            if (car.transform.GetChild(0).GetComponent<Button>().interactable)
                car.transform.GetChild(0).GetComponent<Button>().interactable = false;
            if (car.transform.GetChild(1).gameObject.activeInHierarchy)
                car.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (!car.transform.GetChild(0).GetComponent<Button>().interactable)
                car.transform.GetChild(0).GetComponent<Button>().interactable = true;
            if (!car.transform.GetChild(1).gameObject.activeInHierarchy)
                car.transform.GetChild(1).gameObject.SetActive(true);
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
                        location.forest.tree = lsTree.Length;
                    });
            }
            else
            {
                lsTree[i].transform.DOScale(new Vector3(1f, 1f, 1f), GameConfig.Instance.growTime);
            }
        }
        isGrow = false;
    }
}