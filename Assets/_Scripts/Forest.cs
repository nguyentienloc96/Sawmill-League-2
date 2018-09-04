using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    public Location location;

    public int index = 0;

    public bool isRun;

    public GameObject car;

    public GameObject[] lsTree;

    private float speedCar = 0.5f;

    private bool isGrow;

    public bool isFull;

    public void Start()
    {
        lsTree = new GameObject[16];
        location._Forest._Tree = 16;
        for (int i = 0; i < lsTree.Length; i++)
        {
            lsTree[i] = transform.GetChild(i).gameObject;
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
        isFull = false;
    }

    public void GrowTreesStart()
    {
        isRun = true;
        car.transform.GetChild(1).gameObject.SetActive(false);
        lsTree[index].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }

    public void Update()
    {
        if (isRun)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, lsTree[index + 1].transform.position, speedCar * Time.deltaTime);
            if (index + 1 < lsTree.Length && car.transform.position == lsTree[index + 1].transform.position)
            {
                index++;
                lsTree[index].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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
    }

    public void GrowTrees()
    {
        foreach (GameObject obj in lsTree)
        {
            obj.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), GameConfig.Instance.fellingTime);
        }
        isGrow = false;
        location._Forest._Tree = 16;
        Invoke("FullTree",GameConfig.Instance.fellingTime);
    }

    public void FullTree()
    {
        isFull = true;
    }
}
