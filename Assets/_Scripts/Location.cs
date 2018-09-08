using System.Collections.Generic;
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
    public string _Name;
    public Image _Icon;
    public long _Material;
    public long _MaterialReceive;
    public Transport _TransportType;
    public long _NumberOfMaterialsSent;
    public long _MaxNumberOfMaterialsSent;
    public Text _TextNumberOfMaterial;
    public float _TimeWorking;
    public long _PriceMaterial;
    public long _Price;
    public bool _isCanAuto;
}

public class Location : MonoBehaviour
{
    public int _ID;
    public string _Name;
    public int _CountType = 0;
    public int _IndexType = 0;
    public Mask _MaskLocation;

    public ForestST _Forest;
    public TypeOfWorkST[] _LsWorking;

    public void Start()
    {
        _LsWorking[0]._TransportType.ActionSented = SentedWood;
        _LsWorking[0]._TransportType.ActionReceived = ReceivedWood;
    }

    #region FellingTransport
    public void SentedWood()
    {
        if (_LsWorking[0]._Material >= _LsWorking[0]._MaxNumberOfMaterialsSent)
        {
            _LsWorking[0]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = (_LsWorking[0]._MaxNumberOfMaterialsSent).ToString();
            _LsWorking[0]._Material -= _LsWorking[0]._MaxNumberOfMaterialsSent;
            _LsWorking[0]._NumberOfMaterialsSent = _LsWorking[0]._MaxNumberOfMaterialsSent;
            _LsWorking[0]._TextNumberOfMaterial.text = UIManager.Instance.ConvertMoney(_LsWorking[0]._Material);
        }
        else
        {
            _LsWorking[0]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = _LsWorking[0]._Material.ToString();
            _LsWorking[0]._NumberOfMaterialsSent = _LsWorking[0]._Material;
            _LsWorking[0]._Material = 0;
            _LsWorking[0]._TextNumberOfMaterial.text = UIManager.Instance.ConvertMoney(_LsWorking[0]._Material);
        }
    }

    public void ReceivedWood()
    {
        _LsWorking[0]._TransportType.carman.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        if (_CountType == 0)
        {
            GameManager.Instance.gold += _LsWorking[0]._NumberOfMaterialsSent * 15;
            _LsWorking[0]._NumberOfMaterialsSent = 0;
        }
        else
        {
            _LsWorking[0]._MaterialReceive += _LsWorking[0]._NumberOfMaterialsSent;
            _LsWorking[0]._NumberOfMaterialsSent = 0;
        }
    }

    public void FellingAuto()
    {
        if (_Forest._Tree > 0 && _Forest._ForestCode.isFull && !_LsWorking[0]._isCanAuto)
        {
            if (!_Forest._AnimFelling.activeInHierarchy)
                _Forest._AnimFelling.SetActive(true);
            _LsWorking[0]._TimeWorking += Time.deltaTime;
            if (_LsWorking[0]._TimeWorking >= 3f)
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
        _LsWorking[0]._TextNumberOfMaterial.text = UIManager.Instance.ConvertMoney(_LsWorking[0]._Material);
        if (_Forest._Tree <= 0)
        {
            _Forest._ForestCode.ResetForest();
        }
        if (_LsWorking[0]._Material > 0)
        {
            _LsWorking[0]._TransportType.ResetAll();
        }
    }
    #endregion

    public void Update()
    {
        FellingAuto();
    }

    public void BuildManager(int type)
    {
        if (_CountType >= type)
        {
            _IndexType = type;
            UIManager.Instance.LoadBuildWork(0);
        }
        else
        {
            if (_CountType + 1 == type)
            {
                UIManager.Instance.BuildSell.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _LsWorking[type]._Price.ToString();
                UIManager.Instance.LoadBuildWork(1);
            }
        }
    }

    public void BuildCar(int type)
    {
        UIManager.Instance.LoadBuildWork(2);
    }

}
