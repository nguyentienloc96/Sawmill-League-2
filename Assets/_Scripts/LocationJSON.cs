using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationJSON {

    public int _ID;
    public string _Name;
    public int _CountType = 0;

    public ForestST _Forest;
    public TypeOfWorkST[] _LsWorking;
}
