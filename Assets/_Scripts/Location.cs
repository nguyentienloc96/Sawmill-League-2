using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ForestST
{
    public int _Tree;
    public GameObject _AnimFelling;
    public Forest _ForestCode;
}

[System.Serializable]
public struct TypeOfWorkST
{
    [Header("Information")]
    public string _Name;
    public int _ID;
    public int _Level;

    [Header("Parameters")]
    public long _Material;
    public long _MaterialReceive;
    public long _PriceMaterial;

    public long _NumberMaterialIsMade; //C0
    public float _TimeWorking;
    public bool _IsCanAuto;

    [Header("Transport")]
    public Transport _TransportType;
    public long _LevelTransport;
    public long _PriceUpgradeCar;
    public long _PriceTransportSent; //X0
    public long _NumberOfMaterialsSent;
    public long _MaxNumberOfMaterialsSent; //CI0

    [Header("UI")]
    public Image _Icon;
    public Text _TextMaterial;

    [Header("Price")]
    public long _PriceUpgrade;
    public long _Price; //P0
    public float _UN2;
}

public class Location : MonoBehaviour
{
    public int _ID;
    public string _Name;
    public int _CountType = 0;
    public int _IndexType = 0;
    public Mask _MaskLocation;

    public int capIndex = 4;
    public int captruckIndex = 4;

    public bool isCanStart;

    public ForestST _Forest;
    public TypeOfWorkST[] _LsWorking;

    public void Start()
    {
        if (!isCanStart)
        {
            LoadInfoTypeOfWorkST();
            GetActionTransports();
        }
    }

    public void GetActionTransports()
    {
        for (int i = 0; i < _LsWorking.Length; i++)
        {
            _LsWorking[i]._TransportType.ActionSented = SentedMaterial;
            _LsWorking[i]._TransportType.ActionSented.Invoke(i);
            _LsWorking[i]._TransportType.ActionReceived = ReceivedMaterial;
            _LsWorking[i]._TransportType.ActionReceived.Invoke(i);
        }
    }

    public void LoadInfoTypeOfWorkST()
    {
        for (int i = 0; i < _LsWorking.Length; i++)
        {
            int _IDTypeOfWorkST = i + 1;
            _LsWorking[i]._ID = _IDTypeOfWorkST;
            _LsWorking[i]._UN2 = GameConfig.Instance.UN2;
            _LsWorking[i]._Price = (long)(GameConfig.Instance.p0 * Mathf.Pow(GameConfig.Instance.p0i, _IDTypeOfWorkST));
            _LsWorking[i]._NumberMaterialIsMade = (long)(GameConfig.Instance.c0 * Mathf.Pow(GameConfig.Instance.c0i, _IDTypeOfWorkST));
            _LsWorking[i]._PriceUpgrade = (long)(_LsWorking[i]._Price * GameConfig.Instance.UN1i);
            _LsWorking[i]._PriceTransportSent = (long)(GameConfig.Instance.x0 * Mathf.Pow(GameConfig.Instance.x0i, _IDTypeOfWorkST));
            _LsWorking[i]._MaxNumberOfMaterialsSent = _LsWorking[i]._NumberMaterialIsMade;
            _LsWorking[i]._PriceUpgradeCar = (long)(_LsWorking[i]._Price * GameConfig.Instance.XN1i);
        }
    }

    public string UpgradeInfoTypeOfWorkST()
    {
        string stInfo = "";
        if (GameManager.Instance.gold >= _LsWorking[_IndexType]._PriceUpgrade)
        {
            GameManager.Instance.gold -= _LsWorking[_IndexType]._PriceUpgrade;
            // Update thông số
            _LsWorking[_IndexType]._Level++;
            _LsWorking[_IndexType]._NumberMaterialIsMade = (long)((GameConfig.Instance.c0 * Mathf.Pow(2, _LsWorking[_IndexType]._ID)) * (1 + _LsWorking[_IndexType]._Level / capIndex));
            _LsWorking[_IndexType]._PriceUpgrade = (long)(_LsWorking[_IndexType]._PriceUpgrade * Mathf.Pow((1 + _LsWorking[_IndexType]._UN2), (_LsWorking[_IndexType]._Level - 1)));
            stInfo = _Name + "\n"
                + "Level : " + _LsWorking[_IndexType]._Level + "\n"
                + "Capacity : " + _LsWorking[_IndexType]._NumberMaterialIsMade + "\n"
                + "Price Upgrade : " + _LsWorking[_IndexType]._PriceUpgrade;
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }

    public string UpgradeInfoCar()
    {
        string stInfo = "";
        if (GameManager.Instance.gold >= _LsWorking[_IndexType]._PriceUpgradeCar)
        {
            GameManager.Instance.gold -= _LsWorking[_IndexType]._PriceUpgradeCar;
            // Update thông số
            _LsWorking[_IndexType]._LevelTransport++;
            _LsWorking[_IndexType]._MaxNumberOfMaterialsSent = _LsWorking[_IndexType]._MaxNumberOfMaterialsSent + _LsWorking[_IndexType]._MaxNumberOfMaterialsSent * _LsWorking[_IndexType]._LevelTransport / captruckIndex;
            _LsWorking[_IndexType]._PriceTransportSent = (long)(_LsWorking[_IndexType]._PriceTransportSent * Mathf.Pow((1 + GameConfig.Instance.XT2), (_LsWorking[_IndexType]._LevelTransport - 1)));
            _LsWorking[_IndexType]._PriceUpgradeCar = (long)(_LsWorking[_IndexType]._PriceUpgradeCar * Mathf.Pow((1 + GameConfig.Instance.XN2), (_LsWorking[_IndexType]._LevelTransport - 1)));
            stInfo = _Name + "\n"
                + "Level : " + _LsWorking[_IndexType]._LevelTransport + "\n"
                + "Capacity : " + _LsWorking[_IndexType]._MaxNumberOfMaterialsSent + "\n"
                + "Transportation Fee : " + _LsWorking[_IndexType]._PriceTransportSent + "\n"
                + "Price Upgrade : " + _LsWorking[_IndexType]._PriceUpgradeCar;
        }
        else
        {
            stInfo = "Not Enough Money !!!";
        }

        return stInfo;
    }

    public string CheckInfoTypeOfWorkST()
    {
        string stInfo = "";
        stInfo = _Name + "\n"
                + "Level : " + _LsWorking[_IndexType]._Level + "\n"
                + "Capacity : " + _LsWorking[_IndexType]._NumberMaterialIsMade + "\n"
                + "Price Upgrade : " + _LsWorking[_IndexType]._PriceUpgrade;
        return stInfo;
    }

    public string CheckInfoCar()
    {
        string stInfo = "";
        stInfo = _Name + "\n"
                + "Level : " + _LsWorking[_IndexType]._LevelTransport + "\n"
                + "Capacity : " + _LsWorking[_IndexType]._MaxNumberOfMaterialsSent + "\n"
                + "Transportation Fee : " + _LsWorking[_IndexType]._PriceTransportSent + "\n"
                + "Price Upgrade : " + _LsWorking[_IndexType]._PriceUpgradeCar;
        return stInfo;
    }

    public void SentedMaterial(int indexBegin)
    {
        if (GameManager.Instance.gold >= _LsWorking[indexBegin]._PriceTransportSent)
        {
            GameManager.Instance.gold -= _LsWorking[indexBegin]._PriceTransportSent;
            if (_LsWorking[indexBegin]._Material >= _LsWorking[indexBegin]._MaxNumberOfMaterialsSent)
            {
                _LsWorking[indexBegin]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = UIManager.Instance.ConvertNumber(_LsWorking[indexBegin]._MaxNumberOfMaterialsSent);
                _LsWorking[indexBegin]._Material -= _LsWorking[indexBegin]._MaxNumberOfMaterialsSent;
                _LsWorking[indexBegin]._NumberOfMaterialsSent = _LsWorking[indexBegin]._MaxNumberOfMaterialsSent;
                _LsWorking[indexBegin]._TextMaterial.text = UIManager.Instance.ConvertNumber(_LsWorking[indexBegin]._Material);
            }
            else
            {
                _LsWorking[indexBegin]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = UIManager.Instance.ConvertNumber(_LsWorking[indexBegin]._Material);
                _LsWorking[indexBegin]._NumberOfMaterialsSent = _LsWorking[indexBegin]._Material;
                _LsWorking[indexBegin]._Material = 0;
                _LsWorking[indexBegin]._TextMaterial.text = UIManager.Instance.ConvertNumber(_LsWorking[indexBegin]._Material);
            }
        }
        else
        {
            _LsWorking[indexBegin]._TransportType.isRun = false;
        }
    }

    public void ReceivedMaterial(int indexBegin)
    {
        _LsWorking[indexBegin]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        if (_CountType <= indexBegin)
        {
            GameManager.Instance.gold += _LsWorking[indexBegin]._NumberOfMaterialsSent * _LsWorking[indexBegin]._PriceMaterial;
            _LsWorking[indexBegin]._NumberOfMaterialsSent = 0;
        }
        else
        {
            _LsWorking[indexBegin + 1]._MaterialReceive += _LsWorking[indexBegin]._NumberOfMaterialsSent;
            _LsWorking[indexBegin]._NumberOfMaterialsSent = 0;
        }
    }

    #region Felling
    public void FellingAuto()
    {
        if (_Forest._Tree > 0 && _Forest._ForestCode.isFull && !_LsWorking[0]._IsCanAuto)
        {
            if (!_Forest._AnimFelling.activeInHierarchy)
                _Forest._AnimFelling.SetActive(true);
            _LsWorking[0]._TimeWorking += Time.deltaTime;
            if (_LsWorking[0]._TimeWorking >= GameConfig.Instance.p0Time)
            {
                FellingComplete();
                _LsWorking[0]._TimeWorking = 0;
            }
        }
        else
        {
            if (_Forest._AnimFelling.activeInHierarchy)
                _Forest._AnimFelling.SetActive(false);
        }
    }

    public void FellingComplete()
    {
        if (_Forest._Tree > 0)
        {
            _Forest._ForestCode.lsTree[_Forest._ForestCode.lsTree.Length - _Forest._Tree].transform.GetChild(0).gameObject.SetActive(false);
            _Forest._Tree--;
        }
        _LsWorking[0]._Material += (long)(10000 / _Forest._ForestCode.transform.childCount);
        _LsWorking[0]._TextMaterial.text = UIManager.Instance.ConvertNumber(_LsWorking[0]._Material);
        if (_Forest._Tree <= 0)
        {
            _Forest._ForestCode.ResetForest();
        }
        if (_LsWorking[0]._ID < _LsWorking.Length)
        {
            if (_LsWorking[0]._Material > 0)
            {
                _LsWorking[0]._TransportType.ResetAll();
            }
        }
    }
    #endregion

    #region Debarking
    public void DebarkingAuto()
    {
        if (_LsWorking[1]._MaterialReceive > 0 && !_LsWorking[1]._IsCanAuto)
        {
            _LsWorking[1]._TimeWorking += Time.deltaTime;
            if (_LsWorking[1]._TimeWorking >= GameConfig.Instance.p0Time)
            {
                DebarkingComplete();
                _LsWorking[1]._TimeWorking = 0;
            }
        }
    }

    public void DebarkingComplete()
    {
        long materialCurrent = 0;
        if (_LsWorking[1]._MaterialReceive >= _LsWorking[1]._NumberMaterialIsMade)
        {
            materialCurrent = _LsWorking[1]._NumberMaterialIsMade;
        }
        else
        {
            materialCurrent = _LsWorking[1]._MaterialReceive;
        }
        _LsWorking[1]._MaterialReceive -= materialCurrent;
        _LsWorking[1]._Material += (long)(0.9f * materialCurrent);
        _LsWorking[1]._TextMaterial.text = UIManager.Instance.ConvertNumber(_LsWorking[1]._Material);
        if (_LsWorking[1]._ID < _LsWorking.Length)
        {
            if (_LsWorking[1]._Material > 0)
            {
                _LsWorking[1]._TransportType.ResetAll();
            }
        }
    }
    #endregion

    public void Update()
    {
        FellingAuto();
        DebarkingAuto();
    }

    public void BuildManager(int type)
    {
        AudioManager.Instance.Play("Click");
        _IndexType = type;
        if (_CountType >= type)
        {
            UIManager.Instance.LoadBuildWork(0);
        }
        else
        {
            if (_CountType + 1 == type)
            {
                UIManager.Instance.BuildSell.transform
                    .GetChild(0).GetChild(0)
                    .GetComponent<Text>().text
                    = UIManager.Instance.ConvertNumber(_LsWorking[type]._Price);
                UIManager.Instance.LoadBuildWork(1);
            }
        }
    }

    public void BuildCar(int type)
    {
        AudioManager.Instance.Play("Click");
        _IndexType = type;
        UIManager.Instance.LoadBuildWork(2);
    }

}
