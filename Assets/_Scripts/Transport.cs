using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Transport : MonoBehaviour
{
    public Location location;
    public int indexType;
    public int sizeway;
    public Transform carman;
    public bool isRun;
    public bool isOff;

    public Transform[] way;
    private int indexPos = 0;
    private bool isRetrograde;

    public UnityAction<int> ActionSented;
    public UnityAction<int> ActionReceived;

    public void Start()
    {
        if (!isOff)
        {
            if (location._LsWorking[indexType]._Material <= 0)
            {
                carman.transform.position = way[0].transform.position;
                carman.transform.right = way[indexPos + 1].transform.position - carman.transform.position;
            }
            else
            {
                indexPos = 0;
                carman.gameObject.SetActive(true);
                carman.transform.position = way[0].transform.position;
                carman.transform.right = way[indexPos + 1].transform.position - carman.transform.position;
                ActionSented(indexType);
                isRun = true;
            }
        }
    }

    public void CarRun()
    {
        if (!isOff)
        {
            if (isRun)
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
                            ActionReceived(indexType);
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
                            ActionSented(indexType);
                        }
                    }
                }
            }
            else
            {
                if (location._LsWorking[indexType]._Material > 0
                    && GameManager.Instance.gold >= location._LsWorking[indexType]._PriceTransportSent)
                {
                    isRun = true;
                }
            }
        }
    }

    public void Update()
    {
        CarRun();
    }

    public void ResetAll()
    {
        if (!isRun)
        {
            indexPos = 0;
            carman.gameObject.SetActive(true);
            carman.transform.position = way[0].transform.position;
            carman.transform.right = way[indexPos + 1].transform.position - carman.transform.position;
            ActionSented(indexType);
            isRun = true;
        }
    }
}
