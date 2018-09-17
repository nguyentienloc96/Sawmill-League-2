using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Others : MonoBehaviour
{

    public Location location;

    public Sprite[] arrPrefab;

    public List<Image> lsPoint;

    public void LoadOtherRandom()
    {
        location.lsOther = new List<int>();
        for (int i = 0; i < lsPoint.Count; i++)
        {
            int random = Random.Range(0, arrPrefab.Length);
            lsPoint[i].sprite = arrPrefab[random];
            location.lsOther.Add(random);
        }
        location.isLoaded = true;
    }

    public void LoadOtherJson()
    {
        for (int i = 0; i < lsPoint.Count; i++)
        {
            lsPoint[i].sprite = arrPrefab[location.lsOther[i]];
        }
        location.isLoaded = true;
    }
}
