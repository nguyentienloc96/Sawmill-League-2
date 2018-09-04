using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Transport : MonoBehaviour
{
    public Location location;

    public int sizeway;
    public Transform carman;

    private Transform[] way;
    private int indexRun = 0;
    private bool isRetrograde;
    private bool isPause;

    public UnityAction ActionSented;
    public UnityAction ActionReceived;

    public void Start()
    {
        way = new Transform[sizeway];
        for (int i = 0; i < sizeway; i++)
        {
            way[i] = transform.GetChild(i);
        }
        carman.transform.right = way[indexRun + 1].transform.position - carman.transform.position;
        if(location._LsWorking[0]._Material <= 0)
        {
            isPause = true;
        }
    }

    public void CarRun()
    {
        if (!isPause)
        {
            if (!isRetrograde)
            {
                carman.transform.position = Vector3.MoveTowards(carman.transform.position, way[indexRun + 1].transform.position, 0.5f * Time.deltaTime);
                if (carman.transform.position == way[indexRun + 1].transform.position)
                {
                    indexRun++;
                    if (indexRun + 1 < way.Length)
                    {
                        carman.transform.GetChild(0).right = way[indexRun + 1].transform.position - carman.transform.position;
                        carman.transform.DORotate(carman.transform.GetChild(0).eulerAngles, 0.25f);
                    }
                    else
                    {
                        carman.transform.right = way[way.Length - 2].transform.position - carman.transform.position;
                        isRetrograde = true;
                        ActionReceived();
                    }
                }
            }
            else
            {
                carman.transform.position = Vector3.MoveTowards(carman.transform.position, way[indexRun - 1].transform.position, 0.5f * Time.deltaTime);
                if (carman.transform.position == way[indexRun - 1].transform.position)
                {
                    indexRun--;
                    if (indexRun > 0)
                    {
                        carman.transform.GetChild(0).right = way[indexRun - 1].transform.position - carman.transform.position;
                        carman.transform.DORotate(new Vector3(0f, 0f, carman.transform.GetChild(0).eulerAngles.y + carman.transform.GetChild(0).eulerAngles.z), 0.25f);
                    }
                    else
                    {
                        carman.transform.right = way[1].transform.position - carman.transform.position;
                        isRetrograde = false;
                        ActionSented();
                    }
                }
            }
        }
    }

    public void Update()
    {
        CarRun();
    }

    public void PauseRun()
    {
        isPause = true;
    }

    public void ResetAll()
    {
        if (isPause)
        {
            carman.gameObject.SetActive(true);
            carman.transform.position = way[0].transform.position;
            carman.transform.eulerAngles = new Vector3(0f, 0f, -90f);
            carman.transform.right = way[indexRun + 1].transform.position - carman.transform.position;

            isPause = false;
            indexRun = 0;
            ActionSented();
        }
    }

}
