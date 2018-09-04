using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUpdate : MonoBehaviour
{
    public static DataUpdate Instance = new DataUpdate();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public struct DataNameCountry
    {
        public string name;
    }

    public List<DataNameCountry> lstData_NameCountry = new List<DataNameCountry>();
}
