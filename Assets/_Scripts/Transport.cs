using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Transport : MonoBehaviour
{
    public Location location;
    public int indexType;
    public int sizeway;
    public Transform carman;

    public Transform[] way;
    private int indexPos = 0;
    private bool isRetrograde;
    private bool isPause;

    public UnityAction ActionSented;
    public UnityAction ActionReceived;

    public void Start()
    {
        carman.transform.right = way[indexPos + 1].transform.position - carman.transform.position;
        isPause = true;
    }

    public void CarRun()
    {
        if (!isPause)
        {
            if (!isRetrograde)
            {
                carman.transform.position = Vector3.MoveTowards(carman.transform.position, way[indexPos + 1].transform.position, 0.5f * Time.deltaTime);
                if (carman.transform.position == way[indexPos + 1].transform.position)
                {
                    indexPos++;
                    if (indexPos + 1 < way.Length)
                    {
                        carman.transform.GetChild(0).right = way[indexPos + 1].transform.position - carman.transform.position;
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
                carman.transform.position = Vector3.MoveTowards(carman.transform.position, way[indexPos - 1].transform.position, 0.5f * Time.deltaTime);
                if (carman.transform.position == way[indexPos - 1].transform.position)
                {
                    indexPos--;
                    if (indexPos > 0)
                    {
                        carman.transform.GetChild(0).right = way[indexPos - 1].transform.position - carman.transform.position;
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
            carman.transform.right = way[indexPos + 1].transform.position - carman.transform.position;

            isPause = false;
            indexPos = 0;
            ActionSented();
        }
    }
}
