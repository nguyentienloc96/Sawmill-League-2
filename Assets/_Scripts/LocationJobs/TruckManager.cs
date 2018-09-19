using UnityEngine;
using UnityEngine.UI;

public class TruckManager : MonoBehaviour
{

    public Location location;
    public GameObject truck;
    public Text txtSent;

    public int indexType;
    public bool isRun;
    public bool isOff;

    public Transform[] way;
    private int indexPos = 0;
    private bool isRetrograde;

    public void LoadTruck()
    {
        if (!isRun)
        {
            indexPos = 0;
            truck.gameObject.SetActive(true);
            truck.transform.position = way[0].transform.position;
            truck.transform.right = way[indexPos + 1].transform.position - truck.transform.position;
            SentedOutput();
            isRun = true;
        }
    }

    public void TruckRun()
    {
        if (!isOff)
        {
            if (isRun)
            {
                if (!isRetrograde)
                {
                    truck.transform.position = Vector3.MoveTowards(truck.transform.position, way[indexPos + 1].transform.position, 0.25f * Time.deltaTime);
                    if (truck.transform.position == way[indexPos + 1].transform.position)
                    {
                        indexPos++;
                        if (indexPos + 1 < way.Length)
                        {
                            truck.transform.right = way[indexPos + 1].transform.position - truck.transform.position;
                        }
                        else
                        {
                            truck.transform.right = way[way.Length - 2].transform.position - truck.transform.position;
                            isRetrograde = true;
                            ReceivedOutput();
                        }
                    }
                }
                else
                {
                    truck.transform.position = Vector3.MoveTowards(truck.transform.position, way[indexPos - 1].transform.position, 0.25f * Time.deltaTime);
                    if (truck.transform.position == way[indexPos - 1].transform.position)
                    {
                        indexPos--;
                        if (indexPos > 0)
                        {
                            truck.transform.right = way[indexPos - 1].transform.position - truck.transform.position;
                        }
                        else
                        {
                            truck.transform.right = way[1].transform.position - truck.transform.position;
                            isRetrograde = false;
                            SentedOutput();
                        }
                    }
                }
            }
            else
            {
                if (location.lsWorking[indexType].output > 0
                    && GameManager.Instance.gold >= location.lsWorking[indexType].priceTruckSent)
                {
                    isRun = true;
                }
            }
        }
    }

    public void LateUpdate()
    {
        TruckRun();
    }

    public void SentedOutput()
    {
        if (GameManager.Instance.gold >= location.lsWorking[indexType].priceTruckSent)
        {
            GameManager.Instance.gold -= location.lsWorking[indexType].priceTruckSent;

            if (location.lsWorking[indexType].output >= location.lsWorking[indexType].maxSent)
            {
                location.lsWorking[indexType].truckManager.txtSent.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].maxSent);
                location.lsWorking[indexType].output -= location.lsWorking[indexType].maxSent;
                location.lsWorking[indexType].currentSent = location.lsWorking[indexType].maxSent;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else if(location.lsWorking[indexType].output > 0)
            {
                long outputSent = location.lsWorking[indexType].output;
                location.lsWorking[indexType].truckManager.txtSent.text = UIManager.Instance.ConvertNumber(outputSent);
                location.lsWorking[indexType].currentSent = outputSent;
                location.lsWorking[indexType].output -= outputSent;
                location.lsWorking[indexType].textOutput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType].output);
            }
            else
            {
                location.lsWorking[indexType].truckManager.isRun = false;
            }
        }
        else
        {
            location.lsWorking[indexType].truckManager.isRun = false;
        }
    }

    public void ReceivedOutput()
    {
        location.lsWorking[indexType].truckManager.txtSent.text = "";
        if (location.countType <= indexType)
        {
            GameManager.Instance.gold += location.lsWorking[indexType].currentSent * location.lsWorking[indexType].priceOutput;
            location.lsWorking[indexType].currentSent = 0;
        }
        else
        {
            location.lsWorking[indexType + 1].input += location.lsWorking[indexType].currentSent;
            location.lsWorking[indexType + 1].textInput.text = UIManager.Instance.ConvertNumber(location.lsWorking[indexType + 1].input);
            location.lsWorking[indexType].currentSent = 0;
        }
    }

}
